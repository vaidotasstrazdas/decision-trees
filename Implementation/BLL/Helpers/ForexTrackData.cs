namespace Implementation.BLL.Helpers
{
    public class ForexTrackData
    {

        public int CurrentRecord { get; set; }

        public double PreviousBid { get; set; }
        public double PreviousAsk { get; set; }
        public double PreviousSpread { get; set; }
        public double NextBid { get; set; }
        public double NextAsk { get; set; }
        public double NextSpread { get; set; }
        public double BidMean { get; set; }
        public double AskMean { get; set; }
        public double SpreadMean { get; set; }
        public double BidVariance { get; set; }
        public double AskVariance { get; set; }
        public double SpreadVariance { get; set; }

    }
}
