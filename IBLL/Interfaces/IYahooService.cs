using System.Collections.Generic;
using IBLL.Data;

namespace IBLL.Interfaces
{
    public interface IYahooService
    {

        void ReadCsv(string filePath);
        List<YahooNormalized> PrepareData();

    }
}
