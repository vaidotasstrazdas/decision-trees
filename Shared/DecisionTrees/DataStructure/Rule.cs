namespace Shared.DecisionTrees.DataStructure
{
    public class Rule
    {

        public string Property { get; set; }
        public double Value { get; set; }
        public RelationType Relation { get; set; }
        public int Level { get; set; }
        public Rule LessOrEqualRule { get; set; }
        public Rule GreaterRule { get; set; }
        public MarketAction Action { get; set; }

    }
}
