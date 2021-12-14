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



        private static void GetAllFromNet()
        {
            if (_lstNodes.Count == 0)
            {
                Console.WriteLine("There is no node");
                return;
            }

            foreach (Node node in _lstNodes)
            {
                if (node.Address.ToString() == BlockchainService.DomainName) continue;
                try
                {
                    var url = node.BlockchainRequestAddress;
                    Console.WriteLine("Requiring blockchain from " + url.ToString());
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode != HttpStatusCode.OK) continue;
                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    Blockchain blockchain = JsonConvert.DeserializeObject<Blockchain>(json);
                    if (blockchain != null && BlockchainValidation.IsValid(blockchain))
                        _lstBlockchains.Add(blockchain);
                    else
                        Console.WriteLine(); // add to blacklist
                }
                catch (Exception e) { Console.WriteLine(node.BlockchainRequestAddress + ": " + e.Message); }
            }
        }
        private static void GetLargest()   // or best ?
        {
            foreach (Blockchain blockchain in _lstBlockchains)
            {
                if (blockchain.Blocks == null || blockchain.Blocks.Count == 0) continue;
                if (_largestBlockchain == null || _largestBlockchain.Blocks == null)
                {
                    _largestBlockchain = blockchain;
                }

                if (blockchain.Blocks.Count < _largestBlockchain.Blocks.Count)
                {
                    Console.WriteLine("Blockchain refused; shorter");
                    return;
                }

                foreach (Block block in blockchain.Blocks)
                {
                    foreach (Block blockL in _largestBlockchain.Blocks)
                    {
                        if (block.Index != blockL.Index) continue;
                        if (block.DifficultyScoreNumber < blockL.DifficultyScoreNumber)
                        {
                            Console.WriteLine("Blockchain refused");
                            return;
                        }
                    }
                }
                _largestBlockchain = blockchain;
            }
            _lstBlockchains.Clear();
        }
    }
}
