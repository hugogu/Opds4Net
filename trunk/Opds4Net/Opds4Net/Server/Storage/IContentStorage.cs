using System.IO;

namespace Opds4Net.Server
{
    /// <summary>
    /// We probably change the way of persistance of the book.
    /// When there are only a few book files, the local file system of the Web Server would be enough.
    /// When we have a lot of file to store and contribute. A cloud stroage engine will be involved.
    /// </summary>
    public interface IContentStorage
    {
        /// <summary>
        /// Store a content somewhere
        /// </summary>
        /// <param name="contentName">the name of the content</param>
        /// <param name="inputStream">the content stream</param>
        /// <returns>The access uri of the stored content</returns>
        string Store(string contentName, Stream inputStream);
    }
}
