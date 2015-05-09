using System.Collections.Generic;
using Bridge.IBLL.Data;

namespace Bridge.IBLL.Interfaces
{
    public interface IYahooService
    {

        void ReadCsv(string filePath);
        List<YahooNormalized> PrepareData();

    }
}
