using System;
using System.Collections.Generic;

namespace clronep.provisionExamples
{
    class provisionExample
    {
        static void Main(string[] args)
        {
            provisionForm frm = new provisionForm();
            frm.ShowDialog();
            string vendorname = frm.vendorname;
            string vendortoken = frm.vendortoken;
            string clonecik = frm.clonecik;
            string cloneportalcik = frm.cloneportalcik; //use only if managing by sharecode
            string portalcik = frm.portalcik;

            int r = new Random().Next(1, 10000000);
            string model = "MyTestModel" + r.ToString();
            string sn1 = "001" + r.ToString();
            string sn2 = "002" + r.ToString();
            string sn3 = "003" + r.ToString();
            OnepV1 op = new OnepV1("http://m2.exosite.com/onep:v1/rpc/process", 3);
            Result portalridResult = op.lookup(portalcik, "alias", "");
            string portalrid = null;
            string clonerid = null;
            if (portalridResult.status == Result.OK)
            {
                portalrid = portalridResult.message;
                Console.WriteLine("\r\nportalrid: " + portalrid);
            }
            else Console.WriteLine("\r\nFailed to look up portal RID");
            Result cloneridResult = op.lookup(clonecik, "alias", "");
            Provision provision = null;
            if (cloneridResult.status == Result.OK)
            {
                clonerid = cloneridResult.message;
                Console.WriteLine("\r\nclonerid: " + clonerid);
                provision = new Provision("http://m2.exosite.com", 3, false, true);
            }
            else Console.WriteLine("\r\nFailed to look up clone RID");
            Dictionary<string, string> meta = new Dictionary<string, string>();
            string[] options = new string[2];
            options[0] = vendorname;
            options[1] = model;
            string option = "[" + "\"" + vendorname + "\"" + ", " + "\"" + model + "\"" + "]";
            meta.Add("meta", option);
            string sharecode = op.share(cloneportalcik, clonerid, meta).message;
            try
            {
                Console.WriteLine("\r\nmodel_create()");
                Console.WriteLine("\r\n" + provision.model_create(vendortoken, model, sharecode, false, true, true).message);
                Console.WriteLine("\r\nmodel_list()\r\n"+provision.model_list(vendortoken).message);
                Console.WriteLine("\r\nmodel_info()\r\n" + provision.model_info(vendortoken, model).message);
                Console.WriteLine("\r\nserialnumber_add()");
                Console.WriteLine("\r\n" + provision.serialnumber_add(vendortoken, model, sn1).message);
                Console.WriteLine("\r\nserialnumber_add_batch()");
                string[] sn2andsn3 = new string[2];
                sn2andsn3[0] = sn2;
                sn2andsn3[1] = sn3;
                Console.WriteLine("\r\n" + provision.serialnumber_add_batch(vendortoken, model, sn2andsn3).message);
                Console.WriteLine("\r\nserialnumber_list()\r\n" + provision.serialnumber_list(vendortoken, model, 0, 10).message);
                Console.WriteLine("\r\nserialnumber_remove_batch()");
                Console.WriteLine("\r\n" + provision.serialnumber_remove_batch(vendortoken, model, sn2andsn3).message);
                Console.WriteLine("\r\nserialnumber_list()\r\n" + provision.serialnumber_list(vendortoken, model, 0, 1000).message);
                Console.WriteLine("\r\nserialnumber_enable()"); 
                provision.serialnumber_enable(vendortoken, model, sn1, portalrid); //return clientid
                Console.WriteLine("\r\nAFTER ENABLE: " + provision.serialnumber_info(vendortoken, model, sn1).message);
                Console.WriteLine("\r\nserialnumber_disable()");
                provision.serialnumber_disable(vendortoken, model, sn1);
                Console.WriteLine("\r\nAFTER DISABLE: " + provision.serialnumber_info(vendortoken, model, sn1).message);
                Console.WriteLine("\r\nserialnumber_reenable()");
                provision.serialnumber_reenable(vendortoken, model, sn1);
                Console.WriteLine("\r\nAFTER REENABLE: " + provision.serialnumber_info(vendortoken, model, sn1).message);
                Console.WriteLine("\r\nserialnumber_activate()");
                //return client key
                string sn_cik = provision.serialnumber_activate(model, sn1, vendorname).message;
                Console.WriteLine("\r\nAFTER ACTIVATE: " + provision.serialnumber_info(vendortoken, model, sn1).message);

                Console.WriteLine("\r\ncontent_create()");
                Console.WriteLine("\r\n" + provision.content_create(vendortoken, model, "a.txt", "This is text", false).message);
                Console.WriteLine("\r\ncontent_upload()");
                Console.WriteLine("\r\n" + provision.content_upload(vendortoken, model, "a.txt", "This is content data", "text/plain").message);
                Console.WriteLine("\r\ncontent_list()\r\n" + provision.content_list(vendortoken, model).message);
                Console.WriteLine(vendortoken, model, "a.txt");
                Console.WriteLine("\r\ncontent_remove()");
                provision.content_remove(vendortoken, model, "a.txt");

                Console.WriteLine("\r\nmodel_remove()");
                provision.model_remove(vendortoken, model);
            }
            catch (ProvisionException e) 
            {
                Console.WriteLine("\r\nprovisionExample sequence exception:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
