#region Usings
using System.Collections.Generic;
using Bridge.IBLL.Data;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IStatisticsBase
    {

        List<StatisticsSequenceDto> StatisticsSequence { get; }

        void ReadStatisticsData(string path);
        void PrepareData();
        void ResetSequence();

    }
}
