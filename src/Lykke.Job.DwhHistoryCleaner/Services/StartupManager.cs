using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Sdk;

namespace Lykke.Job.DwhHistoryCleaner.Services
{
    public class StartupManager : IStartupManager
    {
        private readonly ILog _log;

        public StartupManager(ILogFactory logFactory)
        {
            _log = logFactory.CreateLog(this);
        }

        public async Task StartAsync()
        {
            // TODO: Implement your startup logic here. Good idea is to log every step

            await Task.CompletedTask;
        }
    }
}
