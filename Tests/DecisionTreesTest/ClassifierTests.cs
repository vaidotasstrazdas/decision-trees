#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.DecisionTrees;
using Shared.DecisionTrees.DataStructure;
using Tests.DecisionTreesTest.Data;
#endregion

namespace Tests.DecisionTreesTest
{
    [TestClass]
    public class ClassifierTests
    {

        #region Private Fields
        private Classifier<FakeRecord> _classifier;
        private Rule _root;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _classifier = new Classifier<FakeRecord>();
            _root = new Rule
            {
                LessOrEqualRule = new Rule
                {
                    Property = "Ask",
                    Value = 1.1,
                    Relation = RelationType.LessOrEqual,
                    Action = MarketAction.Sell
                },
                GreaterRule = new Rule
                {
                    Property = "Ask",
                    Value = 1.1,
                    Relation = RelationType.Greater,
                    LessOrEqualRule = new Rule
                    {
                        Property = "Bid",
                        Value = 1.2,
                        Relation = RelationType.LessOrEqual,
                        Action = MarketAction.Buy
                    },
                    GreaterRule = new Rule
                    {
                        Property = "Bid",
                        Value = 1.2,
                        Relation = RelationType.Greater,
                        Action = MarketAction.Hold
                    }
                }
            };
        }
        #endregion

        #region TestSuite

        #region Records Tests

        #region Records_ShouldSetCorrectCountOfRecords
        [TestMethod]
        public void Records_ShouldSetCorrectCountOfRecords()
        {
            Assert.AreEqual(2, _classifier.Records.Count);
        }
        #endregion

        #region Records_ShouldSetCorrectKeysForRecords
        [TestMethod]
        public void Records_ShouldSetCorrectKeysForRecords()
        {
            var records = _classifier.Records;
            Assert.IsTrue(records.ContainsKey("Bid"));
            Assert.IsTrue(records.ContainsKey("Ask"));
        }
        #endregion

        #region Records_ShouldSetCorrectInitialValuesForRecords
        [TestMethod]
        public void Records_ShouldSetCorrectInitialValuesForRecords()
        {
            var records = _classifier.Records;

            Assert.AreEqual(0.0, records["Bid"]);
            Assert.AreEqual(0.0, records["Ask"]);
        }
        #endregion

        #endregion

        #region Classify Tests

        #region Classify_AskEquals1Point1Provided_ShouldClassifyAsSell
        [TestMethod]
        public void Classify_AskEquals1Point1Provided_ShouldClassifyAsSell()
        {
            var record = new FakeRecord { Ask = 1.1 };

            var marketAction = _classifier.Classify(record, _root);

            Assert.AreEqual(MarketAction.Sell, marketAction);
        }
        #endregion

        #region Classify_AskLessThan1Point1Provided_ShouldClassifyAsSell
        [TestMethod]
        public void Classify_AskLessThan1Point1Provided_ShouldClassifyAsSell()
        {
            var record = new FakeRecord { Ask = 0.9 };

            var marketAction = _classifier.Classify(record, _root);

            Assert.AreEqual(MarketAction.Sell, marketAction);
        }
        #endregion

        #region Classify_AskGreaterThan1Point1AndBidEquals1Point2Provided_ShouldClassifyAsBuy
        [TestMethod]
        public void Classify_AskGreaterThan1Point1AndBidEquals1Point2Provided_ShouldClassifyAsBuy()
        {
            var record = new FakeRecord { Ask = 1.9, Bid = 1.2 };

            var marketAction = _classifier.Classify(record, _root);

            Assert.AreEqual(MarketAction.Buy, marketAction);
        }
        #endregion

        #region Classify_AskGreaterThan1Point1AndBidLessThan1Point2Provided_ShouldClassifyAsBuy
        [TestMethod]
        public void Classify_AskGreaterThan1Point1AndBidLessThan1Point2Provided_ShouldClassifyAsBuy()
        {
            var record = new FakeRecord { Ask = 1.9, Bid = 0.9 };

            var marketAction = _classifier.Classify(record, _root);

            Assert.AreEqual(MarketAction.Buy, marketAction);
        }
        #endregion

        #region Classify_AskGreaterThan1Point1AndBidGreaterThan1Point2Provided_ShouldClassifyAsHold
        [TestMethod]
        public void Classify_AskGreaterThan1Point1AndBidGreaterThan1Point2Provided_ShouldClassifyAsHold()
        {
            var record = new FakeRecord { Ask = 1.9, Bid = 1.9 };

            var marketAction = _classifier.Classify(record, _root);

            Assert.AreEqual(MarketAction.Hold, marketAction);
        }
        #endregion

        #endregion

        #endregion

    }
}
