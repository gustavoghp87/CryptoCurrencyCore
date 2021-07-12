using CryptoCurrency.Models;

namespace CryptoCurrency.Services.Transactions
{
    public class SignTransactionService
    {
        private Transaction _transaction;
        public SignTransactionService(Transaction transaction, string privateKey)
        {
            _transaction = transaction;
            _transaction.Miner = Miner.Get().PublicKey;
            Sign(privateKey);
        }
        private void Sign(string privateKey)
        {
            _transaction.Signature = WalletService.SignMessage(_transaction, privateKey);
        }
        public string GetMessage()
        {
            return TransactionMessage.Generate(_transaction);
        }
        public string GetSignature()
        {
            return _transaction.Signature;
        }
    }
}
