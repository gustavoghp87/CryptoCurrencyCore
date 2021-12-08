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

namespace Services.Nodes
{
    public class NodeService : INodeService
    {
        private readonly List<Node> _lstNodes;
        public NodeService()
        {
            _lstNodes = new();
            _lstNodes.Add(new Node() { Address = new Uri (BlockchainService.DomainName) });
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
        public void RegisterMe()
        {
            if (_lstNodes == null || _lstNodes.Count == 0) return;
            var httpContent = new StringContent("", Encoding.UTF8, "application/json");
            foreach (Node node in _lstNodes)
            {
                new HttpClient().PostAsJsonAsync(node.Address + "api/node", httpContent);
            }
            
            // var httpClient = new HttpClient();
            // var stringPayload = JsonConvert.SerializeObject(new { Ip = "http://localhost:5000" });
            // var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            // var httpResponse = await httpClient.PostAsync("http://localhost:10000/registry", httpContent);
            // if (!httpResponse.IsSuccessStatusCode) return;
            
        }
        public bool RegisterOne(Node node)
        {
            if (_lstNodes != null) return false;
            if (!_lstNodes.Contains(node)) _lstNodes.Add(node);
            // GetFromOne(node);    loop
            return CheckNew(node);
        }
        public void SendNewBlockchain(Blockchain newBlockchain)
        {
            if (_lstNodes == null) return;
            foreach (Node node in _lstNodes)
            {
                if (node.Address.ToString() == BlockchainService.DomainName) continue;
                string url = node.Address + "api/blockchain";
                Console.WriteLine("Sending new blockchain to " + url);
                new HttpClient().PostAsJsonAsync(url, newBlockchain);
            }
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
            Console.WriteLine("\nLooking for nodes from " + node.Address.ToString());
            if (node.Address.ToString() == BlockchainService.DomainName || node.Address.ToString() == BlockchainService.DomainName + "/")
            {
                Console.WriteLine("This is my domain name......... cancelled\n");
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
                    int i = 0;
                    lstNodes.ForEach(node => {
                        i++;
                        Console.WriteLine("Coming node " + i + " of " + lstNodes.Count + ": " + node.Address.ToString());
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
