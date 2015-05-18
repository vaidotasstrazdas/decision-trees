using System.Collections.Generic;
using Bridge.IBLL.Data;

namespace Bridge.IBLL.Interfaces
{
    public interface IStatisticsService
    {

        string BluePrint { get; set; }
        List<StatisticsSequenceDto> StatisticsSequence { get; }

        void ReadStatisticsData(string path);
        void PrepareData();
        StatisticsDto CalculateStatistics();
        void AddToRepository(StatisticsDto statistics);
        void CommitToRepository(string path);

    }
}
