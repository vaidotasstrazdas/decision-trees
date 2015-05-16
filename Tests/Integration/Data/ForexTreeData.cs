namespace Tests.Integration.Data
{
    public class ForexTreeData
    {

        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Spread { get; set; }

        public double BidChange { get; set; }
        public double AskChange { get; set; }
        public double SpreadChange { get; set; }

        public double BidStandardDeviation { get; set; }
        public double AskStandardDeviation { get; set; }
        public double SpreadStandardDeviation { get; set; }

        public double BidMovingAverage { get; set; }
        public double AskMovingAverage { get; set; }
        public double SpreadMovingAverage { get; set; }
        public string Action { get; set; }

    }
}
