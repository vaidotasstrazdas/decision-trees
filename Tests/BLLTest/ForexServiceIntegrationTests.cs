#region Usings
using System.Collections.Generic;
using System.Linq;
using Bridge.IBLL.Data;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Implementation.BLL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.DecisionTrees.DataStructure;
using Tests.BLLTest.DataBuilders;
#endregion

namespace Tests.BLLTest
{
    public class ForexServiceIntegrationTests
    {

        #region Private Fields
        private Mock<IForexCsvRepository> _forexCsvRepositoryMock;
        private Mock<ITreeDataRepository<ForexTreeData>> _treeRepositoryMock;
        private ForexService _service;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {

            _forexCsvRepositoryMock = new Mock<IForexCsvRepository>();
            _treeRepositoryMock = new Mock<ITreeDataRepository<ForexTreeData>>();

            var forexRecords = ForexRecordsBuilder.BuildRecords();
            _forexCsvRepositoryMock
                .Setup(x => x.CsvLinesNormalized)
                .Returns(forexRecords);

            _service = new ForexService(
                _forexCsvRepositoryMock.Object,
                _treeRepositoryMock.Object
            );
        }
        #endregion

        #region TestSuite

        #region PrepareData Tests

        #region PrepareData_Split1800Provided_BuyAndSellQuantityMustBeTheSame
        [TestMethod]
        [TestCategory("Integration")]
        public void PrepareData_Split1800Provided_BuyAndSellQuantityMustBeTheSame()
        {
            var data = _service.PrepareData(1800);

            var allActions = data[0].ForexData.Select(x => x.Action.ToString()).ToList();
            var buyActions = allActions.Count(x => x == "Buy");
            var sellActions = allActions.Count(x => x == "Sell");

            Assert.AreEqual(buyActions, sellActions);
        }
        #endregion

        #region PrepareData_Split1800ProvidedAndMiniTradingMade_ShouldMakeProfit
        [TestMethod]
        [TestCategory("Integration")]
        public void PrepareData_Split1800ProvidedAndMiniTradingMade_ShouldMakeProfit()
        {
            var data = _service.PrepareData(1800);
            var forexTreeData = data[0].ForexData;

            const double spending = 10000.0;
            const double margin = 0.02;
            const double leverage = 1.0 / margin;
            var eurosSpent = 0.0;
            var profit = 0.0;
            List<double> dollarsList = new List<double>();
            foreach (var record in forexTreeData)
            {
                double dollars;
                switch (record.Action)
                {
                    case MarketAction.Hold:
                        continue;
                    case MarketAction.Buy:
                        eurosSpent += spending;
                        dollars = spending * leverage * record.Bid;
                        dollarsList.Add(MathHelpers.PreservePrecision(dollars));
                        continue;
                }

                dollars = dollarsList.First();
                dollarsList.RemoveAt(0);
                profit += dollars / record.Ask - spending * leverage;
                profit = MathHelpers.CurrencyPrecision(profit);
            }

            if (profit < 0)
            {
                Assert.Fail();
            }

            var eurosNow = eurosSpent + profit;

            Assert.AreEqual(460000.0, eurosSpent);
            Assert.AreEqual(462671.64, eurosNow);
        }
        #endregion

        #endregion

        #endregion

    }
}
