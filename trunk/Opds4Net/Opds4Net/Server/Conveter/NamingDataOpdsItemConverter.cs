using System;
using System.Collections;
using System.Collections.Generic;
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
        public OpdsItems GetItems(OpdsData request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return new OpdsItems() { Items = ConvertDataItems(request) };
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

        private IEnumerable<SyndicationItem> ConvertDataItems(OpdsData dataSource)
        {
            // Assuming every item is of different type.
            // PropertyAdapter should be retreived for every item.
            foreach (var item in dataSource.Data ?? new IOpdsData[] { })
            {
                var accessor = ComponentFactory.AdapterFactory.GetAccessor(item);
                var dataType = ComponentFactory.TypeDetector.DetectType(item);
                if (dataType == OpdsDataType.Category)
                {
                    var syndicationItem = CreateBasicDataItems(accessor, item);
                    var navigationLink = ComponentFactory.LinkGenerator.GetNavigationLink(syndicationItem.Id, String.Empty);
                    if (navigationLink != null)
                    {
                        var count = item.GetProperty("Count", accessor);
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
                    yield return BuildEntity(accessor, item, dataType == OpdsDataType.Detial);
                }
            }
        }

        private SyndicationItem BuildEntity(IPropertyAccessor accessor, object item, bool withDetail)
        {
            var syndicationItem = CreateBasicDataItems(accessor, item);

            // 详细页
            if (withDetail)
            {
                // 购买链接
                var price = item.GetProperty("Price", accessor);
                if (price != null)
                {
                    var buyLinkId = item.GetProperty("BuyLinkId", accessor).ToNullableString() ?? syndicationItem.Id;
                    var buyLink = ComponentFactory.LinkGenerator.GetBuyLink(buyLinkId, String.Empty, Convert.ToDecimal(price));
                    if (buyLink != null)
                    {
                        buyLink.Prices.Single().CurrencyCode = item.GetProperty("CurrencyCode", accessor).ToNullableString() ?? "CNY";
                        syndicationItem.Links.Add(buyLink);
                    }
                }

                // 下载链接
                var downloadLinkId = item.GetProperty("DownloadLinkId", accessor).ToNullableString() ?? syndicationItem.Id;
                var downloadLink = ComponentFactory.LinkGenerator.GetDownloadLink(downloadLinkId, String.Empty);
                if (downloadLink != null)
                {
                    downloadLink.MediaType = item.GetProperty("MimeType", accessor).ToNullableString();
                    downloadLink.Length = Convert.ToInt64(item.GetProperty("Length", accessor));
                    syndicationItem.Links.Add(downloadLink);
                }

                // 其它详细信息
                syndicationItem.Content = item.GetProperty("Content", accessor).MakeSyndicationContent();
                syndicationItem.Copyright = item.GetProperty("Copyright", accessor).MakeSyndicationContent();
                syndicationItem.ISBN = item.GetProperty("ISBN", accessor).ToNullableString();
                syndicationItem.Language = item.GetProperty("Language", accessor).ToNullableString();
                syndicationItem.Issued = item.GetProperty("IssueTime", accessor).ToNullableString();
                syndicationItem.Extent = item.GetProperty("Extent", accessor).ToNullableString();
                var relevance = item.GetProperty("Relevance", accessor);
                if (relevance != null)
                    syndicationItem.Relevance = Convert.ToDouble(relevance);
                var publishDate = item.GetProperty("PublishDate", accessor);
                if (publishDate != null && (DateTime)publishDate != DateTime.MinValue)
                    syndicationItem.PublishDate = new DateTimeOffset(Convert.ToDateTime(publishDate));
                var categories = item.GetProperty("CategoryInfo", accessor);
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
                ExtracPersonInfo(item, syndicationItem.Contributors, "Contributor", accessor);
            }
            // 书籍列表项
            else
            {
                // 详细页链接的Id和书籍的Id可能并没有对应关系。仅当没有提供详细页Id时，使用书籍的Id。
                var detailLinkId = item.GetProperty("DetailLinkId", accessor).ToNullableString() ?? syndicationItem.Id;
                var detailLink = ComponentFactory.LinkGenerator.GetDetailLink(detailLinkId, String.Empty);
                if (detailLink != null)
                    syndicationItem.Links.Add(detailLink);
                else
                    throw new InvalidProgramException("LinkGenerator don't provide detail link");
            }

            // 图片链接
            var thumbnail = item.GetProperty("Thumbnail", accessor).ToNullableString();
            if (thumbnail != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(thumbnail),
                    RelationshipType = OpdsRelations.Thumbnail,
                });
            var cover = item.GetProperty("Cover", accessor).ToNullableString();
            if (cover != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(cover),
                    RelationshipType = OpdsRelations.Cover,
                });

            syndicationItem.Publisher = item.GetProperty("Publisher", accessor).ToNullableString();
            OnSyndicationItemCreated(syndicationItem, item);

            return syndicationItem;
        }

        private OpdsItem CreateBasicDataItems(IPropertyAccessor accessor, object item)
        {
            var syndicationItem = new OpdsItem()
            {
                Title = item.GetProperty("Title", accessor).MakeSyndicationContent(),
                Id = item.GetProperty("Id", accessor).ToNullableString(),
                Summary = item.GetProperty("Summary", accessor).MakeSyndicationContent(),
            };
            if (syndicationItem.Id == null)
                throw new InvalidProgramException(String.Format("Id is missing in the given data of type {0}.", item.GetType().FullName));

            var updateTime = item.GetProperty("UpdateTime", accessor);
            if (updateTime != null && (DateTime)updateTime != DateTime.MinValue)
                syndicationItem.LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime(updateTime));
            ExtracPersonInfo(item, syndicationItem.Authors, "Author", accessor);

            return syndicationItem;
        }

        private static bool FillInCategoryInfo(SyndicationItem syndicationItem, object data, IPropertyAccessor accessor = null)
        {
            var name = data.GetProperty("Name", accessor).ToNullableString();
            if (name == null)
                return false;

            var schema = data.GetProperty("Schema", accessor).ToNullableString();
            var label = data.GetProperty("Label", accessor).ToNullableString();
            syndicationItem.Categories.Add(new SyndicationCategory(name, schema, label));

            return true;
        }

        private static bool FillInPersonInfo(ICollection<SyndicationPerson> persons, object data, IPropertyAccessor accessor = null, string propertyPrefix = null)
        {
            var authorName = data.GetProperty(propertyPrefix + "Name", accessor).ToNullableString();
            var authorEmail = data.GetProperty(propertyPrefix + "Email", accessor).ToNullableString();
            var authorSite = data.GetProperty(propertyPrefix + "Site", accessor).ToNullableString();
            if (authorName != null || authorEmail != null || authorSite != null)
            {
                persons.Add(new SyndicationPerson()
                {
                    Name = authorName,
                    Email = authorEmail,
                    Uri = authorSite,
                });

                return true;
            }

            return false;
        }

        private static void ExtracPersonInfo(object item, ICollection<SyndicationPerson> people, string prefix, IPropertyAccessor accessor)
        {
            if (!FillInPersonInfo(people, item, accessor, prefix))
            {
                var contributors = item.GetProperty(prefix + "Info", accessor);
                if (contributors != null)
                {
                    // 集合
                    if (contributors is IEnumerable)
                    {
                        foreach (var contributor in contributors as IEnumerable)
                        {
                            // we have no idea about the data type of the contributor 
                            // so the accessor cannot be reused.
                            FillInPersonInfo(people, contributor);
                        }
                    }
                    // 单个实例
                    else
                    {
                        FillInPersonInfo(people, contributors);
                    }
                }
            }
        }
    }
}
