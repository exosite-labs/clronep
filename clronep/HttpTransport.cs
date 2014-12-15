/*=============================================================================
* HttpTransport.cs
* HTTP-based JSON-RPC request call.
*==============================================================================
*
* Tested with .NET Framework 3.5
*
* Copyright (c) 2011, Exosite LLC
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
    }
}
