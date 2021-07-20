using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Services.Nodes
{
    public static class ScaffoldServers
    {
        public static List<Uri> Get()
        {
            List<Uri> lstInitialServers = new();
            lstInitialServers.Add(new Uri("http://localhost:10000"));
            lstInitialServers.Add(new Uri("http://localhost:10001"));
            lstInitialServers.Add(new Uri("http://localhost:10002"));
            return lstInitialServers;
        }
        //public static bool Connect(string nodeId)
        //{
        //    try
        //    {
        //        var url = GetUri();
        //        using (var client = new WebClient())
        //        {
        //            var data = new NameValueCollection();
        //            data["nodeId"] = nodeId;
        //            var response = client.UploadValues(url, "POST", data);
        //            string responseInString = Encoding.UTF8.GetString(response);
        //            Console.WriteLine(responseInString);
        //        }
        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        Console.WriteLine(exc.Message);
        //        return false;
        //    }
        //}
    }
}
