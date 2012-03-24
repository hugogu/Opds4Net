using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using Opds4Net.Model;
using Opds4Net.Util;
using Opds4Net.Util.Extension;

namespace Opds4Net.Server
{
    /// <summary>
    /// Provide the capability to generate OPDS Data form custom/existing object model/domain model.
    /// </summary>
    public abstract class NamingDataOpdsItemConverter : IOpdsItemConverter
    {
        /// <summary>
        /// Generate links of OPDS item. The link value is Site-relative.
        /// </summary>
        public IOpdsLinkGenerator LinkGenerator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDataTypeDetector TypeDetector { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        /// <param name="typeDetector"></param>
        public NamingDataOpdsItemConverter(IOpdsLinkGenerator linkGenerator, IDataTypeDetector typeDetector)
        {
            this.LinkGenerator = linkGenerator;
            this.TypeDetector = typeDetector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OpdsItemsResult GetItems(NamingDataSource request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            //if (request.PageIndex <= 0)
            //    throw new ArgumentException("pageIndex start from 1");

            //if (request.PageSize <= 0)
            //    throw new ArgumentException("pageSize should larger than 0");

            //var data = request.Process();

            //if (data.Data.Count() > request.PageSize)
            //    throw new InvalidProgramException("the data processor provide more items than requested.");

            return new OpdsItemsResult()
            {
                Items = ConvertDataItems(request)
            };
        }

        private IEnumerable<SyndicationItem> ConvertDataItems(NamingDataSource dataSource)
        {
            // Assuming every item is of different type.
            // PropertyAccessor should be retreived for every item.
            foreach (var item in dataSource.Data ?? new IOpdsData[] { })
            {
                var accessor = item.GetType().GetPropertyAccessor();
                var dataType = TypeDetector.DetectType(item);
                if (dataType == OpdsDataType.Category)
                {
                    var syndicationItem = CreateBasicDataItems(accessor, item);
                    syndicationItem.Links.Add(LinkGenerator.GetNavigationLink(syndicationItem.Id, String.Empty));
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
                var price = accessor.GetProperty(item, "Price");
                if (price != null)
                {
                    var buyLink = LinkGenerator.GetBuyLink(syndicationItem.Id, String.Empty, Convert.ToDecimal(price));
                    buyLink.Prices.Single().CurrencyCode = accessor.GetProperty(item, "CurrencyCode").ToNullableString() ?? "CNY";
                    syndicationItem.Links.Add(buyLink);
                }

                // 下载链接
                var downloadLink = LinkGenerator.GetDownloadLink(syndicationItem.Id, String.Empty);
                downloadLink.MediaType = accessor.GetProperty(item, "MimeType").ToNullableString();
                downloadLink.Length = Convert.ToInt64(accessor.GetProperty(item, "Length"));
                syndicationItem.Links.Add(downloadLink);

                // 其它详细信息
                syndicationItem.Content = accessor.GetProperty(item, "Content").MakeSyndicationContent();
                syndicationItem.Copyright = accessor.GetProperty(item, "Copyright").MakeSyndicationContent();
                syndicationItem.ISBN = accessor.GetProperty(item, "ISBN").ToNullableString();
                syndicationItem.Language = accessor.GetProperty(item, "Language").ToNullableString();
                syndicationItem.Issued = accessor.GetProperty(item, "IssueTime").ToNullableString();
                var publishDate = accessor.GetProperty(item, "PublishDate");
                if (publishDate != null && (DateTime)publishDate != DateTime.MinValue)
                    syndicationItem.PublishDate = new DateTimeOffset(Convert.ToDateTime(publishDate));
            }
            // 书籍列表项
            else
            {
                syndicationItem.Links.Add(LinkGenerator.GetDetailLink(syndicationItem.Id, String.Empty));
            }

            // 图片链接
            var thumbnail = accessor.GetProperty(item, "Thumbnail").ToNullableString();
            if (thumbnail != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(thumbnail),
                    RelationshipType = OpdsRelations.Thumbnail,
                });
            var cover = accessor.GetProperty(item, "Cover").ToNullableString();
            if (cover != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(cover),
                    RelationshipType = OpdsRelations.Cover,
                });

            syndicationItem.Publisher = accessor.GetProperty(item, "Publisher").ToNullableString();
            OnSyndicationItemCreated(syndicationItem, item);

            return syndicationItem;
        }

        private OpdsItem CreateBasicDataItems(IPropertyAccessor accessor, object item)
        {
            var syndicationItem = new OpdsItem()
            {
                Title = accessor.GetProperty(item, "Title").MakeSyndicationContent(),
                Id = accessor.GetProperty(item, "Id").ToNullableString(),
                Summary = accessor.GetProperty(item, "Summary").MakeSyndicationContent(),
            };
            if (syndicationItem.Id == null)
                throw new InvalidProgramException(String.Format("Id is missing in the given data of type {0}.", item.GetType().FullName));

            var updateTime = accessor.GetProperty(item, "UpdateTime");
            if (updateTime != null && (DateTime)updateTime != DateTime.MinValue)
                syndicationItem.LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime(updateTime));
            var author = accessor.GetProperty(item, "Author");
            if (author != null)
                syndicationItem.Authors.Add(new SyndicationPerson()
                {
                    Name = Convert.ToString(author),
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
        }
    }
}
