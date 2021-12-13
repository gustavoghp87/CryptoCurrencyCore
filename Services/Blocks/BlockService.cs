using Models;
using System.Collections.Generic;

namespace Services.Blocks
{
    public class BlockService
    {
        private Block _block;
        public BlockService(long index, string previousHash, List<Transaction> lstTransactions, int difficulty)
        {
            _block = new();
            _block.Index = index;
            _block.Difficulty = new();
            _block.Difficulty = difficulty;
            _block.PreviousHash = previousHash;
            _block.Transactions = new();
            _block.Transactions.AddRange(lstTransactions);
            _block.Timestamp = TimeService.GetCurrentUnixTime();
            _block.Date = TimeService.GetDateFromUnitTime(_block.Timestamp);
            _block.Nonce = 0;
            _block.Hash = "";
            Mine();
        }
        public Block GetMined()
        {
            return _block;
        }
        
        private void Mine()
        {
            _block = ProofOfWorkService.Create(_block);
            //ValidateBlockService.GetBestHash(_block);        // not connected
        }
    }
}
