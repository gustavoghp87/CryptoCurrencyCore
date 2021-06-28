using cryptoCurrency.Models;
using cryptoCurrency.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace cryptoCurrency.Services.Nodes
{
    public class NodeService : INodeService
    {
        private List<Node> _lstNodes;
        public NodeService()
        {
            _lstNodes = new();
            UpdateList();
        }
        private void UpdateList()
        {
            GetFromBaseServers();
            _lstNodes.ForEach(node => {
                Console.WriteLine("Connected Nodes: " + node.Address);
            });
            // TODO: get request alive man
        }
        private void GetFromBaseServers()
        {
            List<Uri> lstCentralServers = ScaffoldServers.Get();
            foreach (var centralServer in lstCentralServers)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(centralServer + "/node");
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode != HttpStatusCode.OK) continue;
                    Console.WriteLine(response);
                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var lstNodes = JsonConvert.DeserializeObject<List<Node>>(json);
                    if (lstNodes == null) continue;
                    lstNodes.ForEach(node => {
                        if (!_lstNodes.Contains(node)) _lstNodes.Add(node);
                    });
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            };
        }
        private void GetFromBlockchain(List<Node> lstNodes)
        {
            if (lstNodes == null) return;
            lstNodes.ForEach(node => {
                if (!_lstNodes.Contains(node)) _lstNodes.Add(node);
            });
        }
        public Blockchain GetLongestBlockchain()
        {
            Blockchain blockchain = new();
            if (_lstNodes != null && _lstNodes.Count != 0)
                blockchain = LongestBlockchainOnNet.Get(_lstNodes);      // update _lstNodes ??
            GetFromBlockchain(blockchain.Nodes);
            // compare this one with mine's
            return blockchain;
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
                var httpContent = new StringContent("", Encoding.UTF8, "application/json");
                var response = await new HttpClient().PostAsJsonAsync(node.Address + "node/registry", httpContent);
                if (response.IsSuccessStatusCode) counter++;
            }
            Console.WriteLine("Registered in " + counter + " servers");
            
            // var httpClient = new HttpClient();
            // var stringPayload = JsonConvert.SerializeObject(new { Ip = "http://localhost:5000" });
            // var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            // var httpResponse = await httpClient.PostAsync("http://localhost:10000/registry", httpContent);
            // if (!httpResponse.IsSuccessStatusCode) return;
            
            Console.WriteLine(counter);
        }
        public bool RegisterOne(Node node)
        {
            _lstNodes.Add(node);
            return CheckNew(node);
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
            if (_lstNodes == null) return;
            _lstNodes.ForEach(node =>
            {
                new HttpClient().PostAsJsonAsync(node.ToString() + "/new-blockchain", newBlockchain);
            });
        }
    }
}
