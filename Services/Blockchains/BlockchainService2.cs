using cryptoCurrency.Models;
using cryptoCurrency.Services.Interfaces;

namespace cryptoCurrency.Services.Blockchains
{
    public partial class BlockchainService : IBlockchainService
    {
        private void SendToNodes()
        {
            _nodeService.SendNewBlockchain(_blockchain);
        }
        public Blockchain Get()
        {
            return _blockchain;
        }
    }
}
