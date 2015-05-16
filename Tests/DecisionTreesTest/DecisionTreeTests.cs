#region Usings
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.DecisionTrees.DataStructure;
using Shared.DecisionTrees.Interfaces;
using Tests.DecisionTreesTest.Data;
#endregion

namespace Tests.DecisionTreesTest
{
    [TestClass]
    public class DecisionTreeTests
    {

        #region Private Fields
        private Mock<IDecisionTreeReader> _decisionTreeReaderMock;
        private Mock<IRuleBuilder> _ruleBuilderMock;
        private Mock<IClassifier<FakeRecord>> _classifierMock;
        private DecisionTree<FakeRecord> _tree;
        private Rule _fakeRule;
        private string _treeString;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _decisionTreeReaderMock = new Mock<IDecisionTreeReader>();
            _ruleBuilderMock = new Mock<IRuleBuilder>();
            _classifierMock = new Mock<IClassifier<FakeRecord>>();

            _decisionTreeReaderMock
                .Setup(x => x.ReadSubTrees(It.IsAny<string>()));

            var treeBuilder = new StringBuilder();
            treeBuilder.AppendLine("Ask <= 1.1:Sell (31.0/1.0)");
            treeBuilder.AppendLine("Ask > 1.1:");
            treeBuilder.AppendLine("    Bid <= 1.2:Buy (123.0/1.0)");
            treeBuilder.AppendLine("    Bid > 1.2:Hold (31.0/1.0)");
            _treeString = treeBuilder.ToString();

            _decisionTreeReaderMock
                .Setup(x => x.NormalizeTreeSource(It.IsAny<string>()))
                .Returns(_treeString);

            _decisionTreeReaderMock
                .Setup(x => x.NormalizeTree(It.IsAny<string>()))
                .Returns(_treeString);

            _fakeRule = new Rule {Level = 0};
            _ruleBuilderMock
                .Setup(x => x.Read(It.IsAny<string>()))
                .Returns(_fakeRule);

            _tree = new DecisionTree<FakeRecord>(
                _decisionTreeReaderMock.Object,
                _ruleBuilderMock.Object,
                _classifierMock.Object);

        }
        #endregion

        #region TestSuite

        #region ClassifyRecord Tests

        #region ClassifyRecord_ShouldCallClassifier
        [TestMethod]
        public void ClassifyRecord_ShouldCallClassifier()
        {
            var record = new FakeRecord();

            _tree.ClassifyRecord(record);

            _classifierMock
                .Verify(x => x.Classify(record, _tree.Root), Times.Once);
        }
        #endregion

        #endregion

        #region SaveDecisionTree Tests

        #region SaveDecisionTree_ShouldMapEveryRule
        [TestMethod]
        public void SaveDecisionTree_ShouldMapEveryRule()
        {
            _tree.SaveDecisionTree(_treeString);

            _ruleBuilderMock
                .Verify(x => x.MapRules(_tree.Root, _fakeRule), Times.Exactly(4));
        }
        #endregion

        #endregion

        #endregion

    }
}
