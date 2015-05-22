#region Usings
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Bridge.IBLL.Exceptions;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class ForexTradingStatisticsServiceTests
    {

        #region Private Fields
        private Mock<ICsvDataRepository<TradeLogRecord>> _tradeLogCsvDataRepositoryMock;
        private Mock<ITradingResultsRepository> _tradingResultsRepositoryMock;
        private ForexTradingStatisticsService _service;
        private List<TradeLogRecord> _tradeLog;

        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _tradeLogCsvDataRepositoryMock = new Mock<ICsvDataRepository<TradeLogRecord>>();
            _tradingResultsRepositoryMock = new Mock<ITradingResultsRepository>();

            _tradeLog = new List<TradeLogRecord>
            {
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Hold,
                    Profit = 0.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Buy,
                    Profit = 0.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Sell,
                    Profit = 10.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Sell,
                    ExecutedAction = MarketAction.Sell,
                    Profit = 11.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Buy,
                    Profit = 0.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Sell,
                    ExecutedAction = MarketAction.Sell,
                    Profit = -5.5
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Sell,
                    ExecutedAction = MarketAction.Sell,
                    Profit = -3.9
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Sell,
                    ExecutedAction = MarketAction.Sell,
                    Profit = 0.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Hold,
                    Profit = 0.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Hold,
                    Profit = 0.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Buy,
                    Profit = 0.0
                },
                new TradeLogRecord
                {
                    CorrectAction = MarketAction.Buy,
                    ExecutedAction = MarketAction.Buy,
                    Profit = 0.0
                }
            };
            _tradeLogCsvDataRepositoryMock
                .Setup(x => x.CsvLinesNormalized)
                .Returns(_tradeLog);

            _service = new ForexTradingStatisticsService(
                        _tradeLogCsvDataRepositoryMock.Object,
                        _tradingResultsRepositoryMock.Object);
        }
        #endregion

        #region TestSuite

        #region CalculateTradeStatistics Tests

        #region CalculateTradeStatistics_ShouldGetCorrectCountOfTotalHolds
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectCountOfTotalHolds()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(3, statistics.TotalHolds);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectCountOfTotalSells
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectCountOfTotalSells()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(5, statistics.TotalSells);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectCountOfTotalBuys
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectCountOfTotalBuys()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(4, statistics.TotalBuys);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectCountOfMistakes
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectCountOfMistakes()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(4, statistics.Mistakes);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectTotalProfit
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectTotalProfit()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(11.6, statistics.TotalProfit);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectPositiveProfitCloses
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectPositiveProfitCloses()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(2, statistics.PositiveProfitCloses);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectNegativeProfitCloses
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectNegativeProfitCloses()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(2, statistics.NegativeProfitCloses);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectZeroProfitCloses
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectZeroProfitCloses()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(1, statistics.ZeroProfitCloses);
        }
        #endregion

        #region CalculateTradeStatistics_ShouldGetCorrectTotalActions
        [TestMethod]
        public void CalculateTradeStatistics_ShouldGetCorrectTotalActions()
        {
            _service.ReadCsv(string.Empty);

            var statistics = _service.CalculateTradeStatistics();

            Assert.AreEqual(12, statistics.TotalActions);
        }
        #endregion

        #endregion

        #region CleanTradeLog Tests

        #region CleanTradeLog_ShouldRemoveAllExecutedHolds
        [TestMethod]
        public void CleanTradeLog_ShouldRemoveAllExecutedHolds()
        {
            _service.ReadCsv(string.Empty);

            _service.CleanTradeLog();

            Assert.AreEqual(0, _tradeLog.Count(x => x.ExecutedAction == MarketAction.Hold));
        }
        #endregion

        #region CleanTradeLog_ShouldRemoveLastBuys
        [TestMethod]
        public void CleanTradeLog_ShouldRemoveLastBuys()
        {
            _service.ReadCsv(string.Empty);

            _service.CleanTradeLog();

            Assert.AreEqual(MarketAction.Sell, _tradeLog.Last().ExecutedAction);
        }
        #endregion

        #endregion

        #region CommitToRepository Tests

        #region CommitToRepository_DalExceptionThrown_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Exception of DAL: Commit is forbidden.")]
        public void CommitToRepository_DalExceptionThrown_ShouldThrowBllException()
        {
            _tradingResultsRepositoryMock
                .Setup(x => x.Save(It.IsAny<string>()))
                .Throws(new DalException("Commit is forbidden."));

            _service.CommitToRepository(null);
        }
        #endregion

        #region CommitToRepository_ShouldCallSave
        [TestMethod]
        public void CommitToRepository_ShouldCallSave()
        {
            _service.CommitToRepository("Test");

            _tradingResultsRepositoryMock
                .Verify(x => x.Save("Test"), Times.Once());
        }
        #endregion

        #endregion

        #endregion

    }
}
