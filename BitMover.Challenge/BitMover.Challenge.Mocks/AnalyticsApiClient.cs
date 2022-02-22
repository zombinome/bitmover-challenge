using BitMover.Challenge.Contracts.Analytics;

namespace BitMover.Challenge.Mocks
{
    public class AnalyticsApiClient : IAnalyticsApiClient
    {
        public Task<long> GetLicenseSessionsAsync(string licenceKey, DateTime from, DateTime to)
        {
            var random = new Random((int)DateTime.Now.Ticks); 
            
            long sessions = random.Next(0, 1000);

            var delay = random.Next(100, 15000);
            Thread.Sleep(delay);

            return Task.FromResult(sessions);
        }
    }
}
