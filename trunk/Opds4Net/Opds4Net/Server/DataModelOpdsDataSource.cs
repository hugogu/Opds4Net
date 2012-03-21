using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using Opds4Net.Model;
using Opds4Net.Util.Extension;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataModelOpdsDataSource : IOpdsDataSource
    {
        private IOpdsLinkGenerator linkGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        [ImportingConstructor]
        public DataModelOpdsDataSource([Import]IOpdsLinkGenerator linkGenerator)
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
                    CurrencyCode = Convert.ToString(accessor.GetProperty(item, "CurrencyCode")) ?? "CNY"
                });
                syndicationItem.Links.Add(downloadLink);
                syndicationItem.Content = new TextSyndicationContent(Convert.ToString(accessor.GetProperty(item, "Content")));
                syndicationItem.Copyright = new TextSyndicationContent(Convert.ToString(accessor.GetProperty(item, "Copyright")));
                syndicationItem.ISBN = Convert.ToString(accessor.GetProperty(item, "ISBN"));
                syndicationItem.Language = Convert.ToString(accessor.GetProperty(item, "Language"));
                syndicationItem.Issued = Convert.ToString(accessor.GetProperty(item, "IssueTime"));
                syndicationItem.PublishDate = new DateTimeOffset(Convert.ToDateTime(accessor.GetProperty(item, "PublishDate")));
            }
            // 书籍列表项
            else
            {
                syndicationItem.Links.Add(linkGenerator.GetDetailLink(syndicationItem.Id, String.Empty));
            }

            syndicationItem.Publisher = Convert.ToString(accessor.GetProperty(item, "Publisher"));
            OnEntityItemCreated(syndicationItem);

            return syndicationItem;
        }

        private OpdsItem CreateBasicDataItems(IPropertyAccessor accessor, IOpdsData item)
        {
            var syndicationItem = new OpdsItem()
            {
                Title = new TextSyndicationContent(Convert.ToString(accessor.GetProperty(item, "Title"))),
                Id = accessor.GetProperty(item, "Id").ToString(),
                Summary = new TextSyndicationContent(Convert.ToString(accessor.GetProperty(item, "Summary"))),
                LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime(accessor.GetProperty(item, "UpdateTime"))),
            };
            syndicationItem.Authors.Add(new SyndicationPerson()
            {
                Name = Convert.ToString(accessor.GetProperty(item, "Author")),
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
