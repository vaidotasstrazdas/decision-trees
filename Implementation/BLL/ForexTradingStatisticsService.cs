#region Usings
using System;
using System.Collections.Generic;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL.Helpers;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Implementation.BLL
{
    public class ForexTradingStatisticsService : IForexTradingStatisticsService
    {

        #region Private Fields
        private readonly ICsvDataRepository<TradeLogRecord> _tradeLogCsvDataRepository;
        private readonly ITradingResultsRepository _tradingResultsRepository;
        #endregion

        #region Constructors and Destructors
        public ForexTradingStatisticsService(
            ICsvDataRepository<TradeLogRecord> tradeLogCsvDataRepository,
            ITradingResultsRepository tradingResultsRepository)
        {
            _tradeLogCsvDataRepository = tradeLogCsvDataRepository;
            _tradingResultsRepository = tradingResultsRepository;
        }
        #endregion

        #region Implemented interfaces

        #region IService
        public void ReadCsv(string filePath)
        {
            try
            {
                _tradeLogCsvDataRepository.LoadData(filePath);
                _tradeLogCsvDataRepository.NormalizeData();
                TradeLog = _tradeLogCsvDataRepository.CsvLinesNormalized;
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

        #region IForexTradingStatisticsService
        public List<TradeLogRecord> TradeLog { get; private set; }

        public TradeStatistics CalculateTradeStatistics()
        {
            var statistics = new TradeStatistics { TotalActions = TradeLog.Count };
            foreach (var record in TradeLog)
            {
                switch (record.ExecutedAction)
                {
                    case MarketAction.Buy:
                        statistics.TotalBuys++;
                        break;
                    case MarketAction.Sell:
                        statistics.TotalSells++;
                        if (record.Profit < 0.0)
                        {
                            statistics.NegativeProfitCloses++;
                        } else if (record.Profit > 0.0)
                        {
                            statistics.PositiveProfitCloses++;
                        }
                        else
                        {
                            statistics.ZeroProfitCloses++;
                        }
                        break;
                    case MarketAction.Hold:
                        statistics.TotalHolds++;
                        break;
                }

                if (record.ExecutedAction != record.CorrectAction)
                {
                    statistics.Mistakes++;
                }

                statistics.TotalProfit += record.Profit;

            }
            statistics.TotalProfit = MathHelpers.CurrencyPrecision(statistics.TotalProfit);

            return statistics;
        }

        public void CleanTradeLog()
        {
            TradeLog.RemoveAll(x => x.ExecutedAction == MarketAction.Hold);
            var elementsToRemove = new List<TradeLogRecord>();
            for(var i = TradeLog.Count - 1; i >= 0; i--)
            {
                if (TradeLog[i].ExecutedAction != MarketAction.Buy)
                {
                    break;
                }
                elementsToRemove.Add(TradeLog[i]);
            }
            TradeLog.RemoveAll(x => elementsToRemove.Contains(x));
        }

        public void AddToRepository()
        {
            foreach (var logRecord in TradeLog)
            {
                _tradingResultsRepository.Add(logRecord);
            }
        }

        public void CommitToRepository(string path)
        {
            try
            {
                _tradingResultsRepository.Save(path);
            }
            catch (DalException exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of DAL", exception.Message));
            }
        }

        public void Clear()
        {
            TradeLog.Clear();
            _tradingResultsRepository.Clear();
        }
        #endregion

        #endregion

    }
}
