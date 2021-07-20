using Models;
using System;
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
            _block.Timestamp = DateTime.UtcNow;
            _block.Nonce = 0;
            _block.Hash = "";
            Mine();
        }
        private void Mine()
        {
            ProofOfWorkService proofServ = new(_block);
            _block.Nonce = proofServ.GetNonce();
            _block.Hash = proofServ.GetHash();
            //ValidateBlockService.GetBestHash(_block);        // not connected
        }
        public Block GetMined()
        {
            return _block;
        }
    }
}
