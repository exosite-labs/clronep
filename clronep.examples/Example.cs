/*=============================================================================
* Examples.cs
* Use-case examples.
*==============================================================================
*
* Tested with .NET Framework 4.6
*
* Copyright (c) 2015, Exosite LLC
* All rights reserved.
*/

using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
namespace clronep.examples
{
    /// <summary>
    /// Contains methods that are used to demonstrate use-case examples for 
    /// the clronep library
    /// </summary>
    class Example
    {
        /// <summary>
        /// Main method - calls into example sequences for layer 1 API (raw)
        /// and layer 2 API (buffered, batch)
        /// </summary>
        public static void Main(string[] args)
        {
            //NOTE: The easiest way to get a Client Interface Key (CIK) the first time is probably from Exosite Portals https://portals.exosite.com 
            string cik;
            OnePV1Form frm = new OnePV1Form();
            frm.ShowDialog();
            cik = frm.cik;
            OnepV1Examples(cik);         //Layer 1 (low level) examples
            ClientOnepV1Examples(cik);   //Layer 2 (batched/overloaded) examples
        }

        /// <summary>
        /// Worker function for wait threading in OnepV1Examples
        /// </summary>
        public static void waitFunction(string cik, string rid, OnepV1 oneConn)
        {
            Dictionary<string, int> waitOptions = new Dictionary<string, int>();
            waitOptions.Add("timeout", 30000);
            Console.WriteLine("Waiting on dataport with RID " + rid);
            Result result = oneConn.wait(cik, rid, waitOptions);
            if (result.status == Result.OK)
            {
                Console.WriteLine("\r\nWait output: ");
                Console.WriteLine(result.message);
                Console.WriteLine("\r\n");
            }
        }

        /// <summary>
        /// Worker function for write threading in OnepV1Examples
        /// </summary>
        public static void waitWrite(string cik, string rid, OnepV1 oneConn)
        {
            Thread.Sleep(10000);
            int val = new Random().Next(1, 100);
            Result result = oneConn.write(cik, rid, val);
            if (result.status == Result.OK)
            {
                Console.WriteLine(val+10 +" is the expected output of the wait");
            }
        }
        
        /// <summary>
        /// OnepV1Examples method - example sequence for layer 1 calls. Layer 1
        /// calls are direct bindings to the 1P API.
        /// </summary>
        public static void OnepV1Examples(string cik)
        {
            //OnepV1(url, timeout)
            OnepV1 oneConn = new OnepV1("https://m2.exosite.com/onep:v1/rpc/process", 35);
            Result result;
            try
            {
                // Get resource id of dataport given its alias name 'X1'
                string alias_name = "X1";
                result = oneConn.lookup(cik, "alias", alias_name);
                string rid = null;
                if (result.status == Result.OK)
                {
                    rid = result.message;
                } else {
                    // If dataport with alias 'X1' didn't exist, we will create it
                    Console.WriteLine("Could not find Dataport with alias " + alias_name + ", creating...");
                    result = oneConn.create(cik, "dataport", getDescObject());
                    if (result.status == Result.OK)
                    {
                        rid = result.message;
                        result = oneConn.map(cik, rid, alias_name);
                        Console.WriteLine("Dataport: " + rid + " (" + alias_name + ") is created.");
                    }
                }
                
                // Write data to dataport
                int val = new Random().Next(1, 100);
                result = oneConn.write(cik, rid, val);
                if (result.status == Result.OK)
                {
                    Console.WriteLine("Dataport " + rid + " is written with raw value " + val + ".");
                }
                
                // Read data from dataport
                result = oneConn.read(cik, rid, EmptyOption.Instance);
                if (result.status == Result.OK)
                {
                    object[][] read = JsonConvert.DeserializeObject<object[][]>(result.message);
                    val = Int32.Parse(read[0][1].ToString());
                    Console.WriteLine("Dataport " + rid + " is read back as: " + val + " (value stored is different from raw write value due to pre-process rule).");
                }
                
                // Create and then drop a dataport (note - a client can have many dataports w/ same name, but alias & RID must be unique)
                object desc = getDescObject();
                result = oneConn.create(cik, "dataport", desc);
                if (result.status == Result.OK)
                {
                    rid = result.message;
                    Console.WriteLine("\r\nDataport: " + rid + " is created.");
                    alias_name = "test_alias";
                    // map/unmap alias to dataport
                    result = oneConn.map(cik, rid, alias_name);
                    if (result.status == Result.OK)
                    {
                        Console.WriteLine("Dataport: " + rid + " is mapped to alias '" + alias_name + "'");
                        // Un-map the alias from the dataport
                        result = oneConn.unmap(cik, alias_name);
                        if (result.status == Result.OK)
                        {
                            Console.WriteLine("Dataport: " + rid + " is unmapped from alias '" + alias_name + "'");
                        }
                    }
                    result = oneConn.drop(cik, rid);
                    if (result.status == Result.OK)
                    {
                        Console.WriteLine("Dataport: " + rid + " is dropped.");
                    }
                }
                
                // List a client's dataports
                string[] options = new string[] { "dataport" };
                result = oneConn.listing(cik, options);
                if (result.status == Result.OK)
                {
                    Console.WriteLine("\r\nList of all Dataport RIDs for client CIK " + cik + ":");
                    Console.WriteLine(result.message);
                }
                
                /* Get all mapping alias information for dataports */
                // Get resource id of device given device key
                result = oneConn.lookup(cik, "alias", "");
                rid = result.message;
                // Get the alias information of given device
                Dictionary<string, object> option = new Dictionary<string, object>();
                option.Add("aliases", true);
                result = oneConn.info(cik, rid, option);
                if (result.status == Result.OK)
                {
                    Console.WriteLine("\r\nList of all Dataports with an alias for client CIK " + cik + ":");
                    Console.WriteLine(result.message);
                    Console.WriteLine("\r\n");
                }

                //wait example
                rid = oneConn.lookup(cik, "alias", "X1").message;
                ThreadStart starter = delegate { waitFunction(cik, rid, oneConn); };
                Thread thread = new Thread(starter);
                ThreadStart starter2 = delegate { waitWrite(cik, rid, oneConn); };
                Thread thread2 = new Thread(starter2);
                thread.Start();
                thread2.Start();
                Console.WriteLine("Waiting with timeout of 30 seconds for a write that will occur in 10 seconds");
                thread2.Join(Timeout.Infinite);
                thread.Join(Timeout.Infinite);
            }
            catch (OneException e)
            {
                Console.WriteLine("\r\nOnepV1Examples sequence exception:");
                Console.WriteLine(e.Message); 
            }
        }
        
        /// <summary>
        /// ClientOnepV1Examples method - example sequence for Layer 2 calls.
        /// Layer 2 uses Layer 1, but add some function overloading and batch
        /// sequences to simplify common use cases
        /// </summary>
        public static void ClientOnepV1Examples(string cik)
        {
            //ClientOnepV1(url, timeout, cik)
            ClientOnepV1 conn = new ClientOnepV1("https://m2.exosite.com/onep:v1/rpc/process", 3, cik);
            int val = new Random().Next(1, 100);
            string alias_name = "X1";
            string alias2_name = "X2";
            Result result;

            //write data to alias
            try
            {
                Console.WriteLine("Writing to alias " + alias_name + ":");
                result = conn.write(alias_name, val);
                if (result.status == Result.OK)
                {
                    Console.WriteLine("Successfully wrote value: " + val + ".\r\n");
                }
            }
            catch(OnePlatformException e)
            {
                Console.WriteLine("ClientOnepV1Examples, write exception:");
                Console.WriteLine(e.Message + "\r\n"); 
            }            
            
            // read data from alias
            try
            {
                Console.WriteLine("Reading from alias " + alias_name + ":");
                result = conn.read(alias_name);
                if (result.status == Result.OK)
                {
                    object[][] read = JsonConvert.DeserializeObject<object[][]>(result.message);
                    val = Int32.Parse(read[0][1].ToString());
                    Console.WriteLine("Successfully read value: " + val + ".\r\n");
                }
            }
            catch (OneException e)
            { 
                Console.WriteLine("ClientOnepV1Examples, read exception:");
                Console.WriteLine(e.Message + "\r\n"); 
            }

            //create dataport
            try
            {
                Console.WriteLine("Creating a new dataport named " + alias2_name + ":");
                DataportDescription desc = new DataportDescription("integer");//integer format
                desc.retention.count = 10;    // only allow 10 data points to be stored to the resource
                desc.retention.duration = 1; // only allow the platform to keep the data points for 1 hour
                desc.visibility = "parent";
                desc.name = "Friendly Name";
                result = conn.create(alias2_name, desc);
                if (result.status == Result.OK)
                {
                    Console.WriteLine("Success.\r\n");
                }
                else Console.WriteLine("Unsuccessful: " + result.status + ".\r\n");
            }
            catch (OneException e)
            {
                Console.WriteLine("ClientOnepV1Examples, create dataport exception:");
                Console.WriteLine(e.Message + "\r\n");
            } 
            
            //write group data
            try
            {
                val = new Random().Next(1, 100);
                Console.WriteLine("Writing value " + val + " to aliases " + alias_name + ", " + alias2_name + " as a group:");
                Dictionary<string, object> entries = new Dictionary<string, object>();
                entries.Add(alias_name, val);
                entries.Add(alias2_name, val);
                result = conn.writegroup(entries); 
                //NOTE: this call returns Result.OK regardless if _any_ of the writes were successful or not
                if (result.status == Result.OK)
                {
                    Console.WriteLine("Wrote a value of " + val + " to dataports.\r\n");
                }
            }
            catch (OneException e)
            { 
                Console.WriteLine("ClientOnepV1Examples, group write exception:");  
                Console.WriteLine(e.Message + "\r\n"); 
            }
            
            //get all aliases information
            Console.WriteLine("Listing all alias information for client:");            
            Dictionary<string,string> aliasDict = conn.getAllAliasesInfo();
            foreach( string alias in aliasDict.Keys){
                string rid = aliasDict[alias];
                Console.WriteLine(alias + "," + rid);
            }
            Console.WriteLine("\r\n");
            
            /* The comment function has been deprecated in One Platform.  The unit parameter is 
             * now stored in the meta field of clients by Portals.
            // comment  
            try
            {
                Console.WriteLine("Adding \"unit\" parameter to dataport comment field:"); 
                Dictionary<string, string> unit = new Dictionary<string, string>();
                unit.Add("unit", "%");
                string unitstr = JsonConvert.SerializeObject(unit);
                //add "unit" using 'comment' method
                conn.comment(alias_name, "public", unitstr);
                
                //get "unit" using 'info' method
                Dictionary<string, object> option = new Dictionary<string, object>();
                option.Add("comments", true);
                result = conn.info(alias_name, option);
                //Console.WriteLine(res.message);
                Dictionary<string, object[][]> a = JsonConvert.DeserializeObject<Dictionary<string, object[][]>>(result.message);
                Dictionary<string, string> b = JsonConvert.DeserializeObject<Dictionary<string, string>>(a["comments"][0][1].ToString());
                Console.WriteLine("Read back from comment field for parameter \"unit\" as: " + b["unit"] + ".\r\n");              
            }
            catch (OneException e)
            { 
                Console.WriteLine("ClientOnepV1Examples, comment exception:");
                Console.WriteLine(e.Message + "\r\n"); 
            }  
             * 
             * 
             */
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

