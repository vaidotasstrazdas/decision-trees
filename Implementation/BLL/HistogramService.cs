#region Usings
using System.Collections.Generic;
using System.Linq;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL.Base;
using Implementation.BLL.Helpers;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Implementation.BLL
{
    public class HistogramService : StatisticsBase, IHistogramService
    {

        #region Private Fields
        private readonly IHistogramResultsRepository _histogramResultsRepository;
        #endregion

        #region Constructors and Destructors
        public HistogramService(
            ICsvDataRepository<StatisticsRecord> statistiCsvDataRepository,
            IHistogramResultsRepository histogramResultsRepository)
            : base(statistiCsvDataRepository)
        {
            _histogramResultsRepository = histogramResultsRepository;
        }
        #endregion

        #region Implemented Interfaces

        #region IHistogramService
        public double IntervalLength { get; set; }

        public List<HistogramDto> CalculateStatistics()
        {
            var histogramDictionary = new Dictionary<double, HistogramDto>();
            var records = StatistiCsvDataRepository.CsvLinesNormalized;
            for (var i = 0; i < records.Count; i += 2)
            {
                var c45Record = records[i];
                var c50Record = records[i + 1];
                var c45IntervalFrom = GetIntervalFrom(c45Record);
                var c50IntervalFrom = GetIntervalFrom(c50Record);

                AddToHistogram(histogramDictionary, c45IntervalFrom, DecisionTreeAlgorithm.C45);
                AddToHistogram(histogramDictionary, c50IntervalFrom, DecisionTreeAlgorithm.C50);
            }

            var histogram = histogramDictionary.Values.OrderBy(x => x.IntervalFrom).ToList();

            return histogram;
        }

        public void AddToRepository(List<HistogramDto> statistics)
        {
            foreach (var histogramDto in statistics)
            {
                _histogramResultsRepository.Add(new HistogramResult
                {
                    BluePrint = histogramDto.IntervalFrom + "-" + histogramDto.IntervalTo,
                    C45Cases = histogramDto.C45Cases,
                    C50Cases = histogramDto.C50Cases
                });
            }
        }

        public void CommitToRepository(string path)
        {
            try
            {
                _histogramResultsRepository.Save(path);
            }
            catch (DalException exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of DAL", exception.Message));
            }
        }
        #endregion

        #endregion

        #region Methods
        private double GetIntervalFrom(StatisticsRecord record)
        {
            var percentage = (double) record.Errors / record.Cases;
            var part = (int) (MathHelpers.CurrencyPrecision(percentage) / IntervalLength);

            return part * IntervalLength;
        }

        private void AddToHistogram(Dictionary<double, HistogramDto> histogramDictionary, double intervalFrom, DecisionTreeAlgorithm algorithm)
        {
            if (!histogramDictionary.ContainsKey(intervalFrom))
            {
                histogramDictionary.Add(intervalFrom, new HistogramDto
                {
                    IntervalFrom = MathHelpers.PreservePrecision(intervalFrom),
                    IntervalTo = MathHelpers.PreservePrecision(intervalFrom + IntervalLength)
                });
            }
            var histogramDto = histogramDictionary[intervalFrom];

            switch (algorithm)
            {
                case DecisionTreeAlgorithm.C45:
                    histogramDto.C45Cases++;
                    break;
                case DecisionTreeAlgorithm.C50:
                    histogramDto.C50Cases++;
                    break;
            }
        }
        #endregion

    }
}
