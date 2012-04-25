using Opds4Net.Model;

namespace Opds4Net.Server
{
    /// <summary>
    /// Provides the capability to detect the type of the given data. 
    /// </summary>
    public interface IDataTypeDetector
    {
        /// <summary>
        /// Detects the type of the given data. 
        /// The data could be an Plain single instance or an IEnumerable instance that contains many sub items.        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        OpdsDataType DetectType(object data);
    }
}
