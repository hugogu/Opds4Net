using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;
using Opds4Net.Model;
using Opds4Net.Properties;

namespace Opds4Net.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class OpdsHelper
    {
        private static Dictionary<string, string> extensionMimeMap = new Dictionary<string,string>();

        static OpdsHelper()
        {
            foreach (var extension in Settings.Default.MimeTypes)
            {
                var values = extension.Split(',');
                if (values.Length == 2)
                {
                    extensionMimeMap.Add(values[0].Trim(), values[1].Trim());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsReadingElementOf(this XmlReader reader, string ns, string name)
        {
            var @namespace = reader.LookupNamespace(reader.Prefix);

            return @namespace.Equals(ns, StringComparison.Ordinal) &&
                   reader.LocalName.Equals(name, StringComparison.Ordinal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public static string ToXml(this SyndicationFeed feed)
        {
            var writer = new StringWriter();
            feed.SaveAsAtom10(new XmlTextWriter(writer));

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ToXml(this SyndicationItem item)
        {
            var writer = new StringWriter();
            item.SaveAsAtom10(new XmlTextWriter(writer));

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Get the mime type from the extension name of a file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string DetectFileMimeType(string path)
        {
            string mimeType;
            if (extensionMimeMap.TryGetValue(Path.GetExtension(path).ToLowerInvariant(), out mimeType))
            {
                return mimeType;
            }
            else
                throw new NotSupportedException(String.Format("Cannot detect mime type from {0}", path));
        }

        /// <summary>
        /// Get the extension name from a mimeType.
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static string GetExtensionName(string mimeType)
        {
            mimeType = mimeType.ToLowerInvariant();

            foreach (var pair in extensionMimeMap)
            {
                if (pair.Value.Equals(mimeType, StringComparison.Ordinal))
                {
                    return pair.Key;
                }
            }

            throw new NotSupportedException(String.Format("I don't know the extension of mime type {0}", mimeType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileSupported(string path)
        {
            return extensionMimeMap.ContainsKey(Path.GetExtension(path).ToLowerInvariant());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="contentKind"></param>
        /// <returns></returns>
        public static TextSyndicationContent MakeSyndicationContent(this object value, TextSyndicationContentKind? contentKind = null)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                var content = Convert.ToString(value);
                if (contentKind == null)
                {
                    contentKind = TextSyndicationContentKind.Plaintext;
                    if (content.Contains('<') && content.Contains('>'))
                    {
                        contentKind = TextSyndicationContentKind.Html;
                    }
                }

                return new TextSyndicationContent(content, contentKind.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="facetGroup"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static OpdsLink MakeOpdsLinkWithFacet(this string url, string title, string facetGroup, bool isActive = false)
        {
            if (String.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");
            if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("title");
            if (String.IsNullOrWhiteSpace(facetGroup))
                throw new ArgumentNullException("facetGroup");

            return new OpdsLink()
            {
                ActiveFacet = isActive,
                FacetGroup = facetGroup,
                MediaType = OpdsMediaType.AcquisitionFeed,
                RelationshipType = OpdsRelations.Facet,
                Title = title,
                Uri = new Uri(url)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNullableString(this object value)
        {
            return value == null ? null : Convert.ToString(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="links"></param>
        /// <param name="relation"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static string GetLinkValue(this Collection<SyndicationLink> links, string relation, string mediaType = null)
        {
            var searchLink = links.SingleOrDefault(l =>
                relation.Equals(l.RelationshipType, StringComparison.OrdinalIgnoreCase) && 
                (mediaType == null || l.MediaType == null || l.MediaType.StartsWith(mediaType, StringComparison.OrdinalIgnoreCase)));
            if (searchLink == null || searchLink.Uri == null)
                return null;

            return searchLink.Uri.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="links"></param>
        /// <param name="relation"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="mediaType"></param>
        public static void SetLinkValue(this Collection<SyndicationLink> links, string relation, string url, string title, string mediaType)
        {
            var searchLink = links.SingleOrDefault(l =>
                relation.Equals(l.RelationshipType, StringComparison.OrdinalIgnoreCase) &&
                (mediaType == null || l.MediaType == null || l.MediaType.StartsWith(mediaType, StringComparison.OrdinalIgnoreCase)));
            if (searchLink != null)
                searchLink.Uri = new Uri(url, UriKind.RelativeOrAbsolute);
            else
                links.Add(new SyndicationLink(new Uri(url, UriKind.RelativeOrAbsolute), relation, title, mediaType, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="response"></param>
        /// <param name="mediaType"></param>
        public static void WriteTo(this SyndicationFeed feed, HttpResponse response, string mediaType = OpdsMediaType.AcquisitionFeed)
        {
            response.Clear();
            response.ContentType = mediaType;
            response.ContentEncoding = Encoding.UTF8;
            var xmlWriter = new XmlTextWriter(response.Output);
            xmlWriter.Indentation = 4;
            xmlWriter.Formatting = Formatting.Indented;

            feed.SaveAsAtom10(xmlWriter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="response"></param>
        public static void WriteTo(this OpdsItem item, HttpResponse response)
        {
            response.Clear();
            response.ContentType = OpdsMediaType.Entry;
            response.ContentEncoding = Encoding.UTF8;
            var xmlWriter = new XmlTextWriter(response.Output);
            xmlWriter.Indentation = 4;
            xmlWriter.Formatting = Formatting.Indented;

            item.SaveAsAtom10(xmlWriter);
        }
    }
}
