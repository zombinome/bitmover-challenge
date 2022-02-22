using BitMover.Challenge.Contracts.Analytics;

namespace BitMover.Challenge.Mocks
{
    public class AnalyticsApiClient : IAnalyticsApiClient
    {
        public Task<long> GetLicenseSessionsAsync(string licenceKey, DateTime from, DateTime to)
        {
            var random = new Random(); 
            
            long sessions = random.Next(0, 1000);

            return Task.FromResult(sessions);
        }
    }
}
