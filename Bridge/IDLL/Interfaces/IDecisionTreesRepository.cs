#region Usings
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Bridge.IDLL.Interfaces
{
    public interface IDecisionTreesRepository
    {

        string DecisionTreesPath { get; set; }
        string ReadSource(string period, int month, int chunk, DecisionTreeAlgorithm algorithm);

    }
}
