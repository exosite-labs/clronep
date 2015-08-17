/*=============================================================================
* ITransport.cs
* Transport tool interface - e.g. HTTP, XMPP.
*==============================================================================
*
* Tested with .NET Framework 4.6
*
* Copyright (c) 2015, Exosite LLC
* All rights reserved.
*/

namespace clronep{
	interface ITransport{
		string send(string request);
	}
}