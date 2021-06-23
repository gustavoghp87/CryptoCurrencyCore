using BlockchainAPI.Models;
using BlockchainAPI.Services.Nodes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BlockchainAPI.Services.Blockchains
{
    public class LargestBlockchainService
    {
        private Blockchain _blockchain;
        private List<Blockchain> _lstBlockchains;
        private Node[] _nodes;
        public LargestBlockchainService()
        {
            _blockchain = new();
            _lstBlockchains = new();
            _nodes = new NodeService().GetAll();
            if (_nodes != null && _nodes.Length != 0) GetFromNet();
        }
        private void GetFromNet()
        {
            GetAllFromNet();
            if (_lstBlockchains == null || _lstBlockchains.Count == 0) return;
            GetLargest();
        }
        private void GetAllFromNet()
        {
            if (_nodes.Length == 0) Console.WriteLine("There is no node");
            else
            foreach (Node node in _nodes)
            {
                try
                {
                    var url = new Uri(node.Address + "chain");
                    Console.WriteLine(url.ToString());
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var model = new
                        {
                            blockchain = new Blockchain(),
                            length = 0
                        };
                        string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        var data = JsonConvert.DeserializeAnonymousType(json, model);
                        _lstBlockchains.Add(data.blockchain);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void GetLargest()
        {
            Blockchain largestBlockchain = new();
            foreach (Blockchain blockchain in _lstBlockchains)
            {
                if (blockchain.Blocks.Count > largestBlockchain.Blocks.Count && ValidateBlockchainService.IsValid(blockchain))
                    largestBlockchain = blockchain;
            }
            if (largestBlockchain != null) _blockchain = largestBlockchain;
        }
        public Blockchain Get()
        {
            return _blockchain;
        }
    }
}
