using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Opds4Net.Server;
using Reflection4Net;

namespace Opds4Net.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Book : IOpdsDataTypeHost
    {
        public Book()
        {
            OpdsDataType = OpdsDataType.Entity;
            Publisher = "Poleaf Studio";
        }

        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        [DefaultKeySelector]
        [AdaptedName("Title")]
        public string Name { get; set; }

        [MaxLength(200)]
        [AdaptedName("AuthorName")]
        public string Author { get; set; }

        public string Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateTime IssueTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string MimeType { get; set; }

        [AdaptedName("Length")]
        public long FileSize { get; set; }

        [DataType(DataType.Currency)]
        public Decimal OriginalPrice { get; set; }

        [Required]
        [AdaptedName("Price")]
        [DataType(DataType.Currency)]
        public Decimal SalePrice { get; set; }

        public string DownloadAddress { get; set; }

        public DateTime UpdateTime { get; set; }

        [Display(Name = "Category")]
        public ICollection<Category> Categories { get; set; }

        [NotMapped]
        public OpdsDataType OpdsDataType { get; set; }
    }
}