using Shared.DecisionTrees.DataStructure;

namespace Shared.DecisionTrees.Interfaces
{
    public interface IRuleBuilder
    {

        Rule Read(string line);
        void MapRules(Rule previousRule, Rule currentRule);

    }
}
