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

        [DataType(DataType.DateTime)]
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
    }
}