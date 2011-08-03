/*=============================================================================
* EmptyOption.cs
* For empty option in method call.
*==============================================================================
*
* Tested with .NET Framework 3.5
*
* Copyright (c) 2011, Exosite LLC
* All rights reserved.
*/

namespace clronep{
	public class EmptyOption{
		private static EmptyOption instance;
		private EmptyOption() {}
		public static EmptyOption Instance{
			get{
				if (instance == null){
					instance = new EmptyOption();
				}
				return instance;
			}
		}
	}	
}