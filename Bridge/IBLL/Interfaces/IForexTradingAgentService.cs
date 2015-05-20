#region Usings
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces.Base;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IForexTradingAgentService : IForexBaseService
    {

        DecisionTreeAlgorithm Algorithm { get; set; }

        void Initialize(string path);
        MarketAction ClassifyRecord(ForexTreeData forexRecord);

    }
}
