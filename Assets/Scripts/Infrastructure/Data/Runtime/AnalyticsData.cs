namespace Infrastructure.Data.Runtime
{
    public class AnalyticsData
    {
        public int SessionsCount;

        public bool IsFirstSession => SessionsCount == 1;
    }
}
