using System;

namespace Bridge.IBLL.Data
{
    public class YahooNormalized
    {

        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
        public double Change { get; set; }
        public double MovingAverage { get; set; }
        public double Volatility { get; set; }

    }
}
