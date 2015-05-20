#region Usings
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
    public class ForexTradingService : IForexTradingService
    {

        #region Private Fields
        private readonly ITradingResultsRepository _tradingResultsRepository;
        #endregion

        #region Constructors and Destructors
        public ForexTradingService(ITradingResultsRepository tradingResultsRepository)
        {
            _tradingResultsRepository = tradingResultsRepository;
            BuyQuantities = new List<double>();
            Profits = new List<double>();
            TradeLog = new List<TradeLogRecord>();
            MarginRatio = 0.02;
            BidSize = 2000.0;
        }
        #endregion

        #region Implemented Interfaces

        #region IForexTradingService
        public double MarginRatio { get; set; }
        public double BidSize { get; set; }
        public List<double> BuyQuantities { get; private set; }
        public List<double> Profits { get; private set; }
        public List<TradeLogRecord> TradeLog { get; private set; }

        public void PlaceBid(ForexTreeData record, MarketAction action)
        {
            switch (action)
            {
                case MarketAction.Buy:
                    ExecuteBuy(record);
                    break;
                case MarketAction.Sell:
                    ExecuteSell(record);
                    break;
                case MarketAction.Hold:
                    SaveLogRecord(0.0, 0.0, 0.0, MarketAction.Hold, record.Action);
                    break;
                default:
                    throw new BllException("Incorrect market action.");
            }
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
            _tradingResultsRepository.Clear();
        }

        #endregion

        #endregion

        #region Methods
        private void ExecuteBuy(ForexTreeData record)
        {
            var tradeUnits = BidSize / MarginRatio;
            var unitsBought = tradeUnits * record.Bid;
            BuyQuantities.Add(unitsBought);
            SaveLogRecord(unitsBought, 0.0, 0.0, MarketAction.Buy, record.Action);
        }

        private void ExecuteSell(ForexTreeData record)
        {
            if (BuyQuantities.Count < 1)
            {
                throw new BllException("You have no open positions to close for sell.");
            }

            var tradeUnits = BidSize / MarginRatio;
            var unitsSold = BuyQuantities[0];
            var profit = MathHelpers.GreedyCurrencyPrecision(unitsSold / record.Ask - tradeUnits);
            Profits.Add(profit);

            BuyQuantities.RemoveAt(0);
            SaveLogRecord(0.0, unitsSold, profit, MarketAction.Sell, record.Action);
        }

        private void SaveLogRecord(double bought, double sold, double profit, MarketAction executedAction, MarketAction correctAction)
        {
            TradeLog.Add(new TradeLogRecord
            {
                QuantityBought = bought,
                QuantitySold = sold,
                Profit = profit,
                ExecutedAction = executedAction,
                CorrectAction = correctAction
            });
        }
        #endregion

    }
}
