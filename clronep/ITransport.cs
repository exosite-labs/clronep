/*=============================================================================
* ITransport.cs
* Transport tool interface - e.g. HTTP, XMPP.
*==============================================================================
*
* Tested with .NET Framework 3.5
*
* Copyright (c) 2011, Exosite LLC
* All rights reserved.
*/

namespace clronep{
	interface ITransport{
		string send(string request);
	}
}