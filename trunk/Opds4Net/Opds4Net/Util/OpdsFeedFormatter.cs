using System.ServiceModel.Syndication;
using Opds4Net.Model;

namespace Opds4Net.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsFeedFormatter : Atom10FeedFormatter<OpdsFeed>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void ReadFrom(System.Xml.XmlReader reader)
        {
            base.ReadFrom(reader);

            // Because a bug of .NET framework that
            // the TryParseAttribute method of SyndicationLink is nerver called when read xml to Synciation Link.
            // So I cannot support any extention attribute by overriding taht method.
            // Here is the workaround to do that.
            foreach (OpdsLink link in Feed.Links)
            {
                link.LinkDataReaded();
            }
        }

        /// <summary>
        /// The base class create instance by reflection.
        /// Override to speed up.
        /// </summary>
        /// <returns></returns>
        protected override SyndicationFeed CreateFeedInstance()
        {
            return new OpdsFeed();
        }
    }
}
