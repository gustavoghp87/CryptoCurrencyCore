using Models;
using Services.Blockchains;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Services.Nodes
{
    public static class LongestBlockchainOnNet
    {
        private static Blockchain _largestBlockchain;
        private static List<Blockchain> _lstBlockchains;
        private static List<Node> _lstNodes;
        public static Blockchain Get(List<Node> lstNodes)
        {
            _lstNodes = lstNodes;
            _largestBlockchain = new();
            _lstBlockchains = new();
            GetAllFromNet();
            if (_lstBlockchains == null || _lstBlockchains.Count == 0) return null;
            GetLargest();
            return _largestBlockchain;
        }

        #region private methods region    ///////////////////////////////////////////////////////////////////////
        private static void GetAllFromNet()
        {
            if (_lstNodes.Count == 0) Console.WriteLine("There is no node");
            else
                foreach (Node node in _lstNodes)
                {
                    if (node.Address.ToString() == BlockchainService.DomainName)
                    {
                        Console.WriteLine("EQUALS                                  *************************************** ");
                        continue;
                    }
                    try
                    {
                        var url = new Uri(node.Address + "api/blockchain");
                        Console.WriteLine("Requiring blockchain from " + url.ToString());
                        var request = (HttpWebRequest)WebRequest.Create(url);
                        var response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode != HttpStatusCode.OK) continue;
                        string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        Blockchain blockchain = JsonConvert.DeserializeObject<Blockchain>(json);
                        _lstBlockchains.Add(blockchain);
                    }
                    catch (Exception e) { Console.WriteLine(node.Address + "api/blockchain: " + e.Message); }
                }
        }
        private static void GetLargest()
        {
            foreach (Blockchain blockchain in _lstBlockchains)
            {
                if (blockchain.Blocks == null || blockchain.Blocks.Count == 0) continue;
                if (_largestBlockchain != null && _largestBlockchain.Blocks != null)
                {
                    if (blockchain.Blocks.Count > _largestBlockchain.Blocks.Count && BlockchainValidation.IsValid(blockchain))
                        _largestBlockchain = blockchain;
                    else
                        Console.WriteLine("Blockchain refused");
                }
                else if (BlockchainValidation.IsValid(blockchain))
                {
                    _largestBlockchain = blockchain;
                }
            }
        }
        #endregion
    }
}
