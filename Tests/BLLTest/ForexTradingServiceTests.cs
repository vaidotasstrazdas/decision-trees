#region Usings
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bridge.IBLL.Exceptions;
using Implementation.BLL;
using Shared.DecisionTrees.DataStructure;
using Tests.BLLTest.Helpers;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class ForexTradingServiceTests
    {

        #region Private Fields
        private ForexTradingService _service;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _service = new ForexTradingService();
            FakeTradingAgent.Service = _service;
        }
        #endregion

        #region TestSuite

        #region PlaceBid Tests

        #region PlaceBid_BuyAction_ShouldSaveBuyQuantity
        [TestMethod]
        public void PlaceBid_BuyAction_ShouldSaveBuyQuantity()
        {
            FakeTradingAgent.SimpleBuy();

            Assert.AreEqual(111110.0, _service.BuyQuantities[0]);
        }
        #endregion

        #region PlaceBid_IncorrectAction_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Incorrect market action.")]
        public void PlaceBid_IncorrectAction_ShouldThrowBllException()
        {
            _service.PlaceBid(null, default(MarketAction));
        }
        #endregion

        #region PlaceBid_SellActionNoBuyActionsBefore_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "You have no open positions to close for sell.")]
        public void PlaceBid_SellActionNoBuyActionsBefore_ShouldThrowBllException()
        {
            FakeTradingAgent.SimpleSell();
        }
        #endregion

        #region PlaceBid_SellAction_ShouldSaveProfit
        [TestMethod]
        public void PlaceBid_SellAction_ShouldSaveProfit()
        {
            FakeTradingAgent.SimpleBuySell();

            Assert.AreEqual(27.0, _service.Profits[0]);
        }
        #endregion

        #region PlaceBid_SellAction_ShouldRemoveBuyQuantity
        [TestMethod]
        public void PlaceBid_SellAction_ShouldRemoveBuyQuantity()
        {
            FakeTradingAgent.SimpleBuySell();

            Assert.AreEqual(0, _service.BuyQuantities.Count);
        }
        #endregion

        #region PlaceBid_TradeSequence_ShouldCalculateCorrectProfits
        [TestMethod]
        public void PlaceBid_TradeSequence_ShouldCalculateCorrectProfits()
        {
            FakeTradingAgent.TradeSequence();

            var profitsExpected = new List<double> { 27.0, 81.05, 27.0, 27.0, -53.98 };
            var profitsActual = _service.Profits;

            CollectionAssert.AreEqual(profitsExpected, profitsActual);
        }
        #endregion

        #region PlaceBid_TradeSequence_ShouldGetCorrectCountOfRecordsInTradeLog
        [TestMethod]
        public void PlaceBid_TradeSequence_ShouldGetCorrectCountOfRecordsInTradeLog()
        {
            FakeTradingAgent.TradeSequence();

            Assert.AreEqual(10, _service.TradeLog.Count);
        }
        #endregion

        #region PlaceBid_TradeSequence_ShouldGetCorrectQuantitiesBought
        [TestMethod]
        public void PlaceBid_TradeSequence_ShouldGetCorrectQuantitiesBought()
        {
            FakeTradingAgent.TradeSequence();

            var quantitiesExpected = new List<double> { 111110.0, 0.0, 111130.0, 111110.0, 111110.0, 0.0, 0.0, 0.0, 111110.0, 0.0 };
            var quantitiesActual = _service.TradeLog.Select(x => x.QuantityBought).ToList();

            CollectionAssert.AreEqual(quantitiesExpected, quantitiesActual);
        }
        #endregion

        #region PlaceBid_TradeSequence_ShouldGetCorrectQuantitiesSold
        [TestMethod]
        public void PlaceBid_TradeSequence_ShouldGetCorrectQuantitiesSold()
        {
            FakeTradingAgent.TradeSequence();

            var quantitiesExpected = new List<double> { 0.0, 111110.0, 0.0, 0.0, 0.0, 111130.0, 111110.0, 111110.0, 0.0, 111110.0 };
            var quantitiesActual = _service.TradeLog.Select(x => x.QuantitySold).ToList();

            CollectionAssert.AreEqual(quantitiesExpected, quantitiesActual);
        }
        #endregion

        #region PlaceBid_TradeLog_ShouldGetCorrectProfits
        [TestMethod]
        public void PlaceBid_TradeLog_ShouldGetCorrectProfits()
        {
            FakeTradingAgent.TradeSequence();

            var profitsExpected = new List<double> { 0.0, 27.0, 0.0, 0.0, 0.0, 81.05, 27.0, 27.0, 0.0, -53.98 };
            var profitsActual = _service.TradeLog.Select(x => x.Profit).ToList();

            CollectionAssert.AreEqual(profitsExpected, profitsActual);
        }
        #endregion

        #region PlaceBid_TradeLog_ShouldGetCorrectActionsExecuted
        [TestMethod]
        public void PlaceBid_TradeLog_ShouldGetCorrectActionsExecuted()
        {
            FakeTradingAgent.TradeSequence();

            var actionsExpected = new List<MarketAction> { MarketAction.Buy, MarketAction.Sell, MarketAction.Buy, MarketAction.Buy, MarketAction.Buy, MarketAction.Sell, MarketAction.Sell, MarketAction.Sell, MarketAction.Buy, MarketAction.Sell };
            var actionsActual = _service.TradeLog.Select(x => x.ExecutedAction).ToList();

            CollectionAssert.AreEqual(actionsExpected, actionsActual);
        }
        #endregion

        #region PlaceBid_TradeLog_ShouldGetCorrectActionsActual
        [TestMethod]
        public void PlaceBid_TradeLog_ShouldGetCorrectActionsActual()
        {
            FakeTradingAgent.TradeSequence();

            var actionsExpected = new List<MarketAction> { MarketAction.Buy, MarketAction.Sell, MarketAction.Buy, MarketAction.Buy, MarketAction.Buy, MarketAction.Sell, MarketAction.Sell, MarketAction.Sell, MarketAction.Hold, MarketAction.Hold };
            var actionsActual = _service.TradeLog.Select(x => x.CorrectAction).ToList();

            CollectionAssert.AreEqual(actionsExpected, actionsActual);
        }
        #endregion

        #endregion

        #endregion

    }
}
