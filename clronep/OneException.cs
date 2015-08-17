/*=============================================================================
* OneException.cs
* Exception classes for JSON-RPC. 
*==============================================================================
*
* Tested with .NET Framework 4.6
*
* Copyright (c) 2015, Exosite LLC
* All rights reserved.
*/

using System;
namespace clronep{
	public class OneException : Exception{
		public OneException(string message):base(message){}
	}
	public class OnePlatformException : OneException
	{
		public OnePlatformException(string message):base(message){}		
	}
	public class HttpRPCRequestException : OneException
	{
		public HttpRPCRequestException(string message):base(message){} 
	}
	public class HttpRPCResponseException : OneException
	{
		public HttpRPCResponseException(string message):base(message){}
	}
    public class ProvisionException : OneException
    {
        public ProvisionException(string message):base(message){}
    }
}