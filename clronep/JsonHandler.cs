/*=============================================================================
* JsonHandler.cs
* Parse and process JSON-RPC request/response string.
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace clronep
{
    static internal class JsonHandler
    {
        private class Auth
        {
            public string cik { get; set; }
            public string client_id { get; set; }
            public string resource_id { get; set; }
        }
        private class Request
        {
            public Call[] calls { get; set; }
            public Auth auth { get; set; }
        }
        private class Call
        {
            public int id { get; set; }
            public string procedure { get; set; }
            public object[] arguments { get; set; }
        }
        private class CallResponse
        {
            public int id { get; set; }
            public String status { get; set; }
            public object result { get; set; }
            public OnepError error { get; set; }
        }
        private class OnepError
        {
            public int code { get; set; }
            public String message { get; set; }
            public String context { get; set; }
        }
        static internal string getRequest(string parentkey, string proc, object[] args)
        {
            if (proc == "writegroup")
            {
                List<object> newArgs = new List<object>();
                foreach (Dictionary<string, object> dict in args)
                {
                    foreach (KeyValuePair<string, object> kvp in dict)
                    {
                        object[] formattingArray = new object[2];
                        formattingArray[0] = kvp.Key;
                        formattingArray[1] = kvp.Value;
                        newArgs.Add(formattingArray);
                    }
                    args[0] = newArgs.ToArray();
                }
            }
            Call call = new Call { id = 1, procedure = proc, arguments = args };
            Call[] calls = new Call[] { call };
            Auth auth = new Auth { cik = parentkey };
            Request req = new Request { auth = auth, calls = calls };
            return JsonConvert.SerializeObject(req, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
        static internal string getRequest(string parentkey, string id, string proc, object[] args, bool connect_as)
        {
            if (proc == "writegroup")
            {
                List<object> newArgs = new List<object>();
                foreach (Dictionary<string, object> dict in args)
                {
                    foreach (KeyValuePair<string, object> kvp in dict)
                    {
                        object[] formattingArray = new object[2];
                        formattingArray[0] = kvp.Key;
                        formattingArray[1] = kvp.Value;
                        newArgs.Add(formattingArray);
                    }
                    args[0] = newArgs.ToArray();
                }
            }
            Call call = new Call { id = 1, procedure = proc, arguments = args };
            Call[] calls = new Call[] { call };
            Auth auth;
            if (connect_as)
            {
                auth = new Auth { cik = parentkey, client_id = id };
            }
            else
            {
                auth = new Auth { cik = parentkey, resource_id = id };
            }
            Request req = new Request { auth = auth, calls = calls };
            return JsonConvert.SerializeObject(req, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
        static internal Result parseResponse(string res)
        {
            CallResponse[] responses = null;
            try
            {
                responses = JsonConvert.DeserializeObject<CallResponse[]>(res);
            }
            catch (Exception)
            {
                CallResponse cr = JsonConvert.DeserializeObject<CallResponse>(res);
                string errmsg = JsonConvert.SerializeObject(cr.error);
                throw new OnePlatformException(errmsg);
            }
            try
            {
                string status = responses[0].status;
                object result = responses[0].result;
                OnepError error = responses[0].error;
                if (responses[0].error != null)
                {
                    Console.WriteLine("b");
                    throw new OnePlatformException(error.message);
                }
                if (Result.OK == status)
                {
                    if (null == result)
                    {
                        return new Result { status = Result.OK, message = Result.OK };
                    }
                    else
                    {
                        return new Result { status = Result.OK, message = result.ToString() };
                    }
                }
                else
                {
                    return new Result { status = Result.FAIL, message = status };
                }
            }
            catch (Exception)
            {
                throw new OneException("Unknown exception.");
            }
        }
    }
}
