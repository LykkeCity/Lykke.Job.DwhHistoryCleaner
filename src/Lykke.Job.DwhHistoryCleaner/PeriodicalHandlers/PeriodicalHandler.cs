using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common;
using Lykke.Common.Log;
using Lykke.Job.DwhHistoryCleaner.Domain.Services;

namespace Lykke.Job.DwhHistoryCleaner.PeriodicalHandlers
{
    public class PeriodicalHandler : IStartable, IStopable
    {
        private readonly TimerTrigger _timerTrigger;
        private readonly IAccountHistoryCleaner _accountHistoryCleaner;

        public PeriodicalHandler(ILogFactory logFactory, IAccountHistoryCleaner accountHistoryCleaner)
        {
            _timerTrigger = new TimerTrigger(nameof(PeriodicalHandler), TimeSpan.FromHours(6), logFactory);
            _timerTrigger.Triggered += Execute;

            _accountHistoryCleaner = accountHistoryCleaner;
        }

        public void Start()
        {
            _timerTrigger.Start();
        }

        public void Stop()
        {
            _timerTrigger.Stop();
        }

        public void Dispose()
        {
            _timerTrigger.Stop();
            _timerTrigger.Dispose();
        }

        private async Task Execute(ITimerTrigger timer, TimerTriggeredHandlerArgs args, CancellationToken cancellationToken)
        {
            await _accountHistoryCleaner.CleanHistoryAsync();
        }
    }
}
