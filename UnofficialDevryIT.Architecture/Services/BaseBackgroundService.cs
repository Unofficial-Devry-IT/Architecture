using System.Threading;
using System.Threading.Tasks;

namespace UnofficialDevryIT.Architecture.Services
{
    public abstract class BaseBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        protected abstract Task LongRunningTask();
        protected abstract Task InitializeService();
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeService();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await LongRunningTask();
            }
        }
    }
}