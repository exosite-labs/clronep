========================================
About clronep
========================================
This project is a C# binding to the to the Exosite One Platform API. The API is
exposed over HTTP in a JSON RPC style interface.  The solution creates a Common
Language Runtime dll for use by .NET applications that wish to interact with 
Exosite's One Platform.

Recommend using with Microsoft .NET Framework 3.5.

License is BSD, Copyright 2011, Exosite LLC (see LICENSE file)

========================================
Quick Start
========================================
Tested with Visual Studio 2010 (Windows) and MonoDevelop 2.4/Mono 2.6.7 
(Windows/Linux).

--) Example test project in the ./clronep/clronep.examples/ folder<br>
--) API documentation in the ./clronep/docs/ folder<br>

For more information on the API and examples, reference Exosite online 
documentation at http://exosite.com/developers/documentation.

========================================
Release Info
========================================

----------------------------------------
Release 0.7
----------------------------------------
--) Add provisioning support and example<br>
--) Add UI for examples

----------------------------------------
Release 0.6
----------------------------------------
--) Add writegroup RPC method, update tests accordingly<br>
--) Add recordbatch<br>
--) Add usage<br>
--) Add wait and example test<br>

----------------------------------------
Release 0.5
----------------------------------------
--) Add proxy and connect_as support in in OnepV1<br>
--) Modify OnepV1 listing call to take empty options<br>
--) Update all OnepV1 calls that take an RID to take RID as object or a string<br>
--) Remove support for the comment RPC which was deprecated in One Platform<br>

----------------------------------------
Release 0.4
----------------------------------------
--) Support .NET compact framework 3.5<br>

----------------------------------------
Release 0.3
----------------------------------------
--) Updated JSON.Net dll to use .NET 3.5 version<br>
--) Added "layer 2" methods to batch together commonly used functionality<br>
--) Support for API change in "aliases" return format<br>
--) Updated example application for API and layer-2 methods<br>

----------------------------------------
Release 0.2
----------------------------------------
--) added example application<br>
--) NOTE: Due to using JSON.Net dll for .NET 2.0 (for best compatibility), the
    projects generate some related warnings when built against .NET 3.5.<br>

----------------------------------------
Release 0.1
----------------------------------------
--) initial version<br>
