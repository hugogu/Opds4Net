using System;
using Opds4Net.Model;

namespace Opds4Net.Test.Model
{
    [OpdsData(OpdsDataType.Category)]
    internal class DataModel : IOpdsData
    {
        [OpdsName("Title")]
        public string Name { get; set; }

        public string Id { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Summary { get; set; }

        public virtual OpdsDataType DataType
        {
            get { return OpdsDataType.Category; }
        }
    }

    [OpdsData(OpdsDataType.Entity)]
    internal class DataEntry : DataModel
    {
        public Decimal Price { get; set; }

        public string MimeType { get; set; }

        public override OpdsDataType DataType
        {
            get { return OpdsDataType.Entity; }
        }
    }
}
