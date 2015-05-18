#region Usings
using Bridge.IDLL.Data;
using Bridge.IDLL.Interfaces;
using Implementation.DLL.RepositoryBase;
#endregion

namespace Implementation.DLL
{

    public class ForexCsvRepository : CsvDataRepository<ForexRecord>, IForexCsvRepository
    {

        #region Implemented Interfaces

        #region IForexCsvRepository
        public void NormalizeData()
        {
            NormalizeData(0);
        }
        #endregion

        #endregion

    }

}
