#region Usings
using System.Collections.Generic;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
#endregion

namespace Implementation.BLL.Base
{
    public abstract class StatisticsBase : IStatisticsBase
    {

        #region Protected Fields
        protected readonly ICsvDataRepository<StatisticsRecord> StatistiCsvDataRepository;
        #endregion

        #region Constructors and Destructors
        protected StatisticsBase(
            ICsvDataRepository<StatisticsRecord> statistiCsvDataRepository)
        {
            StatistiCsvDataRepository = statistiCsvDataRepository;
            StatisticsSequence = new List<StatisticsSequenceDto>();
        }
        #endregion

        #region Implemented Interfaces

        #region IStatisticsBase
        public List<StatisticsSequenceDto> StatisticsSequence { get; private set; }

        public void ReadStatisticsData(string path)
        {
            try
            {
                StatistiCsvDataRepository.LoadData(path);
            }
            catch (DalException exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of DAL", exception.Message));
            }

            StatistiCsvDataRepository.NormalizeData();
        }

        public void PrepareData()
        {
            var lines = StatistiCsvDataRepository.CsvLinesNormalized;
            var linesCount = lines.Count;
            if (linesCount % 2 != 0)
            {
                throw new BllException("Incorrect number of elements in CSV file. Number of elements should be multiple of two.");
            }

            for (var i = 0; i < linesCount; i += 2)
            {
                var c45Record = lines[i];
                var c50Record = lines[i + 1];
                StatisticsSequence.Add(new StatisticsSequenceDto
                {
                    C45Errors = c45Record.Errors,
                    C50Errors = c50Record.Errors,
                    Cases = c45Record.Cases,
                    Chunk = c45Record.Chunk
                });
            }
        }

        public void ResetSequence()
        {
            StatisticsSequence.Clear();
        }
        #endregion

        #endregion

    }
}
