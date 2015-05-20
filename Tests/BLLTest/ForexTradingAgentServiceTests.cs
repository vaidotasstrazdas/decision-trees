#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Shared.DecisionTrees.DataStructure;
using Shared.DecisionTrees.Interfaces;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class ForexTradingAgentServiceTests
    {

        #region Private Fields
        private Mock<IDecisionTreesRepository> _decisionTreesRepositoryMock;
        private Mock<IDecisionTree<ForexTreeData>> _decisionTreeMock;
        private ForexTradingAgentService _service;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _decisionTreesRepositoryMock = new Mock<IDecisionTreesRepository>();
            _decisionTreeMock = new Mock<IDecisionTree<ForexTreeData>>();
            _service = new ForexTradingAgentService(_decisionTreesRepositoryMock.Object, _decisionTreeMock.Object)
            {
                Period = "300",
                StartingMonth = 1,
                StartingChunk = 0,
                Algorithm = DecisionTreeAlgorithm.C45
            };
        }
        #endregion

        #region TestSuite

        #region Initialize Tests

        #region Initialize_ShouldSetPathForRepository
        [TestMethod]
        public void Initialize_ShouldSetPathForRepository()
        {
            _service.Initialize("Test");

            _decisionTreesRepositoryMock
                .VerifySet(x => x.DecisionTreesPath = "Test", Times.Once());
        }
        #endregion

        #region Initialize_ShouldReadSourceForDecisionTree
        [TestMethod]
        public void Initialize_ShouldReadSourceForDecisionTree()
        {
            _service.Initialize("Test");

            _decisionTreesRepositoryMock
                .Verify(x => x.ReadSource("300", 1, 0, DecisionTreeAlgorithm.C45), Times.Once());
        }
        #endregion

        #region Initialize_ShouldSaveDecisionTree
        [TestMethod]
        public void Initialize_ShouldSaveDecisionTree()
        {
            _service.Initialize("Test");

            _decisionTreeMock
                .Verify(x => x.SaveDecisionTree(It.IsAny<string>()), Times.Once());
        }
        #endregion

        #endregion

        #region ClassifyRecord Tests

        #region ClassifyRecord_ThrowBeforeInitialize_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Can't deduce market action because agent is not initialised.")]
        public void ClassifyRecord_ThrowBeforeInitialize_ShouldThrowBllException()
        {
            _service.ClassifyRecord(null);
        }
        #endregion

        #region ClassifyRecord_ShouldReturnMarketActionDeducedByDecisionTree
        [TestMethod]
        public void ClassifyRecord_ShouldReturnMarketActionDeducedByDecisionTree()
        {
            _service.Initialize("Test");

            _decisionTreeMock
                .Setup(x => x.ClassifyRecord(It.IsAny<ForexTreeData>()))
                .Returns(MarketAction.Buy);

            var action = _service.ClassifyRecord(null);

            Assert.AreEqual(MarketAction.Buy, action);
        }
        #endregion

        #endregion

        #endregion

    }
}
