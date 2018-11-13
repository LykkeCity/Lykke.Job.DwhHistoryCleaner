using Lykke.Job.DwhHistoryCleaner.Settings.JobSettings;
using Lykke.Sdk.Settings;

namespace Lykke.Job.DwhHistoryCleaner.Settings
{
    public class AppSettings : BaseAppSettings
    {
        public DwhHistoryCleanerJobSettings DwhHistoryCleanerJob { get; set; }
    }
}
