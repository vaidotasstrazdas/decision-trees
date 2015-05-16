#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.DecisionTrees;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Tests.DecisionTreesTest
{
    [TestClass]
    public class RuleBuilderTests
    {

        #region Private Fields
        private RuleBuilder _builder;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _builder = new RuleBuilder();
        }
        #endregion

        #region TestSuite

        #region Read Tests

        #region Read_RootSplitRuleProvided_ShouldReadCorrectLevel
        [TestMethod]
        public void Read_RootSplitRuleProvided_ShouldReadCorrectLevel()
        {
            var rule = _builder.Read("Bid <= 1.2622:");

            Assert.AreEqual(0, rule.Level);
        }
        #endregion

        #region Read_LevelOneSplitRuleProvidedWithFinalGroup_ShouldReadCorrectLevel
        [TestMethod]
        public void Read_LevelOneSplitRuleProvidedWithFinalGroup_ShouldReadCorrectLevel()
        {
            var rule = _builder.Read("    Ask > 1.2622:Hold (244.0/9.6)");

            Assert.AreEqual(1, rule.Level);
        }
        #endregion

        #region Read_LevelOneSplitRuleProvidedWithoutFinalGroup_ShouldReadCorrectLevel
        [TestMethod]
        public void Read_LevelOneSplitRuleProvidedWithoutFinalGroup_ShouldReadCorrectLevel()
        {
            var rule = _builder.Read("    Ask <= 1.2622:");

            Assert.AreEqual(1, rule.Level);
        }
        #endregion

        #region Read_LevelSixSplitRuleProvidedShouldReadCorrectLevel
        [TestMethod]
        public void Read_LevelSixSplitRuleProvidedShouldReadCorrectLevel()
        {
            var rule = _builder.Read("                        Ask <= 1.26208:Buy (4.0/1.2)");

            Assert.AreEqual(6, rule.Level);
        }
        #endregion

        #region Read_BidPropertyProvided_ShouldReadCorrectProperty
        [TestMethod]
        public void Read_BidPropertyProvided_ShouldReadCorrectProperty()
        {
            var rule = _builder.Read("Bid <= 1.2622 :");

            Assert.AreEqual("Bid", rule.Property);
        }
        #endregion

        #region Read_DifferentDoubleFormatsProvided_ShouldReadCorrectValues
        [TestMethod]
        public void Read_DifferentDoubleFormatsProvided_ShouldReadCorrectValues()
        {
            var ruleOne = _builder.Read("Bid <= 1.5846e-05 :");
            var ruleTwo = _builder.Read("Bid <= 1.26208 :");

            Assert.AreEqual(0.000015846, ruleOne.Value);
            Assert.AreEqual(1.26208, ruleTwo.Value);
        }
        #endregion

        #region Read_NoFinalGroupProvided_ShouldReadDefaultAction
        [TestMethod]
        public void Read_NoFinalGroupProvided_ShouldReadDefaultAction()
        {
            var rule = _builder.Read("Bid <= 1.2622 :");

            Assert.AreEqual(default(MarketAction), rule.Action);
        }
        #endregion

        #region Read_LessOrEqualRelationProvided_ShouldReadCorrectRelation
        [TestMethod]
        public void Read_LessOrEqualRelationProvided_ShouldReadCorrectRelation()
        {
            var rule = _builder.Read("Bid <= 1.5846e-05 :");

            Assert.AreEqual(RelationType.LessOrEqual, rule.Relation);
        }
        #endregion

        #region Read_GreaterRelationProvided_ShouldReadCorrectRelation
        [TestMethod]
        public void Read_GreaterRelationProvided_ShouldReadCorrectRelation()
        {
            var rule = _builder.Read("Bid > 1.5846e-05 :");

            Assert.AreEqual(RelationType.Greater, rule.Relation);
        }
        #endregion

        #region Read_HoldActionProvided_ShouldReadHoldMarketAction
        [TestMethod]
        public void Read_HoldActionProvided_ShouldReadHoldMarketAction()
        {
            var rule = _builder.Read("|   Ask > 1.2622 : Hold (244.0/9.6)");

            Assert.AreEqual(MarketAction.Hold, rule.Action);
        }
        #endregion

        #region Read_SellActionProvided_ShouldReadSellMarketAction
        [TestMethod]
        public void Read_SellActionProvided_ShouldReadSellMarketAction()
        {
            var rule = _builder.Read("|   Ask > 1.2622 : Sell (244.0/9.6)");

            Assert.AreEqual(MarketAction.Sell, rule.Action);
        }
        #endregion

        #region Read_BuyActionProvided_ShouldReadBuyMarketAction
        [TestMethod]
        public void Read_BuyActionProvided_ShouldReadBuyMarketAction()
        {
            var rule = _builder.Read("|   Ask > 1.2622 : Buy (244.0/9.6)");

            Assert.AreEqual(MarketAction.Buy, rule.Action);
        }
        #endregion

        #endregion

        #region MapRules Tests

        #region MapRules_LessOrEqualRelationCurrentRule_ShouldAddToLessOrEqualGroupInParent
        [TestMethod]
        public void MapRules_LessOrEqualRelationCurrentRule_ShouldAddToLessOrEqualGroupInParent()
        {
            var root = _builder.Read("Bid <= 1.2622 :");
            var ruleTwo = _builder.Read("|   Ask <= 1.2622 : Hold (244.0/9.6)");

            _builder.MapRules(root, ruleTwo);

            Assert.AreSame(root.LessOrEqualRule, ruleTwo);
        }
        #endregion

        #region MapRules_GreaterRelationCurrentRule_ShouldAddToGreaterGroupInParent
        [TestMethod]
        public void MapRules_GreaterRelationCurrentRule_ShouldAddToGreaterGroupInParent()
        {
            var root = _builder.Read("Bid <= 1.2622 :");
            var ruleTwo = _builder.Read("|   Ask > 1.2622 : Hold (244.0/9.6)");

            _builder.MapRules(root, ruleTwo);

            Assert.AreSame(root.GreaterRule, ruleTwo);
        }
        #endregion

        #endregion

        #endregion

    }
}
