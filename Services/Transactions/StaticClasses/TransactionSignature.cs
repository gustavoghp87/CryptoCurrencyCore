using Models;
using Services.Wallets;

namespace Services.Transactions
{
    public static class TransactionSignature
    {
        public static string Sign(Transaction transaction, string privateKey)
        {
            var signature = WalletService.SignMessage(transaction, privateKey);
            return signature;
        }
    }
}
