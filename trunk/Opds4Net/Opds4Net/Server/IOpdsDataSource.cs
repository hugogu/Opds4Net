using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpdsDataSource
    {
        IEnumerable<SyndicationItem> GetItems(string id);

        SyndicationItem GetDetail(string id);
    }
}
