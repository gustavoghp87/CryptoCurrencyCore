using Models;

namespace Services.Blocks
{
    public static class ProofOfWorkService
    {
        public static Block Create(Block block)
        {
            // TODO: validations

            block.Nonce = new long();

            while (!ValidateBlock.IsValid(block))
                block.Nonce++;
            
            block.Hash = ValidateBlock.GetHash(block);
            return block;
        }
    }
}
