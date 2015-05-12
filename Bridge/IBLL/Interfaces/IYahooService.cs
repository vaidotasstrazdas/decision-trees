using System.Collections.Generic;
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces.Base;

namespace Bridge.IBLL.Interfaces
{
    public interface IYahooService : IService
    {

        IEnumerable<YahooNormalized> PrepareData(int updatePeriod = 100);
        void SaveYahooData(IList<YahooNormalized> yahooRecords, string path);

    }
}
