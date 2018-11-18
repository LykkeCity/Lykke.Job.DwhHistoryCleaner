using System;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.DwhHistoryCleaner.Settings.JobSettings
{
    public class DwhHistoryCleanerJobSettings
    {
        public DbSettings Db { get; set; }

        [Optional]
        public TimeSpan? PreservedPeriod { get; set; }
    }
}
