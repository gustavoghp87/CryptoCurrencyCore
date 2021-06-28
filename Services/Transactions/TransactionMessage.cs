using cryptoCurrency.Models;

namespace cryptoCurrency.Services.Transactions
{
    public static class TransactionMessage
    {
        public static string Generate(Transaction transaction)
        {
            return transaction.Timestamp.ToString() + "-"
                 + transaction.Sender + "-"
                 + transaction.Amount.ToString("0.00000000") + "-"
                 + transaction.Recipient + "-"
                 + transaction.Miner + "-"
                 + transaction.Fees.ToString("0.00000000");
        }
    }
}
