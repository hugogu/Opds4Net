using System.ComponentModel.Composition;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    [Export("Naming", typeof(IOpdsDataSource))]
    public class NamingOpdsDataSource : IOpdsDataSource
    {
        private IOpdsItemConverter itemConverter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemConverter"></param>
        [ImportingConstructor]
        public NamingOpdsDataSource([Import("DataModel")]IOpdsItemConverter itemConverter)
        {
            this.itemConverter = itemConverter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual OpdsItemsResult GetItems(IDataRequest request)
        {
            var requestData = request.Process();
            if (requestData != null)
            {
                var result = itemConverter.GetItems(new OpdsDataSource(requestData.Data));

                return new OpdsItemsResult(result.Items);
            }

            return new OpdsItemsResult();
        }
    }
}
