using cryptoCurrency.Models;
using cryptoCurrency.Services.Blockchains;
using cryptoCurrency.Services.Interfaces;
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
        public static Blockchain GetFromNet(List<Node> lstNodes)
        {
            _lstNodes = lstNodes;
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
        private static void GetLargest()
        {
            Blockchain largestBlockchain = new();
            foreach (Blockchain blockchain in _lstBlockchains)
            {
                if (blockchain.Blocks.Count > largestBlockchain.Blocks.Count && ValidateBlockchainService.IsValid(blockchain))
                    largestBlockchain = blockchain;
            }
            if (largestBlockchain != null) _blockchain = largestBlockchain;
        }
    }
}
