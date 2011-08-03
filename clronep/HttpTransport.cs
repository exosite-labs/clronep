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
	internal class HttpTransport: ITransport{
		
		private string Url;		
		private int Timeout;
		
		internal HttpTransport(string url,int timeout){ Url=url;Timeout=timeout;}
		
	    public string send(string message){
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);		
			request.ContentType = "application/json; charset=utf-8";
			request.Method = "POST";
			request.Timeout = Timeout*1000;
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			request.ContentLength = bytes.Length;
			Stream stream = null;
			HttpWebResponse response = null;
			StreamReader reader = null;
			try{
				stream = request.GetRequestStream();
				stream.Write(bytes, 0, bytes.Length);
			}
			catch (System.Exception){
				throw new HttpRPCRequestException("Unable to make http request.");
			}
			finally{
				if (stream != null){
					stream.Close();
				}
			}			
			try{
				response = (HttpWebResponse)request.GetResponse();
				if (response == null) { return null; }
				reader = new StreamReader(response.GetResponseStream());			
				return reader.ReadToEnd();
			}catch (System.Exception){
				throw new HttpRPCResponseException("Unable to get http response.");
			}
			finally{
				if (response != null){					
					response.Close();
				}
				if (reader != null){
					reader.Close();
				}
			}
		}
	}
}