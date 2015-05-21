using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces;
using ForexTradeModel;
using Shared.DecisionTrees.DataStructure;

namespace Tests.ForexTradeModelTest
{
    [TestClass]
    public class TradingModelTests
    {
        private Mock<IStatisticsService> _statisticsServiceMock;
        private Mock<IForexMarketService> _forexMarketServiceMock;
        private Mock<IForexTradingAgentService> _forexTradingAgentServiceMock;
        private Mock<IForexTradingService> _forexTradingServiceMock;
        private List<string> _periods;
        private List<string> _months;
        private List<StatisticsSequenceDto> _statisticsSequence;
        private TradingModel _model;
        private int _nextRecordCalls;
        private int _nextRecordCallsOverall;
        private List<ForexTreeData> _forexTrees;

        [TestInitialize]
        public void TestInitialize()
        {
            _statisticsServiceMock = new Mock<IStatisticsService>();
            _forexMarketServiceMock = new Mock<IForexMarketService>();
            _forexTradingAgentServiceMock = new Mock<IForexTradingAgentService>();
            _forexTradingServiceMock = new Mock<IForexTradingService>();

            _periods = new List<string> { "300", "600", "900", "1800" };

            _months = new List<string>();
            for (var i = 1; i <= 12; i++)
            {
                _months.Add(string.Format("{0:00}", i));
            }

            _statisticsSequence = new List<StatisticsSequenceDto>
            {
                new StatisticsSequenceDto
                {
                    C45Errors = 10,
                    C50Errors = 9,
                    Cases = 500,
                    Chunk = 0
                },
                new StatisticsSequenceDto
                {
                    C45Errors = 10,
                    C50Errors = 9,
                    Cases = 499,
                    Chunk = 1
                },
                new StatisticsSequenceDto
                {
                    C45Errors = 10,
                    C50Errors = 9,
                    Cases = 501,
                    Chunk = 2
                },
                new StatisticsSequenceDto
                {
                    C45Errors = 10,
                    C50Errors = 9,
                    Cases = 498,
                    Chunk = 3
                },
                new StatisticsSequenceDto
                {
                    C45Errors = 10,
                    C50Errors = 9,
                    Cases = 502,
                    Chunk = 4
                }
            };

            _nextRecordCalls = 0;
            _nextRecordCallsOverall = 0;
            _forexTrees = new List<ForexTreeData>
            {
                new ForexTreeData
                {
                    Bid = 1.1111,
                    Ask = 1.1115
                },
                new ForexTreeData
                {
                    Bid = 1.1110,
                    Ask = 1.1114
                },
                new ForexTreeData
                {
                    Bid = 1.1109,
                    Ask = 1.1112
                },
                new ForexTreeData
                {
                    Bid = 1.1107,
                    Ask = 1.1110
                },
                new ForexTreeData
                {
                    Bid = 1.1105,
                    Ask = 1.1105
                }
            };

            _forexMarketServiceMock
                .Setup(x => x.NextRecord())
                .Returns(_forexTrees[_nextRecordCalls])
                .Callback(() => {
                    _nextRecordCalls++;
                    _nextRecordCallsOverall++;
                });

            _forexTradingAgentServiceMock
                .Setup(x => x.ClassifyRecord(It.IsAny<ForexTreeData>()))
                .Returns(MarketAction.Buy);

            _forexMarketServiceMock
                .Setup(x => x.IsDone())
                .Returns(() => _nextRecordCalls == _forexTrees.Count);

            _forexMarketServiceMock
                .Setup(x => x.Clear())
                .Callback(() => _nextRecordCalls = 0);

            _statisticsServiceMock
                .SetupGet(x => x.StatisticsSequence)
                .Returns(_statisticsSequence);

            _forexTradingServiceMock
                .Setup(x => x.BidSize)
                .Returns(2000.0);

            _model = new TradingModel(
                _statisticsServiceMock.Object,
                _forexMarketServiceMock.Object,
                _forexTradingAgentServiceMock.Object,
                _forexTradingServiceMock.Object
                );
        }

        #region TestSuite

        #region Initialize Tests

        #region Initialize_ShouldReadStatisticsData
        [TestMethod]
        public void Initialize_ShouldReadStatisticsData()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, string.Empty);

            foreach (var path in from month in _months
                                 from period in _periods
                                 select string.Format("EURUSD_2014_{0}_{1}.csv", month, period)
                    )
            {
                var filePath = path;
                _statisticsServiceMock
                    .Verify(x => x.ReadStatisticsData(filePath), Times.Once());
            }
        }
        #endregion

        #region Initialize_ShouldPrepareStatisticsData
        [TestMethod]
        public void Initialize_ShouldPrepareStatisticsData()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, string.Empty);

            _statisticsServiceMock
                .Verify(x => x.PrepareData(), Times.Exactly(48));
        }
        #endregion

        #region Initialize_ShouldRemoveTooSmallCasesCorrectCount
        [TestMethod]
        public void Initialize_ShouldRemoveTooSmallCasesCorrectCount()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, string.Empty);

            Assert.AreEqual(3, _statisticsSequence.Count);
        }
        #endregion

        #region Initialize_ShouldRemoveTooSmallCasesCorrectElements
        [TestMethod]
        public void Initialize_ShouldRemoveTooSmallCasesCorrectElements()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, string.Empty);

            Assert.AreEqual(500, _statisticsSequence[0].Cases);
            Assert.AreEqual(501, _statisticsSequence[1].Cases);
            Assert.AreEqual(502, _statisticsSequence[2].Cases);
        }
        #endregion

        #region Initialize_ShouldSetFullForexPath
        [TestMethod]
        public void Initialize_ShouldSetFullForexPath()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            Assert.AreEqual("ForexTrees\\EURUSD\\2014", _model.FullForexTreesPath);
        }
        #endregion

        #region Initialize_ShouldSetFullForexPathForMarketService
        [TestMethod]
        public void Initialize_ShouldSetFullForexPathForMarketService()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            _forexMarketServiceMock
                .Verify(x => x.SetForexTreesPath("ForexTrees\\EURUSD\\2014"), Times.Once());
        }
        #endregion

        #region Initialize_ShouldResetSequence
        [TestMethod]
        public void Initialize_ShouldResetSequence()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            _statisticsServiceMock
                .Verify(x => x.ResetSequence(), Times.Exactly(48));
        }
        #endregion

        #region Initialize_ShouldGenerateCorrectCountOfStatisticsSequences
        [TestMethod]
        public void Initialize_ShouldGenerateCorrectCountOfStatisticsSequences()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            Assert.AreEqual(48, _model.StatisticsSequences.Count);
        }
        #endregion

        #region Initialize_ShouldGenerateCorrectMonthKeysForStatisticsSequences
        [TestMethod]
        public void Initialize_ShouldGenerateCorrectMonthKeysForStatisticsSequences()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            var keysExpected = new List<string>();
            for(var i = 0; i < 48; i++)
            {
                keysExpected.Add(string.Format("{0:00}", i / 4 + 1));
            }
            var keysActual = _model.StatisticsSequences.Keys.Select(x => x.Key).ToList();

            CollectionAssert.AreEqual(keysExpected, keysActual);
        }
        #endregion

        #region Initialize_ShouldGenerateCorrectPeriodKeysForStatisticsSequences
        [TestMethod]
        public void Initialize_ShouldGenerateCorrectPeriodKeysForStatisticsSequences()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            var keysExpected = new List<string>();
            for (var i = 0; i < 48; i += 4)
            {
                keysExpected.Add("300");
                keysExpected.Add("600");
                keysExpected.Add("900");
                keysExpected.Add("1800");
            }
            var keysActual = _model.StatisticsSequences.Keys.Select(x => x.Value).ToList();

            CollectionAssert.AreEqual(keysExpected, keysActual);
        }
        #endregion

        #region Initialize_ShouldGenerateCorrectMonthPeriodKeysForStatisticsSequences
        [TestMethod]
        public void Initialize_ShouldGenerateCorrectMonthPeriodKeysForStatisticsSequences()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            var keysExpected = (from month in _months from period in _periods select month + "_" + period).ToList();
            var keysActual = _model.StatisticsSequences.Keys.Select(x => x.Key + "_" + x.Value).ToList();

            CollectionAssert.AreEqual(keysExpected, keysActual);
        }
        #endregion

        #region Initialize_ShouldGenerateIndependentStatisticsSequences
        [TestMethod]
        public void Initialize_ShouldGenerateIndependentStatisticsSequences()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            foreach (var sequenceElement in _model.StatisticsSequences)
            {
                Assert.AreNotSame(_statisticsSequence, sequenceElement.Value);
            }
        }
        #endregion

        #region Initialize_ShouldGenerateFilteredSequences
        [TestMethod]
        public void Initialize_ShouldGenerateFilteredSequences()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");

            foreach (var sequence in _model.StatisticsSequences)
            {
                Assert.AreEqual(3, sequence.Value.Count);
            }
        }
        #endregion

        #endregion

        #region SetModelQuantities Tests

        #region SetModelQuantities_ShouldSetDefaultBalance
        [TestMethod]
        public void SetModelQuantities_ShouldSetDefaultBalance()
        {
            _model.SetModelQuantities();

            Assert.AreEqual(100000.0, _model.Balance);
        }
        #endregion

        #region SetModelQuantities_ShouldChosenBalance
        [TestMethod]
        public void SetModelQuantities_ShouldChosenBalance()
        {
            _model.SetModelQuantities(150000.0);

            Assert.AreEqual(150000.0, _model.Balance);
        }
        #endregion

        #region SetModelQuantities_ShouldSetDefaultBidSize
        [TestMethod]
        public void SetModelQuantities_ShouldSetDefaultBidSize()
        {
            _model.SetModelQuantities();

            _forexTradingServiceMock
                .VerifySet(x => x.BidSize = 2000.0);
        }
        #endregion

        #region SetModelQuantities_ShouldSetDefaultMarginRatio
        [TestMethod]
        public void SetModelQuantities_ShouldSetDefaultMarginRatio()
        {
            _model.SetModelQuantities();

            _forexTradingServiceMock
                .VerifySet(x => x.MarginRatio = 0.02);
        }
        #endregion

        #region SetModelQuantities_ShouldSetChosenBidSize
        [TestMethod]
        public void SetModelQuantities_ShouldSetChosenBidSize()
        {
            _model.SetModelQuantities(100000.0, 1500.0);

            _forexTradingServiceMock
                .VerifySet(x => x.BidSize = 1500.0);
        }
        #endregion

        #region SetModelQuantities_ShouldSetChosenMarginRatio
        [TestMethod]
        public void SetModelQuantities_ShouldSetChosenMarginRatio()
        {
            _model.SetModelQuantities(100000.0, 2000.0, 0.00025);

            _forexTradingServiceMock
                .VerifySet(x => x.MarginRatio = 0.00025);
        }
        #endregion

        #endregion

        #region PrepareForAlgorithm Tests

        #region PrepareForAlgorithm_C45ShouldSortInErrorRatioIncreasingOrder
        [TestMethod]
        public void PrepareForAlgorithm_C45ShouldSortInErrorRatioIncreasingOrder()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);

            var casesExpected = new List<int> { 502, 501, 500 };
            foreach (var sequenceElement in _model.StatisticsSequences)
            {
                var casesActual = sequenceElement.Value.Select(x => x.Cases).ToList();
                CollectionAssert.AreEqual(casesExpected, casesActual);
            }
        }
        #endregion

        #region PrepareForAlgorithm_C50ShouldSortInErrorRatioIncreasingOrder
        [TestMethod]
        public void PrepareForAlgorithm_C50ShouldSortInErrorRatioIncreasingOrder()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C50);

            var casesExpected = new List<int> { 502, 501, 500 };
            foreach (var sequenceElement in _model.StatisticsSequences)
            {
                var casesActual = sequenceElement.Value.Select(x => x.Cases).ToList();
                CollectionAssert.AreEqual(casesExpected, casesActual);
            }
        }
        #endregion

        #region PrepareForAlgorithm_ShouldSetChosenAlgorithm
        [TestMethod]
        public void PrepareForAlgorithm_ShouldSetChosenAlgorithm()
        {
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);

            Assert.AreEqual(DecisionTreeAlgorithm.C45, _model.Algorithm);
        }
        #endregion

        #region PrepareForAlgorithm_ShouldSetChosenAlgorithmForForexTradingAgentService
        [TestMethod]
        public void PrepareForAlgorithm_ShouldSetChosenAlgorithmForForexTradingAgentService()
        {
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);

            _forexTradingAgentServiceMock
                .VerifySet(x => x.Algorithm = DecisionTreeAlgorithm.C45, Times.Once());
        }
        #endregion

        #endregion

        #region Trade Tests

        #region Trade_NotInitialized_ShouldThrowInvalidOperationException
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "You must initialize trading model at first.")]
        public void Trade_NotInitialized_ShouldThrowInvalidOperationException()
        {
            _model.Trade();
        }
        #endregion

        #region Trade_InitializedButNotPreparedForAlgorithm_ShouldThrowInvalidOperationException
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "You must prepare for chosen algorithm before trading.")]
        public void Trade_InitializedButNotPreparedForAlgorithm_ShouldThrowInvalidOperationException()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, string.Empty);
            _model.Trade();
        }
        #endregion

        #region Trade_InitializedPreparedButNoQuantitiesSet_ShouldThrowInvalidOperationException
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "You must set bid size, and margin to start modeling.")]
        public void Trade_InitializedPreparedButNoQuantitiesSet_ShouldThrowInvalidOperationException()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, string.Empty);
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.Trade();
        }
        #endregion

        #region Trade_ShouldTraverseThroughAllMarketActions
        [TestMethod]
        public void Trade_ShouldTraverseThroughAllMarketActions()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, string.Empty);
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            Assert.AreEqual(720, _nextRecordCallsOverall);
        }
        #endregion

        #region Trade_ShouldInitializeForexTradingAgentServiceWithCorrectPath
        [TestMethod]
        public void Trade_ShouldInitializeForexTradingAgentServiceWithCorrectPath()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexTradingAgentServiceMock
                .Verify(x => x.Initialize("ForexTrees\\EURUSD\\2014"), Times.Exactly(144));
        }
        #endregion

        #region Trade_ShouldInitializeForexTradingAgentServiceWithCorrectStartingPeriods
        [TestMethod]
        public void Trade_ShouldInitializeForexTradingAgentServiceWithCorrectStartingPeriods()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            foreach (var period in _periods)
            {
                var periodSet = period;
                _forexTradingAgentServiceMock
                    .VerifySet(x => x.Period = periodSet, Times.Exactly(12));
            }

        }
        #endregion

        #region Trade_ShouldInitializeForexMarketServiceWithCorrectStartingPeriods
        [TestMethod]
        public void Trade_ShouldInitializeForexMarketServiceWithCorrectStartingPeriods()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            foreach (var period in _periods)
            {
                var periodSet = period;
                _forexMarketServiceMock
                    .VerifySet(x => x.Period = periodSet, Times.Exactly(12));
            }

        }
        #endregion

        #region Trade_ShouldInitializeForexTradingAgentServiceWithCorrectStartingMonths
        [TestMethod]
        public void Trade_ShouldInitializeForexTradingAgentServiceWithCorrectStartingMonths()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            for (var month = 1; month <= 12; month++)
            {
                var monthSet = month;
                _forexTradingAgentServiceMock
                    .VerifySet(x => x.StartingMonth = monthSet, Times.Exactly(4));
            }

        }
        #endregion

        #region Trade_ShouldInitializeForexMarketServiceWithCorrectStartingMonths
        [TestMethod]
        public void Trade_ShouldInitializeForexMarketServiceWithCorrectStartingMonths()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            for (var month = 1; month <= 12; month++)
            {
                var monthSet = month;
                _forexMarketServiceMock
                    .VerifySet(x => x.StartingMonth = monthSet, Times.Exactly(4));
            }

        }
        #endregion

        #region Trade_ShouldInitializeForexTradingAgentServiceWithCorrectStartingChunks
        [TestMethod]
        public void Trade_ShouldInitializeForexTradingAgentServiceWithCorrectStartingChunks()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexTradingAgentServiceMock
                .VerifySet(x => x.StartingChunk = 4, Times.Exactly(48));

            _forexTradingAgentServiceMock
                .VerifySet(x => x.StartingChunk = 2, Times.Exactly(48));

            _forexTradingAgentServiceMock
                .VerifySet(x => x.StartingChunk = 0, Times.Exactly(48));

        }
        #endregion

        #region Trade_ShouldInitializeForexMarketServiceWithCorrectStartingChunks
        [TestMethod]
        public void Trade_ShouldInitializeForexMarketServiceWithCorrectStartingChunks()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexMarketServiceMock
                .VerifySet(x => x.StartingChunk = 4, Times.Exactly(48));

            _forexMarketServiceMock
                .VerifySet(x => x.StartingChunk = 2, Times.Exactly(48));

            _forexMarketServiceMock
                .VerifySet(x => x.StartingChunk = 0, Times.Exactly(48));

        }
        #endregion

        #region Trade_ShouldClearTradingInMarket
        [TestMethod]
        public void Trade_ShouldClearTradingInMarket()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexMarketServiceMock
                .Verify(x => x.Clear(), Times.Exactly(144));
        }
        #endregion

        #region Trade_ShouldClassifyRecords
        [TestMethod]
        public void Trade_ShouldClassifyRecords()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexTradingAgentServiceMock
                .Verify(x => x.ClassifyRecord(It.IsAny<ForexTreeData>()), Times.Exactly(720));
        }
        #endregion

        #region Trade_ShouldAddToReppository
        [TestMethod]
        public void Trade_ShouldAddToReppository()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexTradingServiceMock
                .Verify(x => x.AddToRepository(), Times.Exactly(144));
        }
        #endregion

        #region Trade_ShouldCommitToRepository
        [TestMethod]
        public void Trade_ShouldCommitToRepository()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            foreach (var path in from sequenceElement in _model.StatisticsSequences
                                        let elementKey = sequenceElement.Key
                                        let month = elementKey.Key
                                        let period = elementKey.Value
                                        let sequences = sequenceElement.Value
                                 from sequence in sequences
                                        let chunk = sequence.Chunk
                                 select string.Format("ForexTrees\\EURUSD\\2014\\TradingResults\\P{0}M{1}CH{2}AC45.csv", period, month, chunk))
            {
                var saveFilePath = path;
                _forexTradingServiceMock
                    .Verify(x => x.CommitToRepository(saveFilePath), Times.Once());
            }
        }
        #endregion

        #region Trade_ShouldRecalculateBalanceOnBuy
        [TestMethod]
        public void Trade_ShouldRecalculateBalanceOnBuy()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();


            Assert.AreEqual(90000.0, _model.Balance);
        }
        #endregion

        #region Trade_ShouldKeepTrackOfBalance_CorrectBalance
        [TestMethod]
        public void Trade_ShouldKeepTrackOfBalance_CorrectBalance()
        {

            var profits = new List<double> { 10.0 };

            _forexTradingAgentServiceMock
                .Setup(x => x.ClassifyRecord(It.IsAny<ForexTreeData>()))
                .Returns<ForexTreeData>(x => profits.Count % 2 == 0 ? MarketAction.Sell : MarketAction.Buy)
                .Callback(() => profits.Add(10.0));

            _forexTradingServiceMock
                .Setup(x => x.Profits)
                .Returns(profits);

            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexTradingServiceMock
                .Verify(x => x.PlaceBid(It.IsAny<ForexTreeData>(), MarketAction.Sell), Times.Exactly(360));
        }
        #endregion

        #region Trade_ShouldKeepTrackOfBalance_ShouldBreakWhenBalanceIsLessThanBidSizeAfterSell
        [TestMethod]
        public void Trade_ShouldKeepTrackOfBalance_ShouldBreakWhenBalanceIsLessThanBidSizeAfterSell()
        {

            var profits = new List<double> { -100000.0 };

            _forexTradingAgentServiceMock
                .Setup(x => x.ClassifyRecord(It.IsAny<ForexTreeData>()))
                .Returns<ForexTreeData>(x => profits.Count % 2 == 0 ? MarketAction.Sell : MarketAction.Buy)
                .Callback(() => profits.Add(-100000.0));

            _forexTradingServiceMock
                .Setup(x => x.Profits)
                .Returns(profits);

            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities();
            _model.Trade();

            _forexTradingServiceMock
                .Verify(x => x.PlaceBid(It.IsAny<ForexTreeData>(), MarketAction.Sell), Times.Exactly(60));
        }
        #endregion

        #region Trade_ShouldBuy50TimesWhenNoBalanceLeftForBuy
        [TestMethod]
        public void Trade_ShouldBuy50TimesWhenNoBalanceLeftForBuy()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities(2000.0);
            _model.Trade();

            _forexTradingServiceMock
                .Verify(x => x.PlaceBid(It.IsAny<ForexTreeData>(), MarketAction.Buy), Times.Exactly(144));
        }
        #endregion

        #region Trade_ShouldHold670TimesWhenNoBalanceLeftForBuy
        [TestMethod]
        public void Trade_ShouldHold670TimesWhenNoBalanceLeftForBuy()
        {
            _model.Initialize("EURUSD", "2014", _periods, 500, string.Empty, "ForexTrees");
            _model.PrepareForAlgorithm(DecisionTreeAlgorithm.C45);
            _model.SetModelQuantities(0.0);
            _model.Trade();

            _forexTradingServiceMock
                .Verify(x => x.PlaceBid(It.IsAny<ForexTreeData>(), MarketAction.Hold), Times.Exactly(720));
        }
        #endregion

        #endregion

        #endregion

    }
}
