using System.ComponentModel.Composition;
using Opds4Net.Model;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    [Export("Dynamic", typeof(IDataTypeDetector))]
    public class DynamicObjectDetector : IDataTypeDetector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public OpdsDataType DetectType(object data)
        {
            return (OpdsDataType)(data as dynamic).OpdsDataType;
        }
    }
}
