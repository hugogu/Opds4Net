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
    public class DataEntry : DataModel
    {
        public Decimal Price { get; set; }

        public string MimeType { get; set; }

        public override OpdsDataType OpdsDataType
        {
            get { return OpdsDataType.Entity; }
        }
    }
}
