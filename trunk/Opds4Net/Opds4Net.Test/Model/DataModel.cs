using System;
using Opds4Net.Server;

namespace Opds4Net.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class DataModel : IOpdsData
    {
        [OpdsName("Title")]
        public string Name { get; set; }

        public string Id { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Summary { get; set; }

        public virtual OpdsDataType OpdsDataType
        {
            get { return OpdsDataType.Category; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public AddressInfo()
        {
            Country = "China";
            City = "Shanghai";
        }

        public string Country { get; set; }

        public string City { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthorInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthorInfo()
        {
            Name = "fadefy";
            Email = "fadefy@mail.com";
            Address = new AddressInfo();
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public AddressInfo Address { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataEntry : DataModel
    {
        public DataEntry()
        {
            AuthorInfo = new AuthorInfo();
        }

        public Decimal Price { get; set; }

        public string MimeType { get; set; }

        [OpdsName("AuthorAddress", PropertyPath = "Address.Country")]
        [OpdsName("AuthorEmail", PropertyPath = "Email")]
        [OpdsName("AuthorName", PropertyPath = "Name")]
        public AuthorInfo AuthorInfo { get; set; }

        public override OpdsDataType OpdsDataType
        {
            get { return OpdsDataType.Entity; }
        }
    }
}
