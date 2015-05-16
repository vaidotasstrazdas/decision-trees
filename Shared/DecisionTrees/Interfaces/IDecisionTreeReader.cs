namespace Shared.DecisionTrees.Interfaces
{
    public interface IDecisionTreeReader
    {

        string NormalizeTree(string treeSource);
        void ReadSubTrees(string[] parts);

    }
}
