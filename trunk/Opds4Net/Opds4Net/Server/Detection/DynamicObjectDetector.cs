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
            var dynamic = data as dynamic;

            return (OpdsDataType)dynamic.OpdsDataType;
        }
    }
}
