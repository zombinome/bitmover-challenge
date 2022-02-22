namespace BitMover.Challenge.Contracts.Billing
{
    public interface IBillingMessageSender
    {
        Task SendMessageAsync(BillingMessage message);
    }
}
