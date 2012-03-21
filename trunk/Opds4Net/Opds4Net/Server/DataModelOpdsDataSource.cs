using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using Opds4Net.Model;
using Opds4Net.Util;
using Opds4Net.Util.Extension;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataModelOpdsDataSource : IOpdsDataSource
    {
        protected IOpdsLinkGenerator linkGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        public DataModelOpdsDataSource(IOpdsLinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }

        public IEnumerable<SyndicationItem> GetItems(string id)
        {
            var items = ExtractItems(id);

            if (items.Any())
            {
                var accessor = items.First().GetType().GetPropertyAccessor();
                foreach (var item in items)
                {
                    if (item.DataType == OpdsDataType.Category)
                    {
                        var syndicationItem = CreateBasicDataItems(accessor, item);
                        syndicationItem.Links.Add(linkGenerator.GetNavigationLink(syndicationItem.Id, String.Empty));
                        OnCategoryItemCreated(syndicationItem);

                        yield return syndicationItem;
                    }
                    else
                    {
                        yield return BuildEntity(accessor, item, false);
                    }
                }
            }
        }

        public SyndicationItem GetDetail(string id)
        {
            var detail = ExtraceDetail(id);
            var accessor = detail.GetType().GetPropertyAccessor();
            Debug.Assert(detail.DataType == OpdsDataType.Entity);

            return BuildEntity(accessor, detail, true);
        }

        private SyndicationItem BuildEntity(IPropertyAccessor accessor, IOpdsData item, bool withDetail)
        {            
            var syndicationItem = CreateBasicDataItems(accessor, item);

            // 详细页
            if (withDetail)
            {
                var downloadLink = linkGenerator.GetDownloadLink(syndicationItem.Id, String.Empty);
                downloadLink.Length = Convert.ToInt64(accessor.GetProperty(item, "Length"));
                downloadLink.MediaType = accessor.GetProperty(item, "MimeType").ToString();
                downloadLink.Prices.Add(new OpdsPrice(Convert.ToDecimal(accessor.GetProperty(item, "Price")))
                {
                    CurrencyCode = accessor.GetProperty(item, "CurrencyCode").ToNullableString() ?? "CNY"
                });
                syndicationItem.Links.Add(downloadLink);
                syndicationItem.Content = accessor.GetProperty(item, "Content").MakeSyndicationContent();
                syndicationItem.Copyright = accessor.GetProperty(item, "Copyright").MakeSyndicationContent();
                syndicationItem.ISBN = accessor.GetProperty(item, "ISBN").ToNullableString();
                syndicationItem.Language = accessor.GetProperty(item, "Language").ToNullableString();
                syndicationItem.Issued = accessor.GetProperty(item, "IssueTime").ToNullableString();
                var publishDate = accessor.GetProperty(item, "PublishDate");
                if (publishDate != null)
                    syndicationItem.PublishDate = new DateTimeOffset(Convert.ToDateTime(publishDate));
            }
            // 书籍列表项
            else
            {
                syndicationItem.Links.Add(linkGenerator.GetDetailLink(syndicationItem.Id, String.Empty));
            }

            syndicationItem.Publisher = accessor.GetProperty(item, "Publisher").ToNullableString();
            OnEntityItemCreated(syndicationItem);

            return syndicationItem;
        }

        private OpdsItem CreateBasicDataItems(IPropertyAccessor accessor, IOpdsData item)
        {
            
            var syndicationItem = new OpdsItem()
            {
                Title = accessor.GetProperty(item, "Title").MakeSyndicationContent(),
                Id = accessor.GetProperty(item, "Id").ToString(),
                Summary = accessor.GetProperty(item, "Summary").MakeSyndicationContent(),
            };
            var updateTime = accessor.GetProperty(item, "UpdateTime");
            if (updateTime != null)
                syndicationItem.LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime(updateTime));
            var author = accessor.GetProperty(item, "Author");
            if (author != null)
                syndicationItem.Authors.Add(new SyndicationPerson()
                {
                    Name = Convert.ToString(author),
                });

            return syndicationItem;
        }

        protected virtual void OnCategoryItemCreated(SyndicationItem item)
        {
        }

        protected virtual void OnEntityItemCreated(SyndicationItem item)
        {
        }

        protected abstract IEnumerable<IOpdsData> ExtractItems(string id);

        protected abstract IOpdsData ExtraceDetail(string id);
    }
}
