#region Usings
using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Interfaces;
using Shared.DecisionTrees.DataStructure;
using Shared.DecisionTrees.Interfaces;
#endregion

namespace Implementation.BLL
{
    public class ForexTradingAgentService : IForexTradingAgentService
    {

        #region Private Fields
        private readonly IDecisionTreesRepository _decisionTreesRepository;
        private readonly IDecisionTree<ForexTreeData> _decisionTree;
        private bool _initialised;
        #endregion

        #region Constructors and Destructors
        public ForexTradingAgentService(
            IDecisionTreesRepository decisionTreesRepository,
            IDecisionTree<ForexTreeData> decisionTree)
        {
            _decisionTreesRepository = decisionTreesRepository;
            _decisionTree = decisionTree;
        }
        #endregion

        #region Implemented Interfaces

        #region IForexBaseService
        public string Period { get; set; }
        public int StartingMonth { get; set; }
        public int StartingChunk { get; set; }
        #endregion

        #region IForexTradingAgentService
        public DecisionTreeAlgorithm Algorithm { get; set; }

        public void Initialize(string path)
        {
            _initialised = true;
            _decisionTreesRepository.DecisionTreesPath = path;

            var source = _decisionTreesRepository.ReadSource(Period, StartingMonth, StartingChunk, Algorithm);
            _decisionTree.SaveDecisionTree(source);
        }

        public MarketAction ClassifyRecord(ForexTreeData forexRecord)
        {
            if (!_initialised)
            {
                throw new BllException("Can't deduce market action because agent is not initialised.");
            }

            return _decisionTree.ClassifyRecord(forexRecord);
        }
        #endregion

        #endregion

    }
}
