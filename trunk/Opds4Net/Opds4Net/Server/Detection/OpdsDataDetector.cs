using System;
using System.ComponentModel.Composition;
using Opds4Net.Model;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    [Export("OpdsData", typeof(IDataTypeDetector))]
    public class OpdsDataDetector : IDataTypeDetector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public OpdsDataType DetectType(object data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var opdsData = data as IOpdsData;
            if (opdsData == null)
                throw new NotSupportedException(String.Format("This detector work with object implements {0}", typeof(IOpdsData).Name));

            return opdsData.OpdsDataType;
        }
    }
}
