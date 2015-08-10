/*=============================================================================
* Result.cs
* Represents the result of JSON-RPC.
*==============================================================================
*
* Tested with .NET Framework 4.6
*
* Copyright (c) 2015, Exosite LLC
* All rights reserved.
*/

namespace clronep{	
    public class Result{
		public static readonly string OK = "ok";
		public static readonly string FAIL = "fail";
		public string status{get;set;}
		public string message{get;set;}
	}	
}
