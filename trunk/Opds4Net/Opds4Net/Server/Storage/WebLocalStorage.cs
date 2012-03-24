using System;
using System.ComponentModel.Composition;
using System.IO;

namespace Opds4Net.Server.Storage
{
    /// <summary>
    /// 
    /// </summary>
    [Export("Local", typeof(IContentStorage))]
    public class WebLocalStorage : IContentStorage
    {
        private string localFolder;
        private string downloadPattern;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localFolder"></param>
        /// <param name="downloadPattern"></param>
        [ImportingConstructor]
        public WebLocalStorage(
            [Import("LocalStorageFolder")]string localFolder,
            [Import("DownloadLinkPattern")]string downloadPattern)
        {
            this.localFolder = localFolder;
            this.downloadPattern = downloadPattern;
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

            return String.Format(downloadPattern, Path.GetFileNameWithoutExtension(contentName));
        }

        #endregion
    }
}