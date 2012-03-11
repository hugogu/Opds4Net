using System.Data.Entity;

namespace Opds4Net.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BookDBContext : DbContext
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PickerCategory>().ToTable("PickerCategory");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .Map(c =>
                {
                    c.MapLeftKey("BookId");
                    c.MapRightKey("CategoryId");
                    c.ToTable("BookCategory");
                });

            modelBuilder.Entity<Category>()
                .HasMany<Book>(c => c.Books)
                .WithMany(b => b.Categories)
                .Map(c => {
                    c.MapLeftKey("CategoryId");
                    c.MapRightKey("BookId");
                    c.ToTable("BookCategory");
                });

            modelBuilder.Entity<Category>()
                .HasMany<Category>(c => c.SubCategories)
                .WithOptional(c => c.Parent)
                .Map(c =>
                {
                    c.MapKey("ParentId");
                    c.ToTable("Category");
                });

            modelBuilder.Entity<Category>()
                .HasOptional<Category>(c => c.Parent)
                .WithMany(c => c.SubCategories)
                .Map(c =>
                {
                    c.MapKey("ParentId");
                    c.ToTable("Category");
                });

            modelBuilder.Entity<PickerCategory>()
                .HasMany<PickerCategory>(c => c.SubCategories)
                .WithOptional(c => c.Parent)
                .Map(c =>
                {
                    c.MapKey("ParentId");
                    c.ToTable("PickerCategory");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
