#region Usings

using System.Collections.Generic;
using Bridge.IBLL.Data;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IHistogramService : IStatisticsBase
    {

        double IntervalLength { get; set; }

        List<HistogramDto> CalculateStatistics();
        void AddToRepository(List<HistogramDto> statistics);
        void CommitToRepository(string path);
        void Clear();

    }
}
