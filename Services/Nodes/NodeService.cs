using Models;
using Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services.Nodes
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
            lstCentralServers.ForEach(centralServer => {
                GetFromOne(centralServer);
            });
        }
        private void GetFromOne(Uri nodeAddress)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(nodeAddress + "/api/node");
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) return;
                Console.WriteLine(response);
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var lstNodes = JsonConvert.DeserializeObject<List<Node>>(json);
                AddMany(lstNodes);
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }
        private void AddMany(List<Node> lstNodes)
        {
            if (lstNodes == null) return;
            lstNodes.ForEach(node => {
                if (!_lstNodes.Contains(node)) _lstNodes.Add(node);
            });
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
        public Blockchain GetLongestBlockchain()
        {
            Blockchain blockchain = new();
            if (_lstNodes != null && _lstNodes.Count != 0)
            {
                blockchain = LongestBlockchainOnNet.Get(_lstNodes);
                if (blockchain != null && blockchain.Nodes != null) AddMany(blockchain.Nodes);
            }
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
                var response = await new HttpClient().PostAsJsonAsync(node.Address + "/api/node", httpContent);
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
            bool exists = false;
            _lstNodes.ForEach(nodeX => { if (nodeX.Address == node.Address) exists = true; } );
            if (!exists)
            {
                _lstNodes.Add(node);
                GetFromOne(node.Address);
            }
            return CheckNew(node);
        }
        public void SendNewBlockchain(Blockchain newBlockchain)
        {
            if (_lstNodes == null) return;
            _lstNodes.ForEach(node =>
            {
                new HttpClient().PostAsJsonAsync(node.Address.ToString() + "/api/blockchain", newBlockchain);
            });
        }
    }
}
