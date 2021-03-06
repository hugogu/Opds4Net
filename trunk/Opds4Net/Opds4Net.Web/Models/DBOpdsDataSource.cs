﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Syndication;
using Opds4Net.Server;

namespace Opds4Net.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DBOpdsDataSource
    {
        private AbstractBookDBContext dbContext;
        private IOpdsItemConverter itemConverter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="itemConverter"></param>
        [ImportingConstructor]
        public DBOpdsDataSource(
            [Import]AbstractBookDBContext dbContext,
            [Import("DataModel")]IOpdsItemConverter itemConverter)
        {
            this.dbContext = dbContext;
            this.itemConverter = itemConverter;
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
        public OpdsItems GetItems(IDataRequest request)
        {
            var result = new OpdsItems();
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
                current = dbContext.Categories.Include(c => c.Books).Single(c => c.Id == new Guid(request.Id));
                if (current.Books.Any())
                {
                    result.Items = GetItems(current.Books).Concat(result.Items).ToList();
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
            book.OpdsDataType = OpdsDataType.Detial;
            var result = itemConverter.GetItems(new OpdsData(new[] { book })).Items.Single();
            book.OpdsDataType = OpdsDataType.Entity;

            return result.Value;
        }

        private IEnumerable<KeyValuePair<object, SyndicationItem>> GetItems(IEnumerable<object> categories)
        {
            if (categories != null)
            {
                return itemConverter.GetItems(new OpdsData(categories)).Items;
            }

            return new List<KeyValuePair<object, SyndicationItem>>();
        }
    }
}
