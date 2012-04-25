using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel.Syndication;
using Opds4Net.Model;
using Opds4Net.Reflection;
using Opds4Net.Reflection.Extension;
using Opds4Net.Util;

namespace Opds4Net.Server
{
    /// <summary>
    /// Provide the capability to generate OPDS Data form custom/existing object model/domain model.
    /// </summary>
    [Export("DataModel", typeof(IOpdsItemConverter))]
    public class NamingDataOpdsItemConverter : IOpdsItemConverter
    {
        /// <summary>
        /// Generate links of OPDS item. The link value is Site-relative.
        /// </summary>
        public IOpdsItemConverterComponentFactory ComponentFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentFactory"></param>
        [ImportingConstructor]
        public NamingDataOpdsItemConverter([Import]IOpdsItemConverterComponentFactory componentFactory)
        {
            if (componentFactory == null)
                throw new ArgumentNullException("componentFactory");

            ComponentFactory = componentFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ItemGeneratedEventArgs> ItemGenerated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OpdsItemsResult GetItems(OpdsDataSource request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return new OpdsItemsResult() { Items = ConvertDataItems(request) };
        }

        private IEnumerable<SyndicationItem> ConvertDataItems(OpdsDataSource dataSource)
        {
            // Assuming every item is of different type.
            // PropertyAdapter should be retreived for every item.
            foreach (var item in dataSource.Data ?? new IOpdsData[] { })
            {
                var adapter = ComponentFactory.AdapterFactory.GetAccessor(item);
                var dataType = ComponentFactory.TypeDetector.DetectType(item);
                if (dataType == OpdsDataType.Category)
                {
                    var syndicationItem = CreateBasicDataItems(adapter, item);
                    var navigationLink = ComponentFactory.LinkGenerator.GetNavigationLink(syndicationItem.Id, String.Empty);
                    if (navigationLink != null)
                    {
                        var count = item.GetProperty("Count", adapter);
                        if (count != null)
                        {
                            navigationLink.Count = Convert.ToInt32(count);
                        }
                        syndicationItem.Links.Add(navigationLink);
                    }
                    OnSyndicationItemCreated(syndicationItem, item);

                    yield return syndicationItem;
                }
                else
                {
                    yield return BuildEntity(adapter, item, dataType == OpdsDataType.Detial);
                }
            }
        }

        private SyndicationItem BuildEntity(IPropertyAccessor adapter, object item, bool withDetail)
        {
            var syndicationItem = CreateBasicDataItems(adapter, item);

            // 详细页
            if (withDetail)
            {
                // 购买链接
                var price = item.GetProperty("Price", adapter);
                if (price != null)
                {
                    var buyLinkId = item.GetProperty("BuyLinkId", adapter).ToNullableString() ?? syndicationItem.Id;
                    var buyLink = ComponentFactory.LinkGenerator.GetBuyLink(buyLinkId, String.Empty, Convert.ToDecimal(price));
                    if (buyLink != null)
                    {
                        buyLink.Prices.Single().CurrencyCode = item.GetProperty("CurrencyCode", adapter).ToNullableString() ?? "CNY";
                        syndicationItem.Links.Add(buyLink);
                    }
                }

                // 下载链接
                var downloadLinkId = item.GetProperty("DownloadLinkId", adapter).ToNullableString() ?? syndicationItem.Id;
                var downloadLink = ComponentFactory.LinkGenerator.GetDownloadLink(downloadLinkId, String.Empty);
                if (downloadLink != null)
                {
                    downloadLink.MediaType = item.GetProperty("MimeType", adapter).ToNullableString();
                    downloadLink.Length = Convert.ToInt64(item.GetProperty("Length", adapter));
                    syndicationItem.Links.Add(downloadLink);
                }

                // 其它详细信息
                syndicationItem.Content = item.GetProperty("Content", adapter).MakeSyndicationContent();
                syndicationItem.Copyright = item.GetProperty("Copyright", adapter).MakeSyndicationContent();
                syndicationItem.ISBN = item.GetProperty("ISBN", adapter).ToNullableString();
                syndicationItem.Language = item.GetProperty("Language", adapter).ToNullableString();
                syndicationItem.Issued = item.GetProperty("IssueTime", adapter).ToNullableString();
                syndicationItem.Extent = item.GetProperty("Extent", adapter).ToNullableString();
                var relevance = item.GetProperty("Relevance", adapter);
                if (relevance != null)
                    syndicationItem.Relevance = Convert.ToDouble(relevance);
                var publishDate = item.GetProperty("PublishDate", adapter);
                if (publishDate != null && (DateTime)publishDate != DateTime.MinValue)
                    syndicationItem.PublishDate = new DateTimeOffset(Convert.ToDateTime(publishDate));
                var categories = item.GetProperty("CategoryInfo", adapter);
                if (categories != null)
                {
                    if (categories is IEnumerable)
                    {
                        foreach (var category in categories as IEnumerable)
                        {
                            FillInCategoryInfo(syndicationItem, category);
                        }
                    }
                    else if (categories is string)
                    {
                        syndicationItem.Categories.Add(new SyndicationCategory(categories as string));
                    }
                    else
                    {
                        FillInCategoryInfo(syndicationItem, categories);
                    }
                }
                var contributors = item.GetProperty("ContributorInfo", adapter);
                if (contributors != null)
                {
                    if (contributors is IEnumerable)
                    {
                        foreach (var contributor in contributors as IEnumerable)
                        {
                            FillInPersonInfo(syndicationItem.Contributors, contributor);
                        }
                    }
                    else
                    {
                        FillInPersonInfo(syndicationItem.Contributors, item, adapter, "Contributor");
                    }
                }
            }
            // 书籍列表项
            else
            {
                // 详细页链接的Id和书籍的Id可能并没有对应关系。仅当没有提供详细页Id时，使用书籍的Id。
                var detailLinkId = item.GetProperty("DetailLinkId", adapter).ToNullableString() ?? syndicationItem.Id;
                var detailLink = ComponentFactory.LinkGenerator.GetDetailLink(detailLinkId, String.Empty);
                if (detailLink != null)
                    syndicationItem.Links.Add(detailLink);
                else
                    throw new InvalidProgramException("LinkGenerator don't provide detail link");
            }

            // 图片链接
            var thumbnail = item.GetProperty("Thumbnail", adapter).ToNullableString();
            if (thumbnail != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(thumbnail),
                    RelationshipType = OpdsRelations.Thumbnail,
                });
            var cover = item.GetProperty("Cover", adapter).ToNullableString();
            if (cover != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(cover),
                    RelationshipType = OpdsRelations.Cover,
                });

            syndicationItem.Publisher = item.GetProperty("Publisher", adapter).ToNullableString();
            OnSyndicationItemCreated(syndicationItem, item);

            return syndicationItem;
        }

        private bool FillInCategoryInfo(SyndicationItem syndicationItem, object data, IPropertyAccessor accessor = null)
        {
            var name = data.GetProperty("Name", accessor).ToNullableString();
            if (name == null)
                return false;

            var schema = data.GetProperty("Schema", accessor).ToNullableString();
            var label = data.GetProperty("Label", accessor).ToNullableString();
            syndicationItem.Categories.Add(new SyndicationCategory(name, schema, label));

            return true;
        }

        private bool FillInPersonInfo(Collection<SyndicationPerson> persons, object data, IPropertyAccessor accessor = null, string propertyPrefix = null)
        {
            var authorName = data.GetProperty(propertyPrefix + "Name", accessor);
            var authorEmail = data.GetProperty(propertyPrefix + "Email", accessor);
            var authorSite = data.GetProperty(propertyPrefix + "Site", accessor);
            if (authorName != null || authorEmail != null || authorSite != null)
            {
                persons.Add(new SyndicationPerson()
                {
                    Name = authorName.ToNullableString(),
                    Email = authorEmail.ToNullableString(),
                    Uri = authorSite.ToNullableString(),
                });

                return true;
            }

            return false;
        }

        private OpdsItem CreateBasicDataItems(IPropertyAccessor adapter, object item)
        {
            var syndicationItem = new OpdsItem()
            {
                Title = item.GetProperty("Title", adapter).MakeSyndicationContent(),
                Id = item.GetProperty("Id", adapter).ToNullableString(),
                Summary = item.GetProperty("Summary", adapter).MakeSyndicationContent(),
            };
            if (syndicationItem.Id == null)
                throw new InvalidProgramException(String.Format("Id is missing in the given data of type {0}.", item.GetType().FullName));

            var updateTime = item.GetProperty("UpdateTime", adapter);
            if (updateTime != null && (DateTime)updateTime != DateTime.MinValue)
                syndicationItem.LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime(updateTime));
            FillInPersonInfo(syndicationItem.Authors, item, adapter, "Author");

            return syndicationItem;
        }

        /// <summary>
        /// Raise the ItemGenerated event.
        /// </summary>
        /// <param name="item">An instance of SyndicationItem generated.</param>
        /// <param name="data">Data used to generate Syndication item.</param>
        protected virtual void OnSyndicationItemCreated(SyndicationItem item, object data)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (data == null)
                throw new ArgumentNullException("data");

            var temp = ItemGenerated;
            if (temp != null)
            {
                temp(this, new ItemGeneratedEventArgs(item, data));
            }
        }
    }
}
