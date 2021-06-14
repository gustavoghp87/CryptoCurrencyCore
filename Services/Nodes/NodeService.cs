using BlockchainAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace BlockchainAPI.Services.Nodes
{
    public class NodeService
    {
        private Node[] _nodes;
        private Uri _centralServerUrl { get; set; }
        public NodeService()
        {
            _centralServerUrl = CentralServerConnection.GetUri();
            GetAllFromServer();
        }
        private void GetAllFromServer()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_centralServerUrl);
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(response);
                    //var model = new { nodesRetrieved = new List<Node>(), length = 0 };
                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var data = JsonConvert.DeserializeObject<List<string>>(json);
                    
                    foreach (var address in data)
                    {
                        var newNode = new Node {
                            Address = new Uri(address.EndsWith('/') ? address.Substring(0, -1) : address)
                        };
                        List<Node> lstNodes = _nodes.ToList();
                        if (!lstNodes.Contains(newNode)) lstNodes.Add(newNode);
                        _nodes = lstNodes.ToArray();
                    }
                    foreach (var item in _nodes)
                    {
                        Console.WriteLine("Connected Nodes: " + item.Address);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public Node[] GetAll()
        {
            return _nodes;
        }


        public void RegisterMe()
        {
            RegisterMeService.Send(_nodes);
        }
        public bool RegisterOne(string address)
        {
            var newNode = new Node
            {
                Address = new Uri(address.EndsWith('/') ? address.Substring(0, -1) : address)
            };
            List<Node> lst = _nodes.ToList();
            lst.Add(newNode);
            _nodes = lst.ToArray();
            return CheckNew(newNode);
        }
        private bool CheckNew(Node newNode)
        {
            try
            {
                Console.WriteLine(newNode);
                // post request
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        //public string RegisterMany(string[] nodes)
        //{
        //    var builder = new StringBuilder();
        //    foreach (string url in nodes)
        //    {
        //        RegisterOne(url);
        //        builder.Append($"{url}, ");
        //    }
        //    builder.Insert(0, $"{nodes.Length} new nodes have been added: ");
        //    string result = builder.ToString();
        //    return result.Substring(0, result.Length - 2);
        //}

        public void UpdateList()
        {
            foreach (Node node in _nodes)
            {
                _nodes = Array.Empty<Node>();
                // get request alive man
                if (true)
                {
                    List<Node> lst = _nodes.ToList();
                    lst.Add(node);
                    _nodes = lst.ToArray();
                }
            }
        }
    }
}
