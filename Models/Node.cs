using System;

namespace Models
{
    public class Node
    {
        public Uri Address { get; set; }
        public Uri BlockchainRequestAddress { get => new(Address.ToString() + "api/blockchain"); }
        public Uri NodeRequestAddress { get => new(Address.ToString() + "api/node"); }
        public Uri TransactionRequestAddress { get => new(Address.ToString() + "api/transaction"); }
    }
}
