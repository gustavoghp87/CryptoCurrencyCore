using System.Collections.Generic;

namespace CryptoCurrency.Models
{
    public class Blockchain
    {
        public List<Node> BlackList { get; set; }
        public List<Block> Blocks { get; set; }
        public Wallet IssuerWallet { get; set; }
        public string LastDifficulty {get;set;}
        public decimal LastReward { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
