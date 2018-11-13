using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.DwhHistoryCleaner.Settings.JobSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string RawDwhDataAccountConnString { get; set; }

        [AzureTableCheck]
        public string ConvertedDwhDataAccountConnString { get; set; }
    }
}
