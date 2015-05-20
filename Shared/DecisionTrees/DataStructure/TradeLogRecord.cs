namespace Shared.DecisionTrees.DataStructure
{
    public class TradeLogRecord
    {

        public double QuantityBought { get; set; }
        public double QuantitySold { get; set; }
        public double Profit { get; set; }
        public MarketAction ExecutedAction { get; set; }
        public MarketAction CorrectAction { get; set; }

    }
}
