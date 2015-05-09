using System.Collections.Generic;

namespace Bridge.IDLL.Interfaces
{
    public interface ICsvDataRepository<TRecord>
    {

        List<List<string>> CsvLines { get; set; }
        List<TRecord> CsvLinesNormalized { get; set; }

        void LoadData(string dataFile);
        void NormalizeData(int skip = 1);

    }
}
