using System;
using System.Net.Http;
using ST = System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Services.Blockchains
{
    public class MineHostedService : IHostedService, IDisposable
    {
        private ST.Timer _timer;
        public Task StartAsync(ST.CancellationToken cancellationToken)
        {
            long unixTimeSeconds = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            //Console.WriteLine(unixTimeSeconds);
            int timeLeft = 60 - (int)unixTimeSeconds%60;
            //Console.WriteLine("Waiting (" + timeLeft + " + 60) seconds to start mining");
            //Console.WriteLine("60 seconds left");
            _timer = new ST.Timer(InitMiningCycle, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }
        private void InitMiningCycle(object o)
        {
            long unixTimeSeconds = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            //Console.WriteLine("Sending best proof of work to network..." + unixTimeSeconds);
            //Console.WriteLine("Sending post request to Controller to begin mining...");
            var httpResponse = new HttpClient().GetAsync("https://localhost:5001/blockchain/mine");
            // TODO: some kind of verification
        }
        public Task StopAsync(ST.CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}