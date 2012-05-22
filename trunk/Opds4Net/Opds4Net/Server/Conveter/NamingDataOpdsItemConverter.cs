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
        private IOpdsLinkGenerator linkGenerator = null;
        private IDataTypeDetector dataTypeDetector = null;
        private IAccessorFactory accessorFactory = null;

        /// <summary>
        /// A syndication link generator, provides information of the OPDS Site.
        /// </summary>
        public IOpdsLinkGenerator LinkGenerator
        {
            get
            {
                if (linkGenerator == null)
                    linkGenerator = CreateLinkGenerator();

                return linkGenerator;
            }
            set { linkGenerator = value; }
        }

        /// <summary>
        /// A detector used to detect the data type of a given object.
        /// </summary>
        public IDataTypeDetector TypeDetector
        {
            get
            {
                if (dataTypeDetector == null)
                    dataTypeDetector = CreateDataTypeDetector() ?? new OpdsDataDetector();

                return dataTypeDetector;
            }
            set { dataTypeDetector = value; }
        }

        /// <summary>
        /// A factory used to create property accessor according to a given object.
        /// </summary>
        public IAccessorFactory AccessorFactory
        {
            get
            {
                if (accessorFactory == null)
                    accessorFactory = CreateAccessorFactory() ?? AdaptedAccessorFactory.Instance;

                return accessorFactory;
            }
            set { accessorFactory = value; }
        }

        /// <summary>
        /// Gets or sets the names of object that map to sydication item property.
        /// </summary>
        public OpdsNames Names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NamingDataOpdsItemConverter()
        {
            Names = new OpdsNames();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        [ImportingConstructor]
        public NamingDataOpdsItemConverter([Import]IOpdsLinkGenerator linkGenerator)
            : this()
        {
            if (linkGenerator == null)
                throw new ArgumentNullException("linkGenerator");

            LinkGenerator = linkGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IAccessorFactory CreateAccessorFactory()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IDataTypeDetector CreateDataTypeDetector()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IOpdsLinkGenerator CreateLinkGenerator()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ItemGeneratedEventArgs> ItemGenerated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public OpdsItems GetItems(OpdsData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return new OpdsItems() { Items = ConvertDataItems(data) };
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

        private IEnumerable<SyndicationItem> ConvertDataItems(OpdsData data)
        {
            // Assuming every item is of different type.
            // PropertyAdapter should be retreived for every item.
            foreach (var item in data.Data ?? new IOpdsDataTypeHost[] { })
            {
                var accessor = AccessorFactory.GetAccessor(item);
                var dataType = TypeDetector.DetectType(item);
                if (dataType == OpdsDataType.Category)
                {
                    var syndicationItem = CreateBasicDataItems(accessor, item);
                    var navigationLink = LinkGenerator.GetNavigationLink(syndicationItem.Id, String.Empty);
                    if (navigationLink != null)
                    {
                        var count = item.GetProperty(Names.Count, accessor);
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
                var price = item.GetProperty(Names.Price, accessor);
                if (price != null)
                {
                    var buyLinkId = item.GetProperty(Names.BuyLinkId, accessor).ToNullableString() ?? syndicationItem.Id;
                    var buyLink = LinkGenerator.GetBuyLink(buyLinkId, String.Empty, Convert.ToDecimal(price));
                    if (buyLink != null)
                    {
                        buyLink.Prices.Single().CurrencyCode = item.GetProperty(Names.CurrencyCode, accessor).ToNullableString() ?? "CNY";
                        syndicationItem.Links.Add(buyLink);
                    }
                }

                // 下载链接
                var downloadLinkId = item.GetProperty(Names.DownloadLinkId, accessor).ToNullableString() ?? syndicationItem.Id;
                var downloadLink = LinkGenerator.GetDownloadLink(downloadLinkId, String.Empty);
                if (downloadLink != null)
                {
                    downloadLink.MediaType = item.GetProperty(Names.MimeType, accessor).ToNullableString();
                    downloadLink.Length = Convert.ToInt64(item.GetProperty(Names.Size, accessor));
                    downloadLink.Uri = new Uri(item.GetProperty(Names.DownloadAddress, accessor).ToNullableString() ?? downloadLink.Uri.ToString(), UriKind.RelativeOrAbsolute);
                    syndicationItem.Links.Add(downloadLink);
                }

                // 其它详细信息
                syndicationItem.Content = item.GetProperty(Names.Content, accessor).MakeSyndicationContent();
                syndicationItem.Copyright = item.GetProperty(Names.Copyright, accessor).MakeSyndicationContent();
                syndicationItem.ISBN = item.GetProperty(Names.Isbn, accessor).ToNullableString();
                syndicationItem.Language = item.GetProperty(Names.Language, accessor).ToNullableString();
                syndicationItem.Issued = item.GetProperty(Names.IssueTime, accessor).ToNullableString();
                syndicationItem.Extent = item.GetProperty(Names.Extent, accessor).ToNullableString();
                var relevance = item.GetProperty(Names.Relevance, accessor);
                if (relevance != null)
                    syndicationItem.Relevance = Convert.ToDouble(relevance);
                var publishDate = item.GetProperty(Names.PublishDate, accessor);
                if (publishDate != null && (DateTime)publishDate != DateTime.MinValue)
                    syndicationItem.PublishDate = new DateTimeOffset(Convert.ToDateTime(publishDate));
                var categories = item.GetProperty(Names.CategoryInfo, accessor);
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
                else
                {
                    FillInCategoryInfo(syndicationItem, item, accessor, Names.CategoryPrefix);
                }
                ExtracPersonInfo(item, syndicationItem.Contributors, Names.ContributorPrefix, accessor);
            }
            // 书籍列表项
            else
            {
                // 详细页链接的Id和书籍的Id可能并没有对应关系。仅当没有提供详细页Id时，使用书籍的Id。
                var detailLinkId = item.GetProperty(Names.DetailLinkId, accessor).ToNullableString() ?? syndicationItem.Id;
                var detailLink = LinkGenerator.GetDetailLink(detailLinkId, String.Empty);
                if (detailLink != null)
                    syndicationItem.Links.Add(detailLink);
                else
                    throw new InvalidProgramException("LinkGenerator don't provide detail link");
            }

            // 图片链接
            var thumbnail = item.GetProperty(Names.Thumbnail, accessor).ToNullableString();
            if (thumbnail != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(thumbnail),
                    RelationshipType = OpdsRelations.Thumbnail,
                });
            var cover = item.GetProperty(Names.Cover, accessor).ToNullableString();
            if (cover != null)
                syndicationItem.Links.Add(new SyndicationLink()
                {
                    Uri = new Uri(cover),
                    RelationshipType = OpdsRelations.Cover,
                });

            syndicationItem.Publisher = item.GetProperty(Names.Publisher, accessor).ToNullableString();
            syndicationItem.Source = item.GetProperty(Names.Source, accessor).ToNullableString();
            OnSyndicationItemCreated(syndicationItem, item);

            return syndicationItem;
        }

        private OpdsItem CreateBasicDataItems(IPropertyAccessor accessor, object item)
        {
            var syndicationItem = new OpdsItem()
            {
                Title = item.GetProperty(Names.Title, accessor).MakeSyndicationContent(),
                Id = item.GetProperty(Names.Id, accessor).ToNullableString(),
                Summary = item.GetProperty(Names.Summary, accessor).MakeSyndicationContent(),
            };
            if (syndicationItem.Id == null)
                throw new InvalidProgramException(String.Format("Id is missing in the given data of type {0}.", item.GetType().FullName));

            var updateTime = item.GetProperty(Names.UpdateTime, accessor);
            if (updateTime != null && (DateTime)updateTime != DateTime.MinValue)
                syndicationItem.LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime(updateTime));
            ExtracPersonInfo(item, syndicationItem.Authors, Names.AuthorPrefix, accessor);

            return syndicationItem;
        }

        private bool FillInCategoryInfo(SyndicationItem syndicationItem, object data, IPropertyAccessor accessor = null, string propertyPrefix = null)
        {
            // name refer to id that must exists.
            var name = data.GetProperty(propertyPrefix + Names.CategoryName, accessor).ToNullableString();
            if (name == null)
                return false;

            // Represents what kind of information the category name defined.
            var schema = data.GetProperty(propertyPrefix + Names.CategorySchema, accessor).ToNullableString();
            // Label shows to the user.
            var label = data.GetProperty(propertyPrefix + Names.CategoryLabel, accessor).ToNullableString();
            syndicationItem.Categories.Add(new SyndicationCategory(name, schema, label));

            return true;
        }

        private bool FillInPersonInfo(ICollection<SyndicationPerson> persons, object data, IPropertyAccessor accessor = null, string propertyPrefix = null)
        {
            var authorName = data.GetProperty(propertyPrefix + Names.PersonName, accessor).ToNullableString();
            var authorEmail = data.GetProperty(propertyPrefix + Names.PersonEmail, accessor).ToNullableString();
            var authorSite = data.GetProperty(propertyPrefix + Names.PersonUrl, accessor).ToNullableString();
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

        private void ExtracPersonInfo(object item, ICollection<SyndicationPerson> people, string prefix, IPropertyAccessor accessor)
        {
            if (!FillInPersonInfo(people, item, accessor, prefix))
            {
                var contributors = item.GetProperty(prefix + Names.PersonPostfix, accessor);
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
