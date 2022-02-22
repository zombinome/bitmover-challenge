namespace BitMover.Challenge.Contracts.Analytics
{
    public interface IAnalyticsApiClient
    {
        public Task<LicenseSessionsResponse> GetLicenseSessionsAsync(string licenceKey, DateTime from, DateTime to);
    }
}
