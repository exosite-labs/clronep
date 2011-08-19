/*=============================================================================
* DataportDescription.cs
* Description class used by client to create dataport.
*==============================================================================
*
* Tested with .NET Framework 3.5
*
* Copyright (c) 2011, Exosite LLC
* All rights reserved.
*/
namespace clronep
{
    public class DataportDescription : IDescription
    {
        public DataportDescription(string dataformat){
            format = dataformat;
            preprocess= new object[]{};
            retention = new Retention();           
        }
        public string format { set; get; }
        public string name { set; get; }
        public object[] preprocess{get;set;}
        public Retention retention { get;set;}
        public string subscribe {set;get;}
        public string visibility {set;get;}

    }
    public class Retention{
        public object count {set;get;}
        public object duration {set;get;}
    }    
}
