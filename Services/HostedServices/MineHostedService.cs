using Microsoft.Extensions.Hosting;
using Services.Interfaces;
using System;
using System.Threading.Tasks;
using ST = System.Threading;

namespace Services.Blockchains
{
    public class MineHostedService : IHostedService, IDisposable
    {
        private readonly int _number;
        private ST.Timer _timer;
        private int counter = 0;
        private readonly IBlockchainService _blockchainService;
        public MineHostedService(IBlockchainService blockchainService)
        {
            _number = 1000 * 100;    // milliseconds
            _blockchainService = blockchainService;
        }
        public Task StartAsync(ST.CancellationToken cancellationToken)
        {
            Console.WriteLine("Initializing Mine Hosted Service");
            long unixTime = TimeService.GetCurrentUnixTime();
            Console.WriteLine("Now: " + unixTime);
            int timeLeft = (int)(_number - unixTime%_number);
            Console.WriteLine("Waiting (" + timeLeft + " + " + _number + ") milliseconds to start mining");
            _timer = new ST.Timer(
                InitMiningCycle,
                null,
                TimeSpan.FromMilliseconds(timeLeft),
                TimeSpan.FromMilliseconds(_number));
            return Task.CompletedTask;
        }
        public Task StopAsync(ST.CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void InitMiningCycle(object o)
        {
            counter++;
            if (counter > 1)
            {
                long unixTime = TimeService.GetCurrentUnixTime();
                Console.WriteLine(unixTime + ": Mining " + (counter - 1));
                _blockchainService.Mine();
            }
        }
    }
}