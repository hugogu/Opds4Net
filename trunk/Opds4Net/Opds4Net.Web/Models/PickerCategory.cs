using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Opds4Net.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PickerCategory
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PickerCategory Parent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<PickerCategory> SubCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullName;
        }
    }
}