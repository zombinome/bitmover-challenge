using BitMover.Challenge.Mocks;
using BitMover.Challenge.Service;

// Initializing mocks
await LicenseRepository.Initialize(50, 0);

var dbRepository = new LicenseRepository();
var messageSender = new BillingMessageSender();
var analyticsApi = new AnalyticsApiClient();

var service = new BillingUpdateService(dbRepository, messageSender, analyticsApi);
await service.RunAsync(CancellationToken.None);


// Finalizing mocks
await LicenseRepository.Finalize();