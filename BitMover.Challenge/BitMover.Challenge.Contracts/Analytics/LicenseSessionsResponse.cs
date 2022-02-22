namespace BitMover.Challenge.Contracts.Analytics
{
    public class LicenseSessionsResponse
    {
        public int RowCount { get; set; }

        public IEnumerable<long[]> Rows { get; set; } = Enumerable.Empty<long[]>();
    }
}
