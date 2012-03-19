using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Opds4Net.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Author { get; set; }

        public string Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateTime IssueTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string MimeType { get; set; }

        public long FileSize { get; set; }

        [DataType(DataType.Currency)]
        public Decimal OriginalPrice { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public Decimal SalePrice { get; set; }

        public string DownloadAddress { get; set; }

        public DateTime UpdateTime { get; set; }

        [Display(Name = "Category")]
        public ICollection<Category> Categories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Func<Book, object> FindKeySelector(string propertyName)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                return b => b.UpdateTime;

            switch (propertyName.ToLowerInvariant())
            {
                case "updatetime":
                    return b => b.UpdateTime;
                case "name":
                    return b => b.Name;
                case "saleprice":
                    return b => b.SalePrice;
                case "originalprice":
                    return b => b.OriginalPrice;
                case "filesize":
                    return b => b.FileSize;
                case "publisher":
                    return b => b.Publisher;
                case "issuetime":
                    return b => b.IssueTime;
                case "mimetype":
                    return b => b.MimeType;
                case "author":
                    return b => b.Author;
                default:
                    return b => b.Name;
            }
        }
    }
}