﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Syndication;
using Opds4Net.Model;
using Opds4Net.Server;

namespace Opds4Net.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Export("DB", typeof(IOpdsDataSource))]
    public class DBOpdsDataSource : IOpdsDataSource
    {
        private AbstractBookDBContext dbContext;
        private IOpdsLinkGenerator linkGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        [ImportingConstructor]
        public DBOpdsDataSource(
            [Import]AbstractBookDBContext dbContext,
            [Import]IOpdsLinkGenerator linkGenerator)
        {
            this.dbContext = dbContext;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractBookDBContext DbContext
        {
            get { return dbContext; }
            set { dbContext = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OpdsItemsResult GetItems(IDataRequest request)
        {
            var result = new OpdsItemsResult();
            // 取主分类
            if (request.Id == null)
            {
                result.Items = GetItems(dbContext.Categories.Where(c => c.Parent == null));
            }
            else
            {
                var current = dbContext.Categories.Include(c => c.SubCategories).Single(c => c.Id == new Guid(request.Id));
                // 取子分类
                if (current.SubCategories != null && current.SubCategories.Any())
                {
                    result.Items = GetItems(dbContext.Categories.Where(c => c.Parent != null && c.Parent.Id == new Guid(request.Id)));
                }
                // 无子分类，则取书
                else
                {
                    result.Items = GetItems(current.Books);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SyndicationItem GetDetail(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var book = dbContext.Books.Single(b => b.Id == new Guid(id));

            var item = GetItems(new[] { book }).Single();
            var downloadLink = linkGenerator.GetDownloadLink(id, "下载");
            downloadLink.MediaType = book.MimeType;
            downloadLink.Length = book.FileSize;
            item.Links.Clear();
            item.Links.Add(downloadLink);

            if (book.SalePrice > 0.0M)
            {
                item.Links.Add(linkGenerator.GetBuyLink(id, "购买", book.SalePrice));
            }

            return item;
        }

        private IEnumerable<OpdsItem> GetItems(IEnumerable<Category> categories)
        {
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    var item = new OpdsItem()
                    {
                        Id = category.Id.ToString(),
                        Title = new TextSyndicationContent(category.Name),
                    };
                    var link = linkGenerator.GetNavigationLink(category.Id.ToString(), category.Name);
                    if (category.SubCategories != null && category.SubCategories.Count > 0)
                        link.Count = category.SubCategories.Count;
                    else if (category.Books != null && category.Books.Count > 0)
                        link.Count = category.Books.Count;

                    item.Links.Add(link);

                    yield return item;
                }
            }
        }

        private IEnumerable<OpdsItem> GetItems(IEnumerable<Book> books)
        {
            if (books != null)
            {
                foreach (var book in books)
                {
                    var item = new OpdsItem()
                    {
                        Id = book.Id.ToString(),
                        Title = new TextSyndicationContent(book.Name),
                        LastUpdatedTime = book.UpdateTime,
                    };
                    item.Authors.Add(new SyndicationPerson() { Name = book.Author });
                    item.Links.Add(linkGenerator.GetDetailLink(book.Id.ToString(), book.Name));

                    yield return item;
                }
            }
        }
    }
}
