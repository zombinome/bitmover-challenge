using BitMover.Challenge.Contracts.Billing;
using Newtonsoft.Json;

namespace BitMover.Challenge.Mocks
{
    public class BillingMessageSender : IBillingMessageSender
    {
        public Task SendMessageAsync(BillingMessage message)
        {
            var messageStr = JsonConvert.ToString(message);
            Console.WriteLine("Sent billing message: " + messageStr);
            return Task.CompletedTask;
        }
    }
}
