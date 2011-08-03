/*=============================================================================
* Examples.cs
* Use-case examples.
* Note that CIK strings ("PUTA40CHARACTER...") need to be replaced with a valid
* value.
*==============================================================================
*
* Tested with .NET Framework 3.5
*
* Copyright (c) 2011, Exosite LLC
* All rights reserved.
*/

using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace clronep.examples
{
    /// <summary>
    /// Contains methods that are used to demonstrate use-case examples for 
    /// the clronep library
    /// </summary>
    class Example
    {
        /// <summary>
        /// Main method - runs through a sequence of calls that are typical for a given application
        /// </summary>
        public static void Main (string[] args)
        {
            //NOTE: The easiest way to get a Client Interface Key (CIK - "deviceKey") the first time is probably from Exosite Portals https://portals.exosite.com 
            string deviceKey = "PUTA40CHARACTERCIKHERE";   
            OnepV1 oneConn = new OnepV1("http://m2.exosite.com/api:v1/rpc/process",3);
            Result result;
            try{
                // Get resource id (RID) of dataport given its alias name 'X1'
                result = oneConn.lookup(deviceKey,"alias","X1");
                string rid=null;
                if (result.status == Result.OK)
                {
                    rid = result.message;
                } else {
                    // If dataport with alias 'X1' didn't exist, we will create it
                    Result obj_newdp = oneConn.create(deviceKey, "dataport", getDescObject());
                    if (obj_newdp.status == Result.OK)
                    {
                        rid = obj_newdp.message;
                        Console.WriteLine("Dataport: " + rid + " is created.");
                    }
                }

                // Write data to dataport
                int val = new Random().Next(1,100);
                result = oneConn.write(deviceKey,rid,val);
                if (result.status == Result.OK){
                    Console.WriteLine("Data: " + val + " is written.");
                }
                
                // Read data from dataport
                result = oneConn.read(deviceKey,rid,EmptyOption.Instance);
                if (result.status == Result.OK){
                    object[][] read = JsonConvert.DeserializeObject<object[][]> (result.message);
                    int data = Int32.Parse( read[0][1].ToString() );
                    Console.WriteLine("Data: " + data + " is read.");
                }
                
                // Create and then drop a dataport (note - a client can have many dataports w/ same name, but alias & RID must be unique)
                object desc = getDescObject();
                Result obj1 = oneConn.create(deviceKey,"dataport",desc);
                if (obj1.status == Result.OK){					
                    string dprid = obj1.message;
                    Console.WriteLine("Dataport: " + dprid + " is created.");
                    string alias = "test_alias";
                    // Map an alias to the dataport
                    Result obj2 = oneConn.map(deviceKey,dprid,alias);
                    if (obj2.status == Result.OK){
                        Console.WriteLine("Dataport: " + dprid + " is mapped to alias '"+ alias+"'");
                        // Un-map the alias from the dataport
                        Result obj3 = oneConn.unmap(deviceKey,alias);
                        if (obj3.status == Result.OK){
                            Console.WriteLine("Dataport: " + dprid + " is unmapped from alias '"+ alias+"'");
                        }
                    }
                    Result obj4 = oneConn.drop(deviceKey,dprid);
                    if (obj4.status == Result.OK){
                        Console.WriteLine("Dataport: " + dprid + " is dropped.");
                    }
                }

                // List a client's dataports
                string[] options = new string[]{"dataport"};
                result = oneConn.listing(deviceKey,options);
                if (result.status == Result.OK){
                    Console.WriteLine("Dataport RIDs for client CIK " + deviceKey + ":");
                    Console.WriteLine(result.message);
                }
                   
                /* Get all mapping aliases information of dataports */
                // Get resource id of device given device key
                result = oneConn.lookup(deviceKey,"alias","");
                string deviceRID = result.message;
                // Get the alias information of given device
                Dictionary<string,object> option =new Dictionary<string, object>();
                option.Add("aliases",true);
                result = oneConn.info(deviceKey,deviceRID,option);
                if (result.status == Result.OK){
                    Console.WriteLine("Dataport Alias' for client CIK " + deviceKey + ":");
                    Console.WriteLine(result.message);
                }
            } catch(OneException){
                //do something for exception.
            }
        }

        /// <summary>
        /// Describes the setup of a dataport.  Used when calling "create"
        /// </summary>
        public static object getDescObject()
        {
            Dictionary<string, object> desc = new Dictionary<string, object>();
            desc.Add("format","integer");
            desc.Add("name","X1");
            desc.Add("visibility","parent");
            Dictionary<string, object> retention = new Dictionary<string, object>();
            retention.Add("count","infinity");
            retention.Add("duration","infinity");
            desc.Add("retention",retention);
            object[] p1 = new object[]{"add",10};
            object[] preprocess = new object[]{p1};
            desc.Add("preprocess",preprocess);
            return desc;			
        }
    }
}


