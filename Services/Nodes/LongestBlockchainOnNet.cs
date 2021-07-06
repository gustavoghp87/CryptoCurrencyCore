using cryptoCurrency.Models;
using cryptoCurrency.Services.Blockchains;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace cryptoCurrency.Services.Nodes
{
    public static class LongestBlockchainOnNet
    {
        private static Blockchain _blockchain;
        private static List<Blockchain> _lstBlockchains;
        private static List<Node> _lstNodes;
        public static Blockchain Get(List<Node> lstNodes)
        {
            _lstNodes = lstNodes;
            _blockchain = new();
            _lstBlockchains = new();
            GetAllFromNet();
            if (_lstBlockchains == null || _lstBlockchains.Count == 0) return null;
            GetLargest();
            return _blockchain;
        }
        private static void GetAllFromNet()
        {
            if (_lstNodes.Count == 0) Console.WriteLine("There is no node");
            else
            foreach (Node node in _lstNodes)
            {
                try
                {
                    var url = new Uri(node.Address + "/api/blockchain");
                    Console.WriteLine(url.ToString());
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode != HttpStatusCode.OK) continue;
                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    Blockchain blockchain = JsonConvert.DeserializeObject<Blockchain>(json);
                    _lstBlockchains.Add(blockchain);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }
        private static void GetLargest()
        {
            Blockchain largestBlockchain = new();
            foreach (Blockchain blockchain in _lstBlockchains)
            {
                if (blockchain.Blocks.Count > largestBlockchain.Blocks.Count && ValidateBlockchain.IsValid(blockchain))
                    largestBlockchain = blockchain;
            }
            if (largestBlockchain != null) _blockchain = largestBlockchain;
        }
    }
}
