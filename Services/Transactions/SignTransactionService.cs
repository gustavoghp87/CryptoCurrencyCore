using Models;
using Services.Interfaces;

namespace Services.Transactions
{
    public class SignTransactionService : ISignTransactionService
    {
        private Transaction _transaction;
        public void Initialize(Transaction transaction, string privateKey)
        {
            _transaction = transaction;
            _transaction.Miner = Miner.MinerWallet.PublicKey;
            Sign(privateKey);
        }
        public string GetMessage()
        {
            return TransactionMessage.Generate(_transaction);
        }
        public string GetSignature()
        {
            return _transaction.Signature;
        }
        private void Sign(string privateKey)
        {
            _transaction.Signature = WalletService.SignMessage(_transaction, privateKey);
        }
    }
}
