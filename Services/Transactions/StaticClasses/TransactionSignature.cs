using NBitcoin;
using System;
using Transaction = Models.Transaction;

namespace Services.Transactions
{
    public static class TransactionSignature
    {
        public static string Get(Transaction transaction, string privateKey)
        {
            try
            {
                BitcoinSecret secret = Network.Main.CreateBitcoinSecret(privateKey);
                string message = TransactionMessage.Generate(transaction);
                string signature = secret.PrivateKey.SignMessage(message);
                return signature;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static bool IsVerifiedMessage(Transaction transaction)
        {
            string message = TransactionMessage.Generate(transaction);
            IPubkeyHashUsable address = (IPubkeyHashUsable)BitcoinAddress.Create(transaction.Sender, Network.Main);
            bool result = address.VerifyMessage(message, transaction.Signature);
            if (!result) Console.WriteLine("Refused transaction");
            return result;
        }
    }
}
