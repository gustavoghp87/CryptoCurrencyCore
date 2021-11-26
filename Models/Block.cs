using System;
using System.Collections.Generic;

namespace Models
{
    public class Block
    {
        public long Index { get; set; }
        public int Difficulty { get; set; }
        public string DifficultyScore { get; set; }
        public string Hash { get; set; }
        public long Nonce { get; set; }
        public string PreviousHash { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
