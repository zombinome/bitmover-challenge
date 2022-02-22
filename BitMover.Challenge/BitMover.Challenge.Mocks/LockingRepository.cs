using BitMover.Challenge.Contracts.Storage;
using Microsoft.Data.Sqlite;

namespace BitMover.Challenge.Mocks
{
    public class LockingRepository : ILockingRepository
    {
        private const string LocksTable = "locks";
        private const string LicenseKeyField = "licenseKey";
        private const string ExpiresAtField = "expiresAt";

        public async Task<bool> TryAccquireAsync(Guid licenseKey)
        {
            using (var connection = new SqliteConnection(InMemDb.ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"DELETE FROM {LocksTable} 
                           WHERE {LicenseKeyField} = $licenceKey AND {ExpiresAtField} < datetime('now')";
                    command.Parameters.AddWithValue("$licenceKey", licenseKey.ToString());
                    await command.ExecuteNonQueryAsync();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"INSERT OR IGNORE INTO {LocksTable} ({LicenseKeyField}, {ExpiresAtField})
                            VALUES ($licenseKey, datetime('now'))";
                    command.Parameters.AddWithValue("$licenseKey", licenseKey.ToString());
                    var rowsInserted = await command.ExecuteNonQueryAsync();
                    return rowsInserted > 0;
                }
            }
        }

        public async Task ReleaseAsync(Guid licenseKey)
        {
            using (var connection = new SqliteConnection(InMemDb.ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"DELETE FROM {LocksTable} 
                           WHERE {LicenseKeyField} = $licenceKey AND expiresAt < datetime('now')";
                    command.Parameters.AddWithValue("$licenceKey", licenseKey.ToString());
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task Initialize()
        {
            using (var connection = new SqliteConnection(InMemDb.ConnectionString))
            {
                await connection.OpenAsync();
                using (var createCommand = connection.CreateCommand())
                {
                    createCommand.CommandText =
                        $@"CREATE TABLE {LocksTable} (
                            {LicenseKeyField} TEXT PRIMARY KEY,
                            {ExpiresAtField} TEXT
                        )";
                    await createCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
