namespace BitMover.Challenge.Contracts.Storage
{
    public class License
    {
        public Guid Key { get; set; }

        public DateTime LastUpdated { get; set; }

        public int AttemptsCount { get; set; } = 0;
    }
}
