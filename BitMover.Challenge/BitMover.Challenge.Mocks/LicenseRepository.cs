using BitMover.Challenge.Contracts.Storage;
using Microsoft.Data.Sqlite;

namespace BitMover.Challenge.Mocks
{
    public class LicenseRepository : ILicenseRepository
    {
        private const string ConnectionString = "Data Source=InMemoryStorage;Mode=Memory;Cache=Shared";

        private const string LicenseKeyField = "LicenseKey";
        private const string LastUpdatedField = "LastUpdated";
        private const string IsDeadField = "IsDead";
        private const string LicensesTable = "Licenses";

        // Connection needed to keep database exist
        private static readonly SqliteConnection _connection = new SqliteConnection(ConnectionString);
        static LicenseRepository()
        {
            _connection.Open();
        }

        public async Task<IEnumerable<Guid>> ListLicensesAsync(DateTime timestamp, int offset, int limit)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"SELECT {LicenseKeyField} FROM {LicensesTable} 
                           WHERE {IsDeadField} = 0 AND {LastUpdatedField} < $timestamp
                           ORDER BY {LastUpdatedField} LIMIT $offset, $limit";
                    command.Parameters.AddWithValue("$offset", offset);
                    command.Parameters.AddWithValue("$limit", limit);
                    command.Parameters.AddWithValue("$timestamp", timestamp);

                    var result = new List<Guid>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var value = (string)reader.GetValue(0);
                            var key = Guid.Parse(value);
                            result.Add(key);
                        }
                    }

                    return result;
                }
            }
        }

        public async Task MarkLicenseDeadAsync(Guid licenseKey)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"UPDATE {LicensesTable} 
                           SET ${IsDeadField} = 1
                           WHERE ${LicenseKeyField} = $licenceKey";
                    command.Parameters.AddWithValue("$licenceKey", licenseKey.ToString());
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateLicenceTimestampAsync(Guid licenseKey)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"UPDATE {LicensesTable} 
                           SET ${LastUpdatedField} = $lastUpdated
                           WHERE ${LicenseKeyField} = $licenceKey";
                    command.Parameters.AddWithValue("$licenceKey", licenseKey.ToString());
                    command.Parameters.AddWithValue("$lastUpdated", DateTime.UtcNow);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task Initialize(int count, int deadCount)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var createCommand = connection.CreateCommand())
                {
                    createCommand.CommandText =
                        $@"CREATE TABLE {LicensesTable} (
                            {LicenseKeyField} TEXT,
                            {LastUpdatedField} STRING,
                            {IsDeadField} INTEGER
                        )";
                    await createCommand.ExecuteNonQueryAsync();
                }

                await InsertLicensesAsync(connection, count, false);
                await InsertLicensesAsync(connection, deadCount, true);
            }
        }

        public static async Task Finalize()
        {
            await _connection.CloseAsync();
        }

        private static async Task InsertLicensesAsync(SqliteConnection connection, int count, bool isDead)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            for (var i = 0; i < count; i++)
            {
                int delta = random.Next(5);
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"INSERT INTO {LicensesTable} ({LicenseKeyField}, {LastUpdatedField}, {IsDeadField})
                            VALUES ($licenseKey, $lastUpdated, $isDead)";
                    command.Parameters.AddWithValue("$licenseKey", Guid.NewGuid().ToString());
                    command.Parameters.AddWithValue("$lastUpdated", DateTime.UtcNow.AddSeconds(-delta));
                    command.Parameters.AddWithValue("$isDead", isDead ? 1 : 0);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}