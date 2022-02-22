// See https://aka.ms/new-console-template for more information
using BitMover.Challenge.Mocks;

// Initializing mocks
await LicenseRepository.Initialize(50, 0);

var dbRepository = new LicenseRepository();
var licenses = await dbRepository.ListLicensesAsync(DateTime.UtcNow.AddMinutes(50), 0, 50);
foreach (var license in licenses)
{
    Console.WriteLine(license);
}


// Finalizing mocks
await LicenseRepository.Finalize();