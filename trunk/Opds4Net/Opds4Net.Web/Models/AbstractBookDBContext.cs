using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Opds4Net.Web.Models
{
    public abstract class AbstractBookDBContext : DbContext
    {
        private static readonly PickerCategory emptyCategory = new PickerCategory { Id = Guid.Empty, FullName = String.Empty };

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PickerCategory> PickCategories { get; set; }

        public PickerCategory NoCategory { get { return emptyCategory; } }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<PickerCategory> PickCategoriesWithEmpty
        {
            get
            {
                yield return emptyCategory;

                foreach (var category in PickCategories)
                {
                    yield return category;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Book> Books { get; set; }
    }
}