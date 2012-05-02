using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
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

            return @namespace.Equals(ns) && reader.LocalName.Equals(name);
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
        /// <returns></returns>
        public static TextSyndicationContent MakeSyndicationContent(this object value)
        {
            if (value == null)
                return null;
            else
            {
                var content = Convert.ToString(value);
                var kind = TextSyndicationContentKind.Plaintext;
                if (content.Contains('<') && content.Contains('>'))
                {
                    kind = TextSyndicationContentKind.Html;
                }

                return new TextSyndicationContent(content, kind);
            }
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
    }
}
