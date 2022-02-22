namespace BitMover.Challenge.Contracts.Storage
{
    public interface ILicenseRepository
    {
        public Task<IEnumerable<Guid>> ListLicensesAsync(DateTime timestamp, int offset, int limit);

        public Task UpdateLicenceTimestampAsync(Guid licenseKey);

        public Task MarkLicenseDeadAsync(Guid licenceKey);
    }
}
