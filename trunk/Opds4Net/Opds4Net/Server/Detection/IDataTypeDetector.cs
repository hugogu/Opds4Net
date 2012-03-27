using Opds4Net.Model;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataTypeDetector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        OpdsDataType DetectType(object data);
    }
}
