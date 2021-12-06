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
            return new List<Node>() {
                new Node() { Address = new Uri("https://mysterious-thicket-34741.herokuapp.com/") },
                new Node() { Address = new Uri("https://limitless-sands-00250.herokuapp.com/") },
                new Node() { Address = new Uri("http://190.231.194.136/") },
                new Node() { Address = new Uri("http://190.231.194.136:8081/") }
            };
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
