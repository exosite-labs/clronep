/*
==============================================================================
* ClientOnepV1.cs
* Provides simple use for 1p client layer given CIK.
==============================================================================

Tested with .NET Framework 3.5

Copyright (c) 2011, Exosite LLC
All rights reserved.
*/
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;

namespace clronep
{
    public class ClientOnepV1 : OnepV1
    {
        public ClientOnepV1(string url, int timeout, string cik)
            : base(url, timeout)
        {
            _CIK = cik;
        }
        private Dictionary<string, string> _AliasDict = new Dictionary<string, string>();
        private string _RID = null;
        private string _CIK = null;
        public string CIK
        {
            set
            {
                _CIK = value;
                _RID = null;
                _AliasDict.Clear();
                getAllAliasesInfo();
            }
            get
            {
                return _CIK;
            }
        }

        private Result doCreate(string type, string alias, object desc)
        {
            string rid = null;
            Result res1 = create(_CIK, type, desc);
            if (statusOK(res1))
            {
                rid = res1.message;
                Result res2 = map(_CIK, rid, alias);
                if (!statusOK(res2))
                {
                    drop(_CIK, rid);
                    return res2;
                }
                _AliasDict[alias] = rid;
            }
            return res1;
        }

        private string getRID(string alias)
        {
            if (_AliasDict.ContainsKey(alias))
            {
                return _AliasDict[alias];
            }
            else
            {
                Result res = lookup(_CIK, "alias", alias);
                if (statusOK(res))
                {
                    string rid = res.message;
                    _AliasDict[alias] = rid;
                    return rid;
                }
                else
                {
                    throw new OnePlatformException(res.message);
                }
            }
        }

        private bool statusOK(Result res)
        {
            return res.status == Result.OK;
        }       

        /*
         * the comment function has been deprecated in One Platform
        public Result comment(string alias, string visibility, string comments)
        {
            string rid = getRID(alias);
            return comment(_CIK, rid, visibility, comments);
        }
        */
        public Result create(string alias, IDescription desc)
        {
            if (desc is DataportDescription)
            {
                return doCreate("dataport", alias, desc);
            }
            return null;
        }

        public Dictionary<string, string> getAllAliasesInfo()
        {
            Dictionary<string, string> retDict = new Dictionary<string, string>();
            if (_RID == null)
            {
                Result res1 = lookup(_CIK, "alias", "");
                if (statusOK(res1))
                {
                    _RID = res1.message;
                }
                else return retDict;
            }
            Dictionary<string, object> option = new Dictionary<string, object>();
            option.Add("aliases", true);
            Result res2 = info(_CIK, _RID, option);
            if (statusOK(res2))
            {
                _AliasDict.Clear();
                JObject jobj = null;
                try
                {
                    jobj = JObject.Parse(res2.message);
                    foreach (JProperty p in jobj["aliases"])
                    {
                        string rid = p.Name;
                        JArray jarr = (JArray)jobj["aliases"][rid];
                        string alias = (string)jarr[0];
                        _AliasDict[alias] = rid;
                        retDict.Add(alias, rid);
                    }
                }
                catch (Exception)
                {
                    throw new OneException("Unable to decode returned aliases info string.");
                }
            }
            return retDict;
        }

        public Result info(string alias, object options)
        {
            string rid = getRID(alias);
            Result res = info(_CIK, rid, options);
            if (!statusOK(res) && res.message == "restricted")
            {
                getAllAliasesInfo();
            }
            return res;
        }

        public Result read(string alias, int count)
        {
            if (count <= 0) { count = 1; }
            string rid = getRID(alias);
            Dictionary<string, object> argu = new Dictionary<string, object>();
            argu.Add("limit", count);
            argu.Add("sort", "desc");
            Result res = read(_CIK, rid, argu);
            if (!statusOK(res) && res.message == "restricted")
            {
                getAllAliasesInfo();
            }
            return res;
        }

        public Result read(string alias)
        {
            return read(alias, 1);
        }

        public Result update(string alias, object description)
        {
            string rid = getRID(alias);
            Result res = update(_CIK, rid, description);
            if (!statusOK(res) && res.message == "restricted")
            {
                getAllAliasesInfo();
            }
            return res;
        }

        public Result write(string alias, object data)
        {
            string rid = getRID(alias);
            Result res = write(_CIK, rid, data);
            if (!statusOK(res) && res.message == "restricted")
            {
                getAllAliasesInfo();
            }
            return res;
        }

        public Result write(Dictionary<string, object> entries)
        {
                ArrayList data = new ArrayList();            
                foreach (string alias in entries.Keys)
                {
                    string rid = null;
                    try
                    {
                        rid = getRID(alias);
                    }catch(OneException){
                        continue;
                    }
                    object val = entries[alias];
                    object[] entry = new object[] { rid, val };
                    data.Add(entry);              
                }              
                return base.write(_CIK, data.ToArray());
        }

        public Result writegroup(Dictionary<string, object> entries)
        {
            Dictionary<string, object> entry = new Dictionary<string,object>();
            foreach (string alias in entries.Keys)
            {
                string rid = null;
                try
                {
                    rid = getRID(alias);
                }
                catch (OneException)
                {
                    continue;
                }
                object val = entries[alias];
                entry.Add(rid, val);
            }
            return base.writegroup(_CIK, entry);
        }
    }
}
