using Opds4Net.Model;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataTypeDetector
    {
        OpdsDataType DetectType(object data);
    }
}
