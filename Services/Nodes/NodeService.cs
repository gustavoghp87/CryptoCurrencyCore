using cryptoCurrency.Models;
using cryptoCurrency.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace cryptoCurrency.Services.Nodes
{
    public class NodeService : INodeService
    {
        private List<Node> _lstNodes;
        private Uri _centralServerUrl { get; set; }
        public NodeService()
        {
            _lstNodes = new();
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
                        var newNode = new Node
                        {
                            Address = new Uri(address.EndsWith('/') ? address.Substring(0, -1) : address)
                        };
                        if (!_lstNodes.Contains(newNode)) _lstNodes.Add(newNode);
                    }
                    foreach (var item in _lstNodes)
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
        public List<Node> GetAll()
        {
            return _lstNodes;
        }


        public void RegisterMe()
        {
            RegisterMeService.Send(_lstNodes);
        }
        public bool RegisterOne(string address)
        {
            var newNode = new Node
            {
                Address = new Uri(address.EndsWith('/') ? address.Substring(0, -1) : address)
            };
            _lstNodes.Add(newNode);
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
        public void SendNewBlockchain(Blockchain newBlockchain)
        {
            if (_lstNodes == null)
                foreach (Node node in _lstNodes)
                {
                    new HttpClient().PostAsJsonAsync(node.ToString() + "/new-blockchain", newBlockchain);
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
            foreach (Node node in _lstNodes)
            {
                _lstNodes.Clear();
                // get request alive man
                if (true) _lstNodes.Add(node);
            }
        }

        public Blockchain GetLongestBlockchain()
        {
            Blockchain blockchain = new();
            List<Blockchain> lstBlockchains = new();
            if (_lstNodes != null && _lstNodes.Count != 0) LongestBlockchainOnNet.GetFromNet(_lstNodes);      // update _lstNodes ??
            return blockchain;
        }
    }
}
