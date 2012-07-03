using Opds4Net.Model;
using Opds4Net.Reflection;

namespace Opds4Net.Server
{
    /// <summary>
    /// Generate Opds Link for given data object and the information of the opds link.
    /// </summary>
    public interface IOpdsLinkGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="opdsLinkRelation"></param>
        /// <param name="opdsLinkMediaType"></param>
        /// <param name="propertyAccessor"></param>
        /// <param name="nameMapping"></param>
        /// <returns>If the data is not valid or not enough. Return null. </returns>
        OpdsLink Generate(object data, string opdsLinkRelation, string opdsLinkMediaType, IPropertyAccessor propertyAccessor, OpdsNames nameMapping);
    }
}
