/*=============================================================================
* EmptyOption.cs
* For empty option in method call.
*==============================================================================
*
* Tested with .NET Framework 4.6
*
* Copyright (c) 2015, Exosite LLC
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