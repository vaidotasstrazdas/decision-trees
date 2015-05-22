namespace Bridge.IBLL.Data
{
    public class TradeStatistics
    {

        public int TotalActions { get; set; }
        public int TotalHolds { get; set; }
        public int TotalSells { get; set; }
        public int TotalBuys { get; set; }
        public int Mistakes { get; set; }
        public double TotalProfit { get; set; }
        public int NegativeProfitCloses { get; set; }
        public int PositiveProfitCloses { get; set; }
        public int ZeroProfitCloses { get; set; }

    }
}
