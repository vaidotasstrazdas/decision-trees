#region Usings
using System.Collections.Generic;
using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
#endregion

namespace Implementation.BLL
{
    public class StatisticsService : IStatisticsService
    {

        #region Private Fields
        private readonly ICsvDataRepository<StatisticsRecord> _statistiCsvDataRepository;
        private readonly IStatisticsResultsRepository _statisticsResultsRepository;
        #endregion

        #region Constructors and destructors
        public StatisticsService(
            ICsvDataRepository<StatisticsRecord> statistiCsvDataRepository,
            IStatisticsResultsRepository statisticsResultsRepository)
        {
            _statistiCsvDataRepository = statistiCsvDataRepository;
            _statisticsResultsRepository = statisticsResultsRepository;
            StatisticsSequence = new List<StatisticsSequenceDto>();
        }
        #endregion

        #region Implemented Interfaces

        #region IStatisticsService
        public string BluePrint { get; set; }
        public List<StatisticsSequenceDto> StatisticsSequence { get; private set; }

        public void ReadStatisticsData(string path)
        {
            try
            {
                _statistiCsvDataRepository.LoadData(path);
            }
            catch (DalException exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of DAL", exception.Message));
            }

            _statistiCsvDataRepository.NormalizeData();
        }

        public void PrepareData()
        {
            var lines = _statistiCsvDataRepository.CsvLinesNormalized;
            var linesCount = lines.Count;
            if (linesCount%2 != 0)
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

        public StatisticsDto CalculateStatistics()
        {
            var statistics = new StatisticsDto();

            foreach (var element in StatisticsSequence)
            {
                if (element.C45Errors == element.C50Errors)
                {
                    statistics.Equal++;
                }
                else if (element.C45Errors < element.C50Errors)
                {
                    statistics.C45Better++;
                }
                else
                {
                    statistics.C50Better++;
                }
            }

            return statistics;
        }

        public void AddToRepository(StatisticsDto statistics)
        {
            if (string.IsNullOrWhiteSpace(BluePrint))
            {
                throw new BllException("BluePrint not set.");
            }

            if (statistics == null)
            {
                throw new BllException("StatisticsDto can not be null.");
            }

            var statisticsResult = new StatisticsResult
            {
                BluePrint = BluePrint,
                C45Better = statistics.C45Better,
                C50Better = statistics.C50Better,
                Equal = statistics.Equal
            };

            _statisticsResultsRepository.Add(statisticsResult);

        }

        public void CommitToRepository(string path)
        {
            try
            {
                _statisticsResultsRepository.Save(path);
            }
            catch (DalException exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of DAL", exception.Message));
            }
        }
        #endregion

        #endregion

    }
}
