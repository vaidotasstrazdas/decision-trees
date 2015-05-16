using System.Collections.Generic;

namespace Shared.DecisionTrees.Interfaces
{
    public interface IDecisionTreeReader
    {

        Dictionary<string, string> SubTrees { get; }

        string NormalizeTreeSource(string treeSource);
        string NormalizeTree(string treeSource);
        void ReadSubTrees(string treeSource);

    }
}
