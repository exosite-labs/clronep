/*=============================================================================
* OnepV1Test.cs
* Unit tests for OnepV1 class.  
* Note that CIK and RID strings ("PUTA40CHARACTER...") need to be replaced with
* valid values.
*==============================================================================
*
* Tested with .NET Framework 3.5
*
* Copyright (c) 2011, Exosite LLC
* All rights reserved.
*/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using clronep;

namespace clronep
{
	[TestFixture]
	public class OnepV1Test
	{
		OnepV1 conn;
        //NOTE: The easiest way to get a Client Interface Key (CIK - "clientKey") the first time is probably from Exosite Portals https://portals.exosite.com 
		string clientKey ="PUTA40CHARACTERCIKHERE";
        //NOTE: Use OnepV1.listing() to get a client's RIDs
        string rid = "PUTA40CHARACTERRIDHERE";       
		[SetUp]
		public void Init(){
			Thread.Sleep(1000);//sleep 1 second
            conn = new OnepV1("http://m2.exosite.com/onep:v1/rpc/process", 3);//timeout 3 seconds
		}
		/*
		[Test]
		public void comment(){
			string visibility = "public";
			string comment = "nothing but comment";
			Result obj1 = conn.comment(clientKey,rid,visibility,comment);
			Assert.AreEqual(Result.OK,obj1.status);
		}
		*/
		[Test]
		public void createdrop(){
			object desc = getDescObject();
			Result obj1 = conn.create(clientKey,"dataport",desc);
			Assert.AreEqual(Result.OK,obj1.status);
			string rid= obj1.message;
			Result obj2 = conn.drop(clientKey,rid);
			Assert.AreEqual(Result.OK,obj2.status);
		}
    	
		[Test]
		public void flush(){
			Result obj = conn.flush(clientKey,rid);
			Assert.AreEqual(Result.OK,obj.status);
		}
		[Test]
		public void info(){
			Result obj = conn.info(clientKey,rid,EmptyOption.Instance);
			Assert.AreEqual(Result.OK,obj.status);
			Assert.AreNotEqual(null,obj.message);	  
		}
	
		[Test]
		[ExpectedException(typeof(HttpRPCResponseException),ExpectedMessage="Unable to get http response.")]
		public void responseException(){
			conn = new OnepV1("http://m2.exosite.com/api:v1/rpc/processes",3);
			conn.lookup(clientKey,"alias","X1");
		}
		[Test]
		[ExpectedException(typeof(HttpRPCRequestException),ExpectedMessage="Unable to make http request.")]
		public void requestException(){
			conn = new OnepV1("http://0.0.0.1/api:v1/rpc/process",3);
			conn.lookup(clientKey,"alias","X1");
		}
		
		[Test]
		[ExpectedException(typeof(OnePlatformException))]
		public void invalidAuth()
		{
            //NOTE: The easiest way to get a Client Interface Key (CIK) the first time is probably from Exosite Portals https://portals.exosite.com 
            string cik = "PUTA40CHARACTERCIKHERE";  
			conn.lookup(cik,"alias","X1");	  
		}
		
		[Test]
		public void invalidArgument(){      
			Result obj = conn.lookup(clientKey,"aliases","X1");
			Assert.AreEqual(Result.FAIL,obj.status);
			Assert.AreEqual("badarg",obj.message);
		}
		[Test]
		public void listing(){
			string[] options = new string[]{"dataport"};
			Result obj = conn.listing(clientKey,options);
			Assert.AreEqual(Result.OK,obj.status);
			Assert.AreNotEqual(null,obj.message);	  
		}
		[Test]
		public void lookup(){      
			Result obj = conn.lookup(clientKey,"alias","X1");
			Assert.AreEqual(Result.OK,obj.status);
			Assert.AreEqual(rid,obj.message);
		}
		[Test]
		public void map(){
			object desc = getDescObject();
			Result tmp = conn.create(clientKey,"dataport",desc);
			string rid= tmp.message;
			string alias = "testonly";
			Result obj1 = conn.map(clientKey,rid,alias);
			Assert.AreEqual(Result.OK,obj1.status);
			Result obj2 = conn.unmap(clientKey,alias);
			Assert.AreEqual(Result.OK,obj2.status);	
			Result obj3 = conn.drop(clientKey,rid);
			Assert.AreEqual(Result.OK,obj3.status);
		}
		[Test]
		public void readwrite(){
			Result obj = conn.write(clientKey,rid,70);
			Assert.AreEqual(Result.OK,obj.status);
			Assert.AreEqual(Result.OK,obj.message);
			Dictionary<string, object> argu = new Dictionary<string, object>();
			argu.Add("limit",1);
			argu.Add("sort", "desc");
			obj = conn.read(clientKey,rid,argu);
			Assert.AreEqual(Result.OK,obj.status);
			Assert.AreNotEqual(null,obj.message);	 
		}
		
		[Test]
		public void recordbatch(){
			object[] data1 = new object[]{-2,1};
			object[] data2 = new object[]{-1,2};	
			object[] entries =	new object[]{data1,data2};
			Result obj = conn.recordbatch(clientKey,rid,entries,EmptyOption.Instance);
			Assert.AreEqual(Result.OK,obj.status);	
		}
		
		[Test]
		public void update(){
			Dictionary<string, object> desc = new Dictionary<string, object>();
			desc.Add("name","new");
			object[] p = new object[]{"add",1};
			object[] preprocess = new object[]{p};
			desc.Add("preprocess",preprocess);		
			Result obj = conn.update(clientKey,rid,desc);
			Assert.AreEqual(Result.OK,obj.status);	  	
		}
		
		[Test]
		public void writegroup(){
			Result obj = conn.lookup(clientKey,"alias","X1");
			Assert.AreEqual(Result.OK,obj.status);
			string rid1 = obj.message;
			obj = conn.lookup(clientKey,"alias","X2");
			Assert.AreEqual(Result.OK,obj.status);
			string rid2 = obj.message;
			object[] data1 = new object[]{rid1,1};
			object[] data2 = new object[]{rid2,2};
			object[] data = new object[]{data1,data2};
			obj = conn.write(clientKey,data);
			Assert.AreEqual(Result.OK,obj.status);
			Assert.AreEqual(Result.OK,obj.message); 
		}
		
		public object getDescObject(){			
			Dictionary<string, object> desc = new Dictionary<string, object>();
			desc.Add("format","integer");
			desc.Add("name","testcreate");
			desc.Add("visibility","parent");
			Dictionary<string, object> retention = new Dictionary<string, object>();
			retention.Add("count","infinity");
			retention.Add("duration","infinity");
			desc.Add("retention",retention);
			object[] p1 = new object[]{"add",5};
			object[] preprocess = new object[]{p1};
			desc.Add("preprocess",preprocess);
			return desc;
		}		
	}
}