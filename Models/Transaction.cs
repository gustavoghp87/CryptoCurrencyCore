using System;

namespace Models
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public decimal Fees { get; set; }
        public string Miner { get; set; }
        public string Recipient { get; set; }
        public string Sender { get; set; }
        public string Signature { get; set; }
        public long Timestamp { get; set; }
    }
}
