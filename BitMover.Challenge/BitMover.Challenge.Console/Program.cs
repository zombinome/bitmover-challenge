using BitMover.Challenge.Mocks;
using BitMover.Challenge.Service;

// Initializing mocks
await InMemDb.InitAsync();
await LicenseRepository.Initialize(50, 0);
await LockingRepository.Initialize();

var dbRepository = new LicenseRepository();
var messageSender = new BillingMessageSender();
var analyticsApi = new AnalyticsApiClient();
var locksRepository = new LockingRepository();

var service = new BillingUpdateService(dbRepository, messageSender, analyticsApi, locksRepository);

var thread1 = new Thread(() => { service.RunAsync(CancellationToken.None).Wait(); });
thread1.Name = "WORKER 1";
thread1.Start();
var thread2 = new Thread(() => { service.RunAsync(CancellationToken.None).Wait(); });
thread2.Name = "WORKER 2";
thread2.Start();
var thread3 = new Thread(() => { service.RunAsync(CancellationToken.None).Wait(); });
thread3.Name = "WORKER 3";
thread3.Start();

thread1.Join();
thread2.Join();
thread3.Join();


// Finalizing mocks
await InMemDb.FinalizeAsync();