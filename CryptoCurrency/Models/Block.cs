using System;
using System.Collections.Generic;

namespace CryptoCurrency.Models
{
    public class Block
    {
        public long Index { get; set; }
        public int Difficulty { get; set; }
        public string DifficultyT { get; set; }
        public string Hash { get; set; }
        public long Nonce { get; set; }
        public string PreviousHash { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
