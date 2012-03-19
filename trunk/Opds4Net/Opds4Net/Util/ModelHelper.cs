using System;

namespace Opds4Net.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelHelper<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="KeyType"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Func<T, KeyType> FindKeySelector<KeyType>(string propertyName)
        {
            throw new NotImplementedException();

            // TODO: Emit Code to provide the keySelector.
        }
    }
}
