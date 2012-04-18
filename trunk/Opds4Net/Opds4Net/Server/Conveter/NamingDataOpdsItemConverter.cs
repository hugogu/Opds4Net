using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel.Syndication;
using Opds4Net.Model;
using Opds4Net.Reflection;
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
                var adapter = ComponentFactory.AdapterFactory.GetAdapter(item);
                var dataType = ComponentFactory.TypeDetector.DetectType(item);
                if (dataType == OpdsDataType.Category)
                {
                    var syndicationItem = CreateBasicDataItems(adapter, item);
                    var navigationLink = ComponentFactory.LinkGenerator.GetNavigationLink(syndicationItem.Id, String.Empty);
                    if (navigationLink != null)
                    {
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

        private SyndicationItem BuildEntity(IPropertyAdapter adapter, object item, bool withDetail)
        {
            var syndicationItem = CreateBasicDataItems(adapter, item);

            // 详细页
            if (withDetail)
            {
                // 购买链接
                var price = adapter.GetProperty(item, "Price");
                if (price != null)
                {
                    var buyLink = ComponentFactory.LinkGenerator.GetBuyLink(syndicationItem.Id, String.Empty, Convert.ToDecimal(price));
                    if (buyLink != null)
                    {
                        buyLink.Prices.Single().CurrencyCode = adapter.GetProperty(item, "CurrencyCode").ToNullableString() ?? "CNY";
                        syndicationItem.Links.Add(buyLink);
                    }
                }

                // 下载链接
                var downloadLink = ComponentFactory.LinkGenerator.GetDownloadLink(syndicationItem.Id, String.Empty);
                if (downloadLink != null)
                {
                    downloadLink.MediaType = adapter.GetProperty(item, "MimeType").ToNullableString();
                    downloadLink.Length = Convert.ToInt64(adapter.GetProperty(item, "Length"));
                    syndicationItem.Links.Add(downloadLink);
                }

                // 其它详细信息
                syndicationItem.Content = adapter.GetProperty(item, "Content").MakeSyndicationContent();
                syndicationItem.Copyright = adapter.GetProperty(item, "Copyright").MakeSyndicationContent();
                syndicationItem.ISBN = adapter.GetProperty(item, "ISBN").ToNullableString();
                syndicationItem.Language = adapter.GetProperty(item, "Language").ToNullableString();
                syndicationItem.Issued = adapter.GetProperty(item, "IssueTime").ToNullableString();
                var publishDate = adapter.GetProperty(item, "PublishDate");
                if (publishDate != null && (DateTime)publishDate != DateTime.MinValue)
                    syndicationItem.PublishDate = new DateTimeOffset(Convert.ToDateTime(publishDate));
            }
            // 书籍列表项
            else
            {
                // 详细页链接的Id和书籍的Id可能并没有对应关系。仅当没有提供详细页Id时，使用书籍的Id。
                var detailLinkId = adapter.GetProperty(item, "DetailLinkId").ToNullableString() ?? syndicationItem.Id;
                var detailLink = ComponentFactory.LinkGenerator.GetDetailLink(detailLinkId, String.Empty);
                if (detailLink != null)
                    syndicationItem.Links.Add(detailLink);
                else
                    throw new InvalidProgramException("LinkGenerator don't provide detail link");
            }

            // 图片链接
            var thumbnail = adapter.GetProperty(item, "Thumbnail").ToNullableString();
            if (thumbnail != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(thumbnail),
                    RelationshipType = OpdsRelations.Thumbnail,
                });
            var cover = adapter.GetProperty(item, "Cover").ToNullableString();
            if (cover != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(cover),
                    RelationshipType = OpdsRelations.Cover,
                });

            syndicationItem.Publisher = adapter.GetProperty(item, "Publisher").ToNullableString();
            OnSyndicationItemCreated(syndicationItem, item);

            return syndicationItem;
        }

        private OpdsItem CreateBasicDataItems(IPropertyAdapter adapter, object item)
        {
            var syndicationItem = new OpdsItem()
            {
                Title = adapter.GetProperty(item, "Title").MakeSyndicationContent(),
                Id = adapter.GetProperty(item, "Id").ToNullableString(),
                Summary = adapter.GetProperty(item, "Summary").MakeSyndicationContent(),
            };
            if (syndicationItem.Id == null)
                throw new InvalidProgramException(String.Format("Id is missing in the given data of type {0}.", item.GetType().FullName));

            var updateTime = adapter.GetProperty(item, "UpdateTime");
            if (updateTime != null && (DateTime)updateTime != DateTime.MinValue)
                syndicationItem.LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime(updateTime));
            var authorName = adapter.GetProperty(item, "AuthorName");
            var authorEmail = adapter.GetProperty(item, "AuthorEmail");
            var authorSite = adapter.GetProperty(item, "AuthorSite");
            if (authorName != null || authorEmail != null || authorSite != null)
                syndicationItem.Authors.Add(new SyndicationPerson()
                {
                    Name = authorName.ToNullableString(),
                    Email = authorEmail.ToNullableString(),
                    Uri = authorSite.ToNullableString(),
                });

            return syndicationItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        protected virtual void OnSyndicationItemCreated(SyndicationItem item, object data)
        {
            var temp = ItemGenerated;
            if (temp != null)
            {
                temp(this, new ItemGeneratedEventArgs(item, data));
            }
        }
    }
}
