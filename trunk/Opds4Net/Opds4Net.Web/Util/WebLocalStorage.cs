using System.ComponentModel.Composition;
using System.IO;
using Opds4Net.Server;

namespace Opds4Net.Web.Util
{
    /// <summary>
    /// 
    /// </summary>
    [Export("Local", typeof(IContentStorage))]
    public class WebLocalStorage : IContentStorage
    {
        private string localFolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localFolder"></param>
        [ImportingConstructor]
        public WebLocalStorage([Import("LocalStorageFolder")]string localFolder)
        {
            this.localFolder = localFolder;
        }

        #region IContentStorage Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentName"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public string Store(string contentName, Stream inputStream)
        {
            using (var file = File.OpenWrite(Path.Combine(localFolder, contentName)))
            {
                inputStream.CopyTo(file);
            }

            return "/Download/" + Path.GetFileNameWithoutExtension(contentName);
        }

        #endregion
    }
}