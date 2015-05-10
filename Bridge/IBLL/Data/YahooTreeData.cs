using Bridge.IBLL.Data.Base;

namespace Bridge.IBLL.Data
{
    public class YahooTreeData
    {

        public double Spread { get; set; }
        public double Change { get; set; }
        public double Volatility { get; set; }
        public MarketAction Action { get; set; }

    }
}
