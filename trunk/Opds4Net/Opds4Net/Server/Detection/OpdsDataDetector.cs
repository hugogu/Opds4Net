using System;
using System.ComponentModel.Composition;

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
            {
                return (OpdsDataType)(data as dynamic).OpdsDataType;
            }

            return opdsData.OpdsDataType;
        }
    }
}
