#region Usings
using System.Collections.Generic;
using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL.Base;

#endregion

namespace Implementation.BLL
{
    public class StatisticsService : StatisticsBase, IStatisticsService
    {

        #region Private Fields
        private readonly ICsvDataRepository<StatisticsRecord> _statistiCsvDataRepository;
        private readonly IStatisticsResultsRepository _statisticsResultsRepository;
        #endregion

        #region Constructors and destructors
        public StatisticsService(
            ICsvDataRepository<StatisticsRecord> statistiCsvDataRepository,
            IStatisticsResultsRepository statisticsResultsRepository)
            : base(statistiCsvDataRepository)
        {
            _statisticsResultsRepository = statisticsResultsRepository;
        }
        #endregion

        #region Implemented Interfaces

        #region IStatisticsService
        public string BluePrint { get; set; }

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
