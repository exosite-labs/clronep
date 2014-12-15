/*=============================================================================
* OnepV1.cs
* Wrapper class for One Platofrm API procedures.
*==============================================================================
*
* Tested with .NET Framework 3.5
*
* Copyright (c) 2011, Exosite LLC
* All rights reserved.
*/

using System;
namespace clronep
{
    public class OnepV1
    {
        private HttpTransport transport;
        private string client_id = null;
        private string resource_id = null;
        public OnepV1(string url, int timeout)
        {
            transport = new HttpTransport(url, timeout);
        }
        private Result callRPC(string clientkey, string proc, object[] options)
        {
            string req;
            if (client_id != null)
            {
                req = JsonHandler.getRequest(clientkey, client_id, proc, options, true);
            }
            else if (resource_id != null)
            {
                req = JsonHandler.getRequest(clientkey, resource_id, proc, options, false);
            }
            else
            {
                req = JsonHandler.getRequest(clientkey, proc, options);
            }
            string res = transport.send(req);
            return JsonHandler.parseResponse(res);
        }

        public void set_proxy(string ip, int port, string user, string password)
        {
            transport.set_proxy(ip, port, user, password);
        }

        public void set_proxy(string ip, int port)
        {
            transport.set_proxy(ip, port, null, null);
        }
        public void connect_as(string clientid)
        {
            client_id = clientid;
            resource_id = null;
        }
        public void connect_owner(string resourceid)
        {
            resource_id = resourceid;
            client_id = null;
        }
        public Result activate(string clientkey, string codetype, string code)
        {
            object[] argu = new object[] { codetype, code };
            return callRPC(clientkey, "activate", argu);
        }
        /* The comment function is deprecated in the One Platform
        public Result comment(string clientkey, string rid, string visibility, string comment)
        {
            object[] argu = new object[] { rid, visibility, comment };
            return callRPC(clientkey, "comment", argu);
        }*/
        public Result create(string clientkey, string type, object desc)
        {
            object[] argu = new object[] { type, desc };
            return callRPC(clientkey, "create", argu);
        }
        public Result deactivate(string clientkey, string codetype, string code)
        {
            object[] argu = new object[] { codetype, code };
            return callRPC(clientkey, "deactivate", argu);
        }
        public Result drop(string clientkey, string rid)
        {
            object[] argu = new object[] { rid };
            return callRPC(clientkey, "drop", argu);
        }
        public Result drop(string clientkey, object rid)
        {
            object[] argu = new object[] { rid };
            return callRPC(clientkey, "drop", argu);
        }
        public Result flush(string clientkey, string rid)
        {
            object[] argu = new object[] { rid };
            return callRPC(clientkey, "flush", argu);
        }
        public Result flush(string clientkey, object rid)
        {
            object[] argu = new object[] { rid };
            return callRPC(clientkey, "flush", argu);
        }
        public Result info(string clientkey, string rid, object options)
        {
            object[] argu = new object[] { rid, options };
            return callRPC(clientkey, "info", argu);
        }
        public Result info(string clientkey, object rid, object options)
        {
            object[] argu = new object[] { rid, options };
            return callRPC(clientkey, "info", argu);
        }
        public Result listing(string clientkey, object types)
        {
            object[] argu = new object[] { types, EmptyOption.Instance };
            return callRPC(clientkey, "listing", argu);
        }
        public Result listing(string clientkey, object types, object options)
        {
            object[] argu = new object[] { types, options };
            return callRPC(clientkey, "listing", argu);
        }
        public Result lookup(string clientkey, string type, string alias)
        {
            object[] argu = new object[] { type, alias };
            return callRPC(clientkey, "lookup", argu);
        }
        public Result map(string clientkey, string rid, string alias)
        {
            object[] argu = new object[] { "alias", rid, alias };
            return callRPC(clientkey, "map", argu);
        }
        public Result read(string clientkey, string rid, object options)
        {
            object[] argu = new object[] { rid, options };
            return callRPC(clientkey, "read", argu);
        }
        public Result read(string clientkey, object rid, object options)
        {
            object[] argu = new object[] { rid, options };
            return callRPC(clientkey, "read", argu);
        }
        public Result record(string clientkey, string rid, object[] entries, object options)
        {
            object[] argu = new object[] { rid, entries, options };
            return callRPC(clientkey, "record", argu);
        }
        public Result record(string clientkey, object rid, object[] entries, object options)
        {
            object[] argu = new object[] { rid, entries, options };
            return callRPC(clientkey, "record", argu);
        }
        public Result revoke(string clientkey, string codetype, string code)
        {
            object[] argu = new object[] { codetype, code };
            return callRPC(clientkey, "revoke", argu);
        }
        public Result share(string clientkey, string rid, object options)
        {
            object[] argu = new object[] { rid, options };
            return callRPC(clientkey, "share", argu);
        }
        public Result share(string clientkey, object rid, object options)
        {
            object[] argu = new object[] { rid, options };
            return callRPC(clientkey, "share", argu);
        }
        public Result unmap(string clientkey, string alias)
        {
            object[] argu = new object[] { "alias", alias };
            return callRPC(clientkey, "unmap", argu);
        }
        public Result update(string clientkey, string rid, object desc)
        {
            object[] argu = new object[] { rid, desc };
            return callRPC(clientkey, "update", argu);
        }
        public Result update(string clientkey, object rid, object desc)
        {
            object[] argu = new object[] { rid, desc };
            return callRPC(clientkey, "update", argu);
        }
        public Result write(string clientkey, string rid, object value)
        {
            object[] argu = new object[] { rid, value, EmptyOption.Instance };
            return callRPC(clientkey, "write", argu);
        }
        public Result write(string clientkey, object rid, object value)
        {
            object[] argu = new object[] { rid, value, EmptyOption.Instance };
            return callRPC(clientkey, "write", argu);
        }
        public Result write(string clientkey, string rid, object value, object options)
        {
            object[] argu = new object[] { rid, value, options };
            return callRPC(clientkey, "write", argu);
        }
        public Result write(string clientkey, object rid, object value, object options)
        {
            object[] argu = new object[] { rid, value, options };
            return callRPC(clientkey, "write", argu);
        }
        public Result write(string clientkey, object[] entries)
        {
            object[] argu = new object[] { entries, EmptyOption.Instance };
            return callRPC(clientkey, "write", argu);
        }
        public Result write(string clientkey, object[] entries, object options)
        {
            object[] argu = new object[] { entries, options };
            return callRPC(clientkey, "write", argu);
        }
    }
}