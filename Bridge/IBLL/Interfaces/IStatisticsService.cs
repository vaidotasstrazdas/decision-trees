using Bridge.IBLL.Data;

namespace Bridge.IBLL.Interfaces
{
    public interface IStatisticsService : IStatisticsBase
    {

        string BluePrint { get; set; }

        StatisticsDto CalculateStatistics();
        void AddToRepository(StatisticsDto statistics);
        void CommitToRepository(string path);

    }
}
