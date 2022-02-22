using BitMover.Challenge.Contracts.Analytics;

namespace BitMover.Challenge.Mocks
{
    public class AnalyticsApiClient : IAnalyticsApiClient
    {
        private const int RowsInResponse = 5;

        public Task<LicenseSessionsResponse> GetLicenseSessionsAsync(string licenceKey, DateTime from, DateTime to)
        {
            var random = new Random();
            var rows = new List<long[]>();
            for (var i = 0; i < RowsInResponse; i++)
            {
                long timestamp = DateTime.UtcNow.Ticks; // There should be UNIX timestamp, but I don't plan to use it, so don't care
                long sessions = random.Next(0, 1000);
                rows.Add(new long[] { timestamp, sessions });
            }
            var result = new LicenseSessionsResponse()
            {
                RowCount = rows.Count,
                Rows = rows,
            };

            return Task.FromResult(result);
        }
    }
}
