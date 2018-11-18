using System;
using Autofac;
using Common;
using Lykke.Job.DwhHistoryCleaner.Domain.Services;
using Lykke.Job.DwhHistoryCleaner.DomainServices;
using Lykke.Job.DwhHistoryCleaner.Services;
using Lykke.Job.DwhHistoryCleaner.Settings.JobSettings;
using Lykke.Sdk;
using Lykke.Sdk.Health;
using Lykke.Job.DwhHistoryCleaner.PeriodicalHandlers;

namespace Lykke.Job.DwhHistoryCleaner.Modules
{
    public class JobModule : Module
    {
        private readonly DwhHistoryCleanerJobSettings _settings;

        public JobModule(DwhHistoryCleanerJobSettings settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterType<PeriodicalHandler>()
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();

            builder.RegisterType<AccountHistoryCleaner>()
                .As<IAccountHistoryCleaner>()
                .SingleInstance()
                .WithParameter("rawDwhDataAccountConnStrings", _settings.Db.RawDwhDataAccountConnString)
                .WithParameter("convertedDwhDataAccountConnStrings", _settings.Db.ConvertedDwhDataAccountConnString)
                .WithParameter("preservedPeriod", _settings.PreservedPeriod ?? TimeSpan.FromHours(24));
        }
    }
}
