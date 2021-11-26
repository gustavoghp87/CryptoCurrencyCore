using Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Services.Nodes
{
    public static class ScaffoldServers
    {
        public static List<Node> Get()
        {
            List<Node> lstInitialServers = new();
                Node node1 = new() { Address = new Uri("https://localhost:5001") };
                Node node2 = new() { Address = new Uri("https://localhost:5002") };
                Node node3 = new() { Address = new Uri("https://localhost:5003") };
                lstInitialServers.Add(node1);
                lstInitialServers.Add(node2);
                lstInitialServers.Add(node3);
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