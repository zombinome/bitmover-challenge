namespace BitMover.Challenge.Contracts.Storage
{
    public interface ILicenseRepository
    {
        public Task<IReadOnlyCollection<License>> ListLicensesAsync(DateTime timestamp, int offset, int limit);

        public Task UpdateLicenceTimestampAsync(Guid licenseKey, DateTime timestamp);

        public Task IncrementFailureCountAsync(Guid licenseKey);

        public Task MarkLicenseDeadAsync(Guid licenceKey);
    }
}
