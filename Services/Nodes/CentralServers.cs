using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace cryptoCurrency.Services.Nodes
{
    public static class CentralServers
    {
        public static List<Uri> Get()
        {
            List<Uri> lstCentralServers = new();
            lstCentralServers.Add(new Uri("http://localhost:10000/nodes"));
            return lstCentralServers;
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