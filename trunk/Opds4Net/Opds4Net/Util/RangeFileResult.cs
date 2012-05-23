using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Opds4Net.Util
{
    /// <summary>
    /// A file download result enable breakpoint resume transmission.
    /// </summary>
    public class RangeFileResult : FileResult
    {
        private string filePath;
        private HttpRequestBase request;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        public RangeFileResult(HttpRequestBase request, string filePath, string fileName, string contentType)
            : base(contentType)
        {
            this.request = request;
            this.filePath = filePath;
            base.FileDownloadName = fileName;
        }

        /// <summary>
        /// Refer to http://www.cnblogs.com/gjahead/archive/2007/06/18/787654.html
        /// </summary>
        /// <param name="response"></param>
        protected override void WriteFile(HttpResponseBase response)
        {
            #region--验证：HttpMethod，请求的文件是否存在
            switch (request.HttpMethod.ToUpper())
            { 
                case "GET":
                case "HEAD":
                    break;
                default:
                    response.StatusCode = 501;
                    return;
            }
            if (!File.Exists(filePath))
            {
                response.StatusCode = 404;
                response.End();
                return;
            }
            #endregion

            #region 定义局部变量
            var startBytes = 0L;
            //分块读取，每块10K bytes
            var packSize = 1024 * 10;
            var fileName = Path.GetFileName(filePath);
            var file = MemoryMappedFile.CreateFromFile(filePath).CreateViewStream();
            var fileLength = file.Length;
            var sleep = 0;
            var lastUpdateTiemStr = File.GetLastWriteTimeUtc(filePath).ToString("r");
            //便于恢复下载时提取请求头;
            var eTag = HttpUtility.UrlEncode(fileName, Encoding.UTF8) + lastUpdateTiemStr;
            #endregion

            #region--验证：文件是否太大，是否是续传，且在上次被请求的日期之后是否被修
            //文件太大
            if (file.Length > Int32.MaxValue)
            {
                response.StatusCode = 413;//请求实体太大
            }

            //对应响应头ETag：文件名+文件最后修改时间  
            if (request.Headers["If-Range"] != null)
            {
                //上次被请求的日期之后被修改过
                if (request.Headers["If-Range"].Replace("\"", "") != eTag)
                {
                    response.StatusCode = 412;//预处理失败
                }
            }
            #endregion

            using (var br = new BinaryReader(file))
            {
                #region -------添加重要响应头、解析请求头、相关验证-------------------
                response.Clear();
                response.Buffer = false;
                //用于验证文件 
                // TODO: 验证MD5生成方式是否正确，这里对整个文件Hash，似乎有问题。
                response.AddHeader("Content-MD5", GetMd5Hash(file));
                //重要：续传必须
                response.AddHeader("Accept-Ranges", "bytes");
                //重要：续传必须
                response.AppendHeader("ETag", "\"" + eTag + "\"");
                response.AppendHeader("Last-Modified", lastUpdateTiemStr);//把最后修改日期写入响应
                response.ContentType = "application/octet-stream";//MIME类型：匹配任意文件类型
                response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                response.AddHeader("Connection", "Keep-Alive");
                // 不要设置Content-Distribution，FileResult会自动设置这个属性。
                response.ContentEncoding = Encoding.UTF8;
                //如果是续传请求，则获取续传的起始位置，即已经下载到客户端的字节数
                if (request.Headers["Range"] != null)
                {
                    //重要：续传必须，表示局部范围响应。初始下载时默认为200
                    response.StatusCode = 206;
                    //"bytes=1474560-"
                    string[] range = request.Headers["Range"].Split(new char[] { '=', '-' });
                    //已经下载的字节数，即本次下载的开始位置
                    startBytes = Convert.ToInt64(range[1]);
                    //无效的起始位置  
                    if (startBytes < 0 || startBytes >= fileLength)
                    {
                        return;
                    }
                }
                //如果是续传请求，告诉客户端本次的开始字节数，总长度，以便客户端将续传数据追加到startBytes位置后
                if (startBytes > 0)
                {
                    response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                }
                #endregion

                #region -------向客户端发送数据块-------------------
                br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                //分块下载，剩余部分可分成的块数
                var maxCount = (int)Math.Ceiling((fileLength - startBytes + 0.0) / packSize);
                for (var i = 0; i < maxCount && response.IsClientConnected; i++)
                {
                    //客户端中断连接，则暂停
                    response.BinaryWrite(br.ReadBytes(packSize));
                    response.Flush();
                    if (sleep > 1)
                        Thread.Sleep(sleep);
                }
                #endregion
            }
        }

        /// <summary>
        /// Simplified form http://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(Stream input)
        {
            return String.Concat(MD5.Create().ComputeHash(input).Select(b => b.ToString("x2")));
        }
    }
}
