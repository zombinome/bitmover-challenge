using BitMover.Challenge.Contracts.Analytics;
using BitMover.Challenge.Contracts.Billing;
using BitMover.Challenge.Contracts.Storage;

namespace BitMover.Challenge.Service
{
    public class BillingUpdateService
    {
        private readonly ILicenseRepository licenseRepository;
        private readonly IBillingMessageSender billingMessageSender;
        private readonly IAnalyticsApiClient analyticsApi;
        private const int MaxAttemptsCount = 10;

        private const int batchSize = 10;

        public BillingUpdateService(
            ILicenseRepository licenseRepository,
            IBillingMessageSender billingMessageSender,
            IAnalyticsApiClient analyticsApi)
        {
            this.licenseRepository = licenseRepository;
            this.billingMessageSender = billingMessageSender;
            this.analyticsApi = analyticsApi;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var timestamp = DateTime.UtcNow;
            int offset = 0;

            // TODO: Add proper exception handling
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var licensesToUpdate = await this.licenseRepository.ListLicensesAsync(timestamp, offset, batchSize);
                if (licensesToUpdate.Count == 0)
                {
                    break;
                }

                foreach(var license in licensesToUpdate)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    await UpdateBillingAsync(license, timestamp);
                }

                offset += batchSize;
            }
        }

        private async Task UpdateBillingAsync(License license, DateTime timestamp)
        {
            long sessionCount = 0;
            try
            {
                sessionCount = await this.analyticsApi.GetLicenseSessionsAsync(license.Key.ToString(), license.LastUpdated, timestamp);
            }
            catch
            {
                // TODO: Add propert network failure cases handling
                license.AttemptsCount++;
                if (license.AttemptsCount > MaxAttemptsCount)
                {
                    await this.licenseRepository.MarkLicenseDeadAsync(license.Key);
                }
                else
                {
                    await this.licenseRepository.IncrementFailureCountAsync(license.Key);
                }
                return;
            }
            await this.licenseRepository.UpdateLicenceTimestampAsync(license.Key, timestamp);
            if (sessionCount > 0)
            {
                var message = new BillingMessage
                {
                    LicenceKey = license.Key,
                    From = license.LastUpdated,
                    To = timestamp,
                    Product = "analytics",
                    Usage = sessionCount
                };
                await this.billingMessageSender.SendMessageAsync(message);
            }
        }
    }
}