using System.Collections.Generic;

namespace Shared.DecisionTrees.Interfaces
{
    public interface IDecisionTreeReader
    {

        Dictionary<string, string> SubTrees { get; }
        string NormalizeTree(string treeSource);
        void ReadSubTrees(string treeSource);

    }
}
