#region Usings
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Shared.DecisionTrees.Interfaces
{
    public interface IDecisionTree<in TRecord>
    {

        Rule Root { get; }

        MarketAction ClassifyRecord(TRecord record);
        void SaveDecisionTree(string rawTree);

    }
}
