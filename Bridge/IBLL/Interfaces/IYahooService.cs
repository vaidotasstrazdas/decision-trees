using System.Collections.Generic;
using Bridge.IBLL.Data;

namespace Bridge.IBLL.Interfaces
{
    public interface IYahooService
    {

        void ReadCsv(string filePath);
        IEnumerable<YahooNormalized> PrepareData();
        void SaveYahooData(IList<YahooNormalized> yahooRecords, string path);

    }
}
