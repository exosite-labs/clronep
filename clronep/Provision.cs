using System;
using System.Collections.Generic;
using System.Net;

namespace clronep
{
    public class Provision
    {
        public static string PROVISION_BASE = "/provision";
        public static string PROVISION_ACTIVATE = PROVISION_BASE + "/activate";
        public static string PROVISION_DOWNLOAD = PROVISION_BASE + "/download";
        public static string PROVISION_MANAGE = PROVISION_BASE + "/manage";
        public static string PROVISION_MANAGE_MODEL = PROVISION_MANAGE + "/model/";
        public static string PROVISION_MANAGE_CONTENT = PROVISION_MANAGE + "/content/";
        public static string PROVISION_REGISTER = PROVISION_BASE + "/register";

        private HttpTransport transport;
        private bool manage_by_cik;
        private bool manage_by_sharecode;

        public Provision(string url, int timeout, bool managebycik, bool managebysharecode)
        {
            transport = new HttpTransport(url, timeout);
            manage_by_cik = managebycik;
            manage_by_sharecode = managebysharecode;
        }

        private string[] filter_options(bool aliases, bool comments, bool historical)
        {
            string[] options = new string[3];
            int i = 0;
            if (aliases == false)
            {
                options[i] = "noaliases";
                i++;
            }
            if (comments == false)
            {
                options[i] = "nocomments";
                i++;
            }
            if (historical == false)
            {
                options[i] = "nohistorical";
                i++;
            }
            return options;
        }
        private Result request(string path, string key, string data, string method, bool key_is_cik, WebHeaderCollection extra_headers)
        {
            string url, body;
            if (method == "GET")
            {
                if (data.Length > 0)
                    url = path + "?" + data;
                else
                    url = path;
                body = null;    
            }
            else
            {
                url = path;
                body = data;
            }
            WebHeaderCollection headers = new WebHeaderCollection();
            if (key_is_cik)
                headers.Add("X-Exosite-CIK", key);
            else
                headers.Add("X-Exosite-Token", key);
            if (method == "POST")
                headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            headers.Add("Accept", "text/plain, text/csv, application/x-www-form-urlencoded");
            if (extra_headers != null) { 
            foreach (string webkey in extra_headers.Keys)
                headers.Add(webkey, extra_headers[webkey]);
                }
            string[] response = transport.provisionSend(body, method, url, headers);
            return JsonHandler.parseProvisionResponse(response[1], response[0]);
        }

        public Result content_create(string key, string model, string contentid, string meta, bool protect)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("id", contentid);
            parameters.Add("meta", meta);
            if (protect != false) parameters["protected"] = "true";
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_CONTENT + model + "/";
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result content_download(string cik, string vendor, string model, string contentid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vendor", vendor);
            parameters.Add("model", model);
            parameters.Add("id", contentid);

            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("Accept", "*");
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            return request(PROVISION_DOWNLOAD, cik, data, "GET", true, headers);
        }
        public Result content_info(string key, string model, string contentid, string vendor)
        {
            if (vendor == null)
            {
                string path = PROVISION_MANAGE_CONTENT + model + "/" + contentid;
                return request(path, key, "", "GET", manage_by_cik, null);
            }
            else
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("vendor", vendor);
                parameters.Add("model", model);
                parameters.Add("info", "true");
                List<string> dataList = new List<string>();
                foreach (string s in parameters.Keys)
                    dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
                string data = String.Join("&", dataList.ToArray());
                return request(PROVISION_DOWNLOAD, key, data, "GET", manage_by_cik, null);
            }
        }
        public Result content_list(string key, string model){
            string path = PROVISION_MANAGE_CONTENT + model + "/";
            return request(path, key, "", "GET", manage_by_cik, null);
        }
        public Result content_remove(string key, string model, string contentid)
        {
            string path = PROVISION_MANAGE_CONTENT + model + "/" + contentid;
            return request(path, key, "", "DELETE", manage_by_cik, null);
        }
        public Result content_upload(string key, string model, string contentid, string data, string mimetype)
        {
            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("Content-Type", mimetype);
            string path = PROVISION_MANAGE_CONTENT + model + "/" + contentid;
            return request(path, key, data, "POST", manage_by_cik, headers);
        }
        public Result model_create(string key, string model, string sharecode, bool aliases, bool comments, bool historical)
        {
            string[] options = filter_options(aliases, comments, historical);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("model", model);
            if (manage_by_sharecode) parameters.Add("code", sharecode);
            else parameters.Add("rid", sharecode);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            foreach (string s in options)
                if (s != null) dataList.Add(String.Concat("options[]", "=", Uri.EscapeDataString(s)));
            string data = String.Join("&", dataList.ToArray());
            return request(PROVISION_MANAGE_MODEL, key, data, "POST", manage_by_cik, null);
        }
        public Result model_info(string key, string model)
        {
            return request(PROVISION_MANAGE_MODEL + model, key, "", "GET", manage_by_cik, null);
        }
        public Result model_list(string key)
        {
            return request(PROVISION_MANAGE_MODEL, key, "", "GET", manage_by_cik, null);
        }
        public Result model_remove(string key, string model)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("delete", "true");
            parameters.Add("model", model);
            parameters.Add("confirm", "true");
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model;
            return request(path, key, data, "DELETE", manage_by_cik, null);
        }
        public Result model_update(string key, string model, string clonerid, bool aliases, bool comments, bool historical)
        {
            string[] options = filter_options(aliases, comments, historical);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rid", clonerid);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            foreach (string s in options)
                if (s != null) dataList.Add(String.Concat("options[]", "=", Uri.EscapeDataString(s)));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model;
            return request(path, key, data, "PUT", manage_by_cik, null);
        }
        public Result serialnumber_activate(string model, string serialnumber, string vendor)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vendor", vendor);
            parameters.Add("model", model);
            parameters.Add("sn", serialnumber);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            return request(PROVISION_ACTIVATE, "", data, "POST", manage_by_cik, null);
        }
        public Result serialnumber_add(string key, string model, string sn)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("add", "true");
            parameters.Add("sn", sn);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/";
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result serialnumber_add_batch(string key, string model, string[] sns)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("add", "true");
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            foreach (string sn in sns)
                dataList.Add(String.Concat("sn[]", "=", Uri.EscapeDataString(sn)));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/";
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result serialnumber_disable(string key, string model, string serialnumber)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("disable", "true");
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/" + serialnumber;
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result serialnumber_enable(string key, string model, string serialnumber, string owner)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("enable", "true");
            parameters.Add("owner", owner);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/" + serialnumber;
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result serialnumber_info(string key, string model, string serialnumber)
        {
            string path = PROVISION_MANAGE_MODEL + model + "/" + serialnumber;
            return request(path, key, "", "GET", manage_by_cik, null);
        }
        public Result serialnumber_list(string key, string model, int offset, int limit)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("offset", offset.ToString());
            parameters.Add("limit", limit.ToString());
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/";
            return request(path, key, data, "GET", manage_by_cik, null);
        }
        public Result serialnumber_reenable(string key, string model, string serialnumber)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("enable", "true");
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/" + serialnumber;
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result serialnumber_remap(string key, string model, string serialnumber, string oldsn)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("enable", "true");
            parameters.Add("oldsn", oldsn);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/" + serialnumber;
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result serialnumber_remove(string key, string model, string serialnumber)
        {
            string path = PROVISION_MANAGE_MODEL + model + "/" + serialnumber;
            return request(path, key, "", "DELETE", manage_by_cik, null);
        }
        public Result serialnumber_remove_batch(string key, string model, string[] sns)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("remove", "true");
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            foreach (string sn in sns)
                dataList.Add(String.Concat("sn[]", "=", Uri.EscapeDataString(sn)));
            string data = String.Join("&", dataList.ToArray());
            string path = PROVISION_MANAGE_MODEL + model + "/";
            return request(path, key, data, "POST", manage_by_cik, null);
        }
        public Result vendor_register(string key, string vendor)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vendor", vendor);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            return request(PROVISION_REGISTER, key, data, "POST", manage_by_cik, null);
        }
        public Result vendor_show(string key)
        {
            return request(PROVISION_REGISTER, key, "", "GET", false, null);
        }
        public Result vendor_unregister(string key, string vendor) 
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("delete", "true");
            parameters.Add("vendor", vendor);
            List<string> dataList = new List<string>();
            foreach (string s in parameters.Keys)
                dataList.Add(String.Concat(s, "=", Uri.EscapeDataString(parameters[s])));
            string data = String.Join("&", dataList.ToArray());
            return request(PROVISION_REGISTER, key, data, "POST", false, null);
        }
    }
}
