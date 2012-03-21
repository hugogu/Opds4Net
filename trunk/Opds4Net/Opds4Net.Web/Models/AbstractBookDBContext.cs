using System.Data.Entity;

namespace Opds4Net.Web.Models
{
    public abstract class AbstractBookDBContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PickerCategory> PickCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Book> Books { get; set; }
    }
}