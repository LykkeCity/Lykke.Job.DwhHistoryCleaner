using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Job.DwhHistoryCleaner.Domain.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Lykke.Job.DwhHistoryCleaner.DomainServices
{
    public class AccountHistoryCleaner: IAccountHistoryCleaner
    {
        private readonly TimeSpan _preservePeriod = TimeSpan.FromHours(24);

        private readonly string _rawDwhDataAccountConnStrings;
        private readonly string _convertedDwhDataAccountConnStrings;

        public AccountHistoryCleaner(string rawDwhDataAccountConnStrings, string convertedDwhDataAccountConnStrings)
        {
            _rawDwhDataAccountConnStrings = rawDwhDataAccountConnStrings;
            _convertedDwhDataAccountConnStrings = convertedDwhDataAccountConnStrings;
        }

        public async Task CleanHistoryAsync()
        {
            var now = DateTime.UtcNow;
            await Task.WhenAll(
                new List<Task>
                {
                    ClearAccountHistoricalDataAsync(_rawDwhDataAccountConnStrings, now),
                    ClearAccountHistoricalDataAsync(_convertedDwhDataAccountConnStrings, now),
                });
        }

        private async Task ClearAccountHistoricalDataAsync(string accountConnString, DateTime now)
        {
            var blobClient = CloudStorageAccount.Parse(accountConnString).CreateCloudBlobClient();
            BlobContinuationToken token = null;
            do
            {
                var containersResult = await blobClient.ListContainersSegmentedAsync(token);
                foreach (var container in containersResult.Results)
                {
                    BlobContinuationToken containerToken = null;
                    do
                    {
                        var blobsResult = await container.ListBlobsSegmentedAsync(containerToken);
                        foreach (var blob in blobsResult.Results)
                        {
                            var cloudBlob = blob as CloudBlob;
                            if (cloudBlob == null)
                                continue;
                            if (!cloudBlob.Properties.LastModified.HasValue)
                                await cloudBlob.FetchAttributesAsync();
                            if (now - cloudBlob.Properties.LastModified.Value > _preservePeriod)
                                await cloudBlob.DeleteIfExistsAsync();
                        }
                        containerToken = blobsResult.ContinuationToken;
                    } while (containerToken != null);
                }
                token = containersResult.ContinuationToken;
            } while (token != null);
        }
    }
}
