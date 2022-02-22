namespace BitMover.Challenge.Contracts.Analytics
{
    public interface IAnalyticsApiClient
    {
        public Task<long> GetLicenseSessionsAsync(string licenceKey, DateTime from, DateTime to);
    }
}
