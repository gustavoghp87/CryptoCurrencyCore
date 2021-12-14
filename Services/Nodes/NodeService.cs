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
            var stringPayload = JsonConvert.SerializeObject(new { Ip = BlockchainService.DomainName });
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            foreach (Node node in _lstNodes)
            {
                new HttpClient().PostAsJsonAsync(node.NodeRequestAddress, httpContent);
            }
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
                Uri url = node.BlockchainRequestAddress;
                Console.WriteLine("Sending new blockchain to " + url);
                new HttpClient().PostAsJsonAsync(url, newBlockchain);
            }
        }
        public void SendNewTransaction(Transaction transaction)
        {
            if (_lstNodes == null) return;
            foreach (Node node in _lstNodes)
            {
                if (node.Address.ToString() == BlockchainService.DomainName) continue;
                Uri url = node.TransactionRequestAddress;
                Console.WriteLine("Sending new transaction to " + url);
                new HttpClient().PostAsJsonAsync(url, transaction);
            }
        }


        private void UpdateList()
        {
            GetFromScaffoldServers();
            _lstNodes.ForEach(node => {
                Console.WriteLine("Connected Node: " + node.Address);
            });
            // TODO: alive man request
        }
        private void GetFromScaffoldServers()
        {
            List<Node> scaffoldServersList = ScaffoldServers.Get();
            if (scaffoldServersList == null) return;
            scaffoldServersList.ForEach(scaffoldServer => {
                RegisterOne(scaffoldServer);
                GetFromOne(scaffoldServer);
            });
        }
        private void GetFromOne(Node node)
        {
            Console.WriteLine("\nLooking for nodes from " + node.Address.ToString());
            if (node.Address.ToString() == BlockchainService.DomainName
                || node.Address.ToString() == BlockchainService.DomainName + "/")
            {
                Console.WriteLine("This is my domain name......... cancelled\n");
                return;
            }
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(node.NodeRequestAddress);
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
            } catch (Exception e) { Console.WriteLine(node.NodeRequestAddress + ": " + e.Message); }
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
    }
}
