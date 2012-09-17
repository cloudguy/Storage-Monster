using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;

namespace StorageMonster.Web.Services.Upload
{
    public class MultipartFormHelper
    {
        public static byte[] GetMultipartBoundary(HttpWorkerRequest workerRequest)
        {
            if (workerRequest == null)
                throw new ArgumentNullException("workerRequest");

            string text = workerRequest.GetKnownRequestHeader(HttpWorkerRequest.HeaderContentType);                
            if (text == null)            
                return null;
            
            text = GetAttributeFromHeader(text, "boundary");
            if (text == null)
                return null;

            text = "--" + text;
            return Encoding.ASCII.GetBytes(text.ToCharArray());
        }

        public static Encoding GetEncodingFromHeaders(HttpWorkerRequest workerRequest)
        {
            if (workerRequest == null)
                throw new ArgumentNullException("workerRequest");

            string userAgent = workerRequest.GetKnownRequestHeader(HttpWorkerRequest.HeaderUserAgent);
            if (userAgent != null && CultureInfo.InvariantCulture.CompareInfo.IsPrefix(userAgent, "UP"))
            {
                string text = workerRequest.GetUnknownRequestHeader("x-up-devcap-post-charset");
                if (!string.IsNullOrEmpty(text))
                {
                    try
                    {
                        return Encoding.GetEncoding(text);
                    }
                    catch
                    {
                    }
                }
            }
            if (!workerRequest.HasEntityBody())
            {
                return null;
            }
            string contentType = workerRequest.GetKnownRequestHeader(HttpWorkerRequest.HeaderContentType);
            if (contentType == null)
            {
                return null;
            }
            string attributeFromHeader = GetAttributeFromHeader(contentType, "charset");
            if (attributeFromHeader == null)
            {
                return null;
            }
            Encoding result = null;
            try
            {
                result = Encoding.GetEncoding(attributeFromHeader);
            }
            catch
            {
            }
            return result;
        }

        public  static string GetAttributeFromHeader(string headerValue, string attrName)
        {
            if (headerValue == null)
            {
                return null;
            }
            int length = headerValue.Length;
            int length2 = attrName.Length;
            int i;
            for (i = 1; i < length; i += length2)
            {
                i = CultureInfo.InvariantCulture.CompareInfo.IndexOf(headerValue, attrName, i, CompareOptions.IgnoreCase);
                if (i < 0 || i + length2 >= length)
                {
                    break;
                }
                char c = headerValue[i - 1];
                char c2 = headerValue[i + length2];
                if ((c == ';' || c == ',' || char.IsWhiteSpace(c)) && (c2 == '=' || char.IsWhiteSpace(c2)))
                {
                    break;
                }
            }
            if (i < 0 || i >= length)
            {
                return null;
            }
            i += length2;
            while (i < length && char.IsWhiteSpace(headerValue[i]))
            {
                i++;
            }
            if (i >= length || headerValue[i] != '=')
            {
                return null;
            }
            i++;
            while (i < length && char.IsWhiteSpace(headerValue[i]))
            {
                i++;
            }
            if (i >= length)
            {
                return null;
            }
            string result = null;
            if (i < length && headerValue[i] == '"')
            {
                if (i == length - 1)
                {
                    return null;
                }
                int num = headerValue.IndexOf('"', i + 1);
                if (num < 0 || num == i + 1)
                {
                    return null;
                }
                result = headerValue.Substring(i + 1, num - i - 1).Trim();
            }
            else
            {
                int num = i;
                while (num < length && headerValue[num] != ' ' && headerValue[num] != ',')
                {
                    num++;
                }
                if (num == i)
                {
                    return null;
                }
                result = headerValue.Substring(i, num - i).Trim();
            }
            return result;
        }

    }
}