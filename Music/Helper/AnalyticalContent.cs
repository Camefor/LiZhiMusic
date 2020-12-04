using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Music.Helper {
    public class AnalyticalContent {

        static public string GetHtml(string url) {
            HttpWebRequest myReq =
            (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)myReq.GetResponse()) {
                // Get the stream associated with the response.
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        return readStream.ReadToEnd();
                    }
                }
            }
        }


        public static string HtmlToPlainText(string html) {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);
            return text;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="imgUrl">图片url</param>
        /// <param name="fullPath">要保存的路径</param>
        public static void GetImgRes(string imgUrl, string fullPath) {
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage response = client.GetAsync(imgUrl).Result) {
                    response.EnsureSuccessStatusCode();
                    var respnseBody = response.Content.ReadAsByteArrayAsync().Result;
                    using (Stream resStream = (response.Content.ReadAsStreamAsync().Result)) {
                        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                        //https://blog.lindexi.com/post/C-dotnet-%E5%B0%86-Stream-%E4%BF%9D%E5%AD%98%E5%88%B0%E6%96%87%E4%BB%B6%E7%9A%84%E6%96%B9%E6%B3%95.html
                        using (var fileStream = File.Create(fullPath)) {
                            resStream.Seek(0, SeekOrigin.Begin);
                            resStream.CopyTo(fileStream);
                        }
                    }
                }
            }
        }
    }
}
