using System;
using Opds4Net.Model;

namespace Opds4Net.Test.Model
{
    internal class DataModel : IOpdsData
    {
        [OpdsName("Title")]
        public string Name { get; set; }

        public string Id { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Summary { get; set; }

        public Decimal Price { get; set; }

        public string MimeType { get; set; }

        public OpdsDataType DataType { get; set; }
    }
}
