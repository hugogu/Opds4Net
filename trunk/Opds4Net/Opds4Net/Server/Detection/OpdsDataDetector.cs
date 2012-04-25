using System;
using System.Collections;
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
                if (data is IEnumerable)
                {
                    // As long as there is an Detail item, the result will be Detail.
                    // As long as there is an Entity item, the result will be Entity,
                    // because the category info could be the category of the entity.
                    var finalType = OpdsDataType.Category;
                    foreach (var dataItem in data as IEnumerable)
                    {
                        try
                        {
                            var type = DetectType(dataItem);
                            if (type == OpdsDataType.Detial)
                                return type;
                            else if (type == OpdsDataType.Entity)
                                finalType = OpdsDataType.Entity;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                    return finalType;
                }

                return (OpdsDataType)(data as dynamic).OpdsDataType;
            }

            return opdsData.OpdsDataType;
        }
    }
}
