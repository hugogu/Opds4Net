﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Syndication;
using Opds4Net.Util;

namespace Opds4Net.Server.FileSystem
{
    /// <summary>
    /// 
    /// </summary>
    [Export("FileSystem", typeof(IOpdsDataSource))]
    public class FSOpdsDataSource : IOpdsDataSource
    {
        private string bookFolder;
        private IOpdsLinkGenerator linkGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        /// <param name="bookFolder"></param>
        [ImportingConstructor]
        public FSOpdsDataSource(
            [Import]IOpdsLinkGenerator linkGenerator,
            [Import("BookFolder")] string bookFolder)
        {
            this.bookFolder = bookFolder;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OpdsItemsResult GetItems(IDataRequest request)
        {
            request.Id = request.Id ?? String.Empty;

            var items = new List<SyndicationItem>();
            var result = new OpdsItemsResult() { Items = items };

            var root = Path.Combine(bookFolder, request.Id);

            if (!Directory.Exists(root))
                throw new ArgumentException("id");

            foreach (var path in Directory.GetFiles(root))
            {
                if (OpdsHelper.IsFileSupported(path))
                {
                    var item = GetDetail(path);
                    item.Links.Clear();
                    item.Links.Add(linkGenerator.GetDetailLink(Path.Combine(request.Id, Path.GetFileName(path)), "详细信息"));

                    items.Add(item);
                }
            }

            foreach (var path in Directory.GetDirectories(root))
            {
                var directoryInfo = new DirectoryInfo(path);
                var item = new SyndicationItem()
                {
                    Title = new TextSyndicationContent(directoryInfo.Name, TextSyndicationContentKind.Plaintext),
                    LastUpdatedTime = DateTimeOffset.Parse(directoryInfo.LastWriteTime.ToString("o")),
                };
                item.Links.Add(linkGenerator.GetNavigationLink(Path.Combine(request.Id, Path.GetFileName(path)), directoryInfo.Name));

                items.Add(item);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SyndicationItem GetDetail(string id)
        {
            id = id ?? String.Empty;
            var path = Path.Combine(bookFolder, id);

            if (!File.Exists(path))
                throw new ArgumentException("id");

            var fileInfo = new FileInfo(path);
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(path);

            var item = new SyndicationItem(fileInfo.Name, fileVersionInfo.FileDescription, null, id, DateTimeOffset.Parse(fileInfo.LastWriteTimeUtc.ToString("o")));
            item.PublishDate = DateTimeOffset.Parse(fileInfo.CreationTimeUtc.ToString("o"));
            var downloadLink = linkGenerator.GetDownloadLink(id, "下载");
            downloadLink.MediaType = OpdsHelper.DetectFileMimeType(path);
            downloadLink.Length = fileInfo.Length;
            item.Links.Add(downloadLink);

            return item;
        }
    }
}
