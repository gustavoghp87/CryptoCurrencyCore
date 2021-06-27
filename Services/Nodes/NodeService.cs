using cryptoCurrency.Models;
using cryptoCurrency.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace cryptoCurrency.Services.Nodes
{
    public class NodeService : INodeService
    {
        private List<Node> _lstNodes;
        public NodeService()
        {
            _lstNodes = new();
            GetFromCentralServers();
            GetFromLongestBlockchain();
            _lstNodes.ForEach(node => {
                Console.WriteLine("Connected Nodes: " + node.Address);
            });
        }
        private void GetFromCentralServers()
        {
            List<Uri> lstCentralServers = CentralServers.Get();
            foreach (var centralServer in lstCentralServers)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(centralServer);
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode != HttpStatusCode.OK) continue;
                    Console.WriteLine(response);
                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var lstNodes = JsonConvert.DeserializeObject<List<Node>>(json);
                    lstNodes.ForEach(node => {
                        if (!_lstNodes.Contains(node)) _lstNodes.Add(node);
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            };
        }
        private void GetFromLongestBlockchain()
        {

        }
        public List<Node> GetAll()
        {
            return _lstNodes;
        }


        public async Task RegisterMe()
        {
            int counter = 0;
            if (_lstNodes == null || _lstNodes.Count == 0) return;
            foreach (Node node in _lstNodes)
            {
                var response = await new HttpClient().GetAsync(node.Address + "node/registry");
                if (response.IsSuccessStatusCode) counter++;
            }
            Console.WriteLine(counter);
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
