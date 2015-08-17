/*=============================================================================
* HttpTransport.cs
* HTTP-based JSON-RPC request call.
*==============================================================================
*
* Tested with .NET Framework 4.6
*
* Copyright (c) 2015, Exosite LLC
* All rights reserved.
*/

using System;
using System.IO;
using System.Text;
using System.Net;

namespace clronep
{
    internal class HttpTransport : ITransport
    {

        private string Url;
        private int Timeout;
        private WebProxy ProxyServer = null;

        internal HttpTransport(string url, int timeout) { Url = url; Timeout = timeout; }

        public void set_proxy(string ip, int port, string user, string password)
        {
            if (null != ip && port > 0)
            {
                if (0 == ip.Length)
                    return;
                this.ProxyServer = new WebProxy(ip, port);
                if (null != user && null != password)
                {
                    this.ProxyServer.Credentials = new NetworkCredential(user, password);
                }
                System.Net.ServicePointManager.Expect100Continue = false;
            }
        }

        public string send(string message)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            if (null != this.ProxyServer)
                request.Proxy = this.ProxyServer;
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";
            request.Timeout = Timeout * 1000;
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            request.ContentLength = bytes.Length;
            Stream stream = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                throw new HttpRPCRequestException("Unable to make http request.");
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response == null) { return null; }
                reader = new StreamReader(response.GetResponseStream());
                string recv = reader.ReadToEnd();
                return recv;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                throw new HttpRPCResponseException("Unable to get http response.");
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        public string[] provisionSend(string message, string method, string url, WebHeaderCollection headers)
        {
            url = Url + url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (null != this.ProxyServer)
                request.Proxy = this.ProxyServer;
            request.Method = method;
            request.Timeout = Timeout * 1000;
            if (headers["Accept"] != null)
            {
                request.Accept = headers["Accept"];
                headers.Remove("Accept");
            }
            if (headers["Connection"] != null)
            {
                request.Connection = headers["Connection"];
                headers.Remove("Connection");
            }
            if (headers["Content-Length"] != null)
            {
                request.ContentLength = Convert.ToInt64(headers["Content-Length"]);
                headers.Remove("Content-Length");
            }
            if (headers["Content-Type"] != null)
            {
                request.ContentType = headers["Content-Type"];
                headers.Remove("Content-Type");
            }
            if (headers["Expect"] != null)
            {
                request.Expect = headers["Expect"];
                headers.Remove("Expect");
            }
            if (headers["If-Modified-Since"] != null)
            {
                request.IfModifiedSince = Convert.ToDateTime(headers["If-Modified-Since"]);
                headers.Remove("If-Modified-Since");
            }
            if (headers["Range"] != null)
            {
                request.AddRange(Convert.ToInt32(headers["Range"]));
                headers.Remove("Range");
            }
            if (headers["Referer"] != null)
            {
                request.Referer = headers["Referer"];
                headers.Remove("Referer");
            }
            if (headers["Transfer-Encoding"] != null)
            {
                request.TransferEncoding = headers["Transfer-Encoding"];
                headers.Remove("Transfer-Encoding");
            }
            if (headers["User-Agent"] != null)
            {
                request.UserAgent = headers["User-Agent"];
                headers.Remove("User-Agent");
            }
            foreach (string key in headers.Keys)
            {
                if (request.Headers[key] != null) 
                {
                    request.Headers.Set(key, headers[key]);
                }
                else request.Headers.Add(key, headers[key]);
            }
            HttpWebResponse response = null;
            StreamReader reader = null;
            Stream stream = null;
            byte[] bytes = null;
            if (message != null)
            {
                bytes = Encoding.UTF8.GetBytes(message);
                request.ContentLength = bytes.Length;
                try
                {
                    stream = request.GetRequestStream();
                    stream.Write(bytes, 0, bytes.Length);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw new HttpRPCRequestException("Unable to make http request.");
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response == null) { return null; }
                reader = new StreamReader(response.GetResponseStream());
                string recv = reader.ReadToEnd();
                string statuscode;
                if (HttpStatusCode.OK == response.StatusCode || HttpStatusCode.ResetContent == response.StatusCode || HttpStatusCode.NoContent == response.StatusCode)
                {
                    statuscode = response.StatusCode.ToString();
                    recv = statuscode + "\r\n" + recv;
                }
                else
                {
                    statuscode = "FAIL";
                    recv = statuscode + "\r\n" + recv;
                }
                return new string[2] {statuscode, recv};
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                throw new HttpRPCResponseException("Unable to get http response.");
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
