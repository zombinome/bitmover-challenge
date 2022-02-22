using Microsoft.Data.Sqlite;

namespace BitMover.Challenge.Mocks
{
    public static class InMemDb
    {
        public const string ConnectionString = "Data Source=InMemoryStorage;Mode=Memory;Cache=Shared";

        // Connection needed to keep database exist
        private static readonly SqliteConnection connection = new SqliteConnection(ConnectionString);

        public static Task InitAsync()
        {
            return connection.OpenAsync();
        }
            
        public static Task FinalizeAsync()
        {
            return connection.CloseAsync();
        }
    }
}
