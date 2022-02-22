namespace BitMover.Challenge.Contracts.Billing
{
    public class BillingMessage
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public long Usage { get; set; }

        public string Product { get; set; }

        public Guid LicenceKey { get; set; }
    }
}
