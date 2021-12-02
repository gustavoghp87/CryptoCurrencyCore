using Models;
using Newtonsoft.Json;
using Services.Blockchains;
using Services.Interfaces;
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
        private readonly List<Node> _lstNodes;
        public NodeService()
        {
            _lstNodes = new();
            UpdateList();
        }
        public Blockchain GetLongestBlockchain()
        {
            Blockchain blockchain = new();
            if (_lstNodes != null && _lstNodes.Count != 0)
            {
                blockchain = LongestBlockchainOnNet.Get(_lstNodes);
                if (blockchain != null && blockchain.Nodes != null)
                {
                    blockchain.Nodes.ForEach(node => {
                        RegisterOne(node);
                    });
                }
            }
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
                var response = await new HttpClient().PostAsJsonAsync(node.Address + "api/node", httpContent);
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
            if (_lstNodes != null) return false;
            if (!_lstNodes.Contains(node)) _lstNodes.Add(node);
            GetFromOne(node);
            return CheckNew(node);
        }
        public void SendNewBlockchain(Blockchain newBlockchain)
        {
            if (_lstNodes == null) return;
            _lstNodes.ForEach(node =>
            {
                new HttpClient().PostAsJsonAsync(node.Address + "api/blockchain", newBlockchain);
            });
        }

        #region private methods region    ///////////////////////////////////////////////////////////////////////
        private void UpdateList()
        {
            GetFromScaffoldServers();
            _lstNodes.ForEach(node => {
                Console.WriteLine("Connected Node: " + node.Address);
            });
            // TODO: get request alive man
        }
        private void GetFromScaffoldServers()
        {
            List<Node> scaffoldServersList = ScaffoldServers.Get();
            if (scaffoldServersList == null) return;
            scaffoldServersList.ForEach(scaffoldServer => {
                if (!_lstNodes.Contains(scaffoldServer)) _lstNodes.Add(scaffoldServer);
                GetFromOne(scaffoldServer);
            });
        }
        private void GetFromOne(Node node)
        {
            Console.WriteLine("Looking for nodes from " + node.Address.ToString());
            Console.WriteLine("My domain name is " + BlockchainService.DomainName);
            if (node.Address.ToString() == BlockchainService.DomainName)
            {
                Console.WriteLine("This is my domain name......... cancelled");
                return;
            }
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(node.Address + "api/node");
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) return;
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var lstNodes = JsonConvert.DeserializeObject<List<Node>>(json);
                if (lstNodes != null && lstNodes.Count > 0)
                {
                    lstNodes.ForEach(node => {
                        RegisterOne(node);
                    });
                }
            } catch (Exception e) { Console.WriteLine(node.Address + "api/node: " + e.Message); }
        }
        private static bool CheckNew(Node newNode)
        {
            try
            {
                Console.WriteLine(newNode);
                // TODO: post request
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion
    }
}
