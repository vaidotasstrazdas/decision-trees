using Bridge.IDLL.Data;
using Bridge.IDLL.Interfaces;
using Implementation.DLL.RepositoryBase;

namespace Implementation.DLL
{

    public class ForexCsvRepository : CsvDataRepository<ForexRecord>, IForexCsvRepository
    {

        public void NormalizeData()
        {
            NormalizeData(0);
        }

    }

}
