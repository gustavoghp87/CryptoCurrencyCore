using System;

namespace Models
{
    public class TransactionRequest
    {
        public decimal Amount { get; set; }
        public decimal Fees { get; set; }
        public string PrivateKey { get; set; }
        public string Recipient { get; set; }
        public string Sender { get; set; }
        public long Timestamp { get; set; }
    }
}
