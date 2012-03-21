using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opds4Net.Model;

namespace Opds4Net.Test.Model
{    
    internal class DataModel : IOpdsData
    {
        [OpdsName("CategoryName")]
        public string Name { get; set; }

        public OpdsDataType DataType
        {
            get { return OpdsDataType.Category; }
        }
    }
}
