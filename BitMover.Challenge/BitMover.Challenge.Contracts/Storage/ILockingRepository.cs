namespace BitMover.Challenge.Contracts.Storage
{
    public interface ILockingRepository
    {
        Task ReleaseAsync(Guid licenseKey);
        Task<bool> TryAccquireAsync(Guid licenseKey);
    }

}
