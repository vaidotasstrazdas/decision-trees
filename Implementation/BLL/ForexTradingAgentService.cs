using Bridge.IBLL.Interfaces;
using Shared.DecisionTrees.Interfaces;

namespace Implementation.BLL
{
    public class ForexTradingAgentService : IForexTradingAgentService
    {

        private readonly IDecisionTreeReader _decisionTreeReader;

        public ForexTradingAgentService(IDecisionTreeReader decisionTreeReader)
        {
            _decisionTreeReader = decisionTreeReader;
        }

        public string Period { get; set; }
        public int StartingMonth { get; set; }
        public int StartingChunk { get; set; }

    }
}
