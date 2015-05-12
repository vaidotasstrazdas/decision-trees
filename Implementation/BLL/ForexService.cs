using System;
using System.Collections.Generic;
using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;

namespace Implementation.BLL
{
    public class ForexService : IForexService
    {
        private readonly IForexCsvRepository _forexCsvRepository;
        private readonly ITreeDataRepository<ForexTreeData> _forexTreeDataRepository;

        #region Constructors and Destructors
        public ForexService(
            IForexCsvRepository forexCsvRepository,
            ITreeDataRepository<ForexTreeData> forexTreeDataRepository)
        {
            _forexCsvRepository = forexCsvRepository;
            _forexTreeDataRepository = forexTreeDataRepository;
        }
        #endregion

        #region Implemented Interfaces

        #region IService
        public void ReadCsv(string filePath)
        {
            try
            {
                _forexCsvRepository.LoadData(filePath);
                _forexCsvRepository.NormalizeData();
            }
            catch (DalException exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of DAL", exception.Message));
            }
            catch (Exception exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of other origin", exception.Message));
            }
        }
        #endregion

        #region IForexService
        public void PrepareData(int recalculationPeriodSeconds, int splitPeriodSeconds)
        {
            throw new System.NotImplementedException();
        }

        public void SaveForexData(IList<ForexDto> forexRecords, string path)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #endregion

    }
}
