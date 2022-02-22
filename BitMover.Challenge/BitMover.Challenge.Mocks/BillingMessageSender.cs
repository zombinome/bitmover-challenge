using BitMover.Challenge.Contracts.Billing;
using Newtonsoft.Json;

namespace BitMover.Challenge.Mocks
{
    public class BillingMessageSender : IBillingMessageSender
    {
        public Task SendMessageAsync(BillingMessage message)
        {
            var messageStr = JsonConvert.SerializeObject(message);
            Console.WriteLine($"[{Thread.CurrentThread.Name}]: Sent billing message: {messageStr}");
            return Task.CompletedTask;
        }
    }
}
