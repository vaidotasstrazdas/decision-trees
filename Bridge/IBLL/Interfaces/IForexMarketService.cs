#region Usings
using Bridge.IBLL.Data;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IForexMarketService
    {

        string Period { get; set; }
        int StartingMonth { get; set; }
        int StartingChunk { get; set; }
        DecisionTreeAlgorithm Algorithm { get; set; }

        bool IsDone();
        ForexTreeData NextRecord();

    }
}
