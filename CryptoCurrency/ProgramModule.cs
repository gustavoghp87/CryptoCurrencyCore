using Autofac;
using Services.Blockchains;
using Services.Interfaces;
using Services.Nodes;
using Services.Transactions;

namespace CryptoCurrency
{
    internal class ProgramModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlockchainService>()
                .As<IBlockchainService>()
                .SingleInstance();
            builder.RegisterType<NodeService>()
                .As<INodeService>()
                .SingleInstance();
            builder.RegisterType<TransactionService>()
                .As<ITransactionService>()
                .SingleInstance();
        }
    }
}
