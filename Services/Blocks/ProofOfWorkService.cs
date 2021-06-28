using cryptoCurrency.Models;

namespace cryptoCurrency.Services.Blocks
{
    public class ProofOfWorkService
    {
        private long _nonce;
        private string _hash;
        public ProofOfWorkService(Block block)
        {
            // TODO: validations
            _nonce = new long();
            _hash = "";
            Create(block);
        }
        private void Create(Block block)
        {
            while (!ValidateBlock.IsValid(block))
                block.Nonce++;
            _nonce = block.Nonce;
            _hash = ValidateBlock.GetHash(block);
        }
        public long GetNonce()
        {
            return _nonce;
        }
        public string GetHash()
        {
            return _hash;
        }
    }
}
