using System;
using System.Text;
using Bridge.IBLL.Data;
using Bridge.IBLL.Data.Base;
using Bridge.IDLL.Data;

namespace Implementation.BLL.Helpers
{
    public static class ForexHelper
    {

        public static ForexTreeData BuildForexTreeRecord(ForexRecord record, ForexTrackData options)
        {
            var spread = record.Bid - record.Ask;

            var prevSize = options.CurrentRecord - 1;
            
            options.BidMean = (prevSize * options.BidMean + record.Bid) / options.CurrentRecord;
            options.AskMean = (prevSize * options.AskMean + record.Ask) / options.CurrentRecord;
            options.SpreadMean = (prevSize * options.SpreadMean + spread) / options.CurrentRecord;

            if (prevSize > 0)
            {
                var differenceBid = record.Bid - options.BidMean;
                options.BidVariance = (double)prevSize / options.CurrentRecord * options.BidVariance + 1.0 / prevSize * differenceBid * differenceBid;

                var differenceAsk = record.Ask - options.AskMean;
                options.AskVariance = (double)prevSize / options.CurrentRecord * options.AskVariance + 1.0 / prevSize * differenceAsk * differenceAsk;

                var differenceSpread = spread - options.SpreadMean;
                options.SpreadVariance = (double)prevSize / options.CurrentRecord * options.SpreadVariance + 1.0 / prevSize * differenceSpread * differenceSpread;
            }

            var forexTreeData = new ForexTreeData
            {
                Bid = record.Bid,
                Ask = record.Ask,
                Spread = MathHelpers.PreservePrecision(spread),

                BidChange = options.PreviousBid < 0.0 ? 0.0 : MathHelpers.PreservePrecision(record.Bid / options.PreviousBid - 1),
                AskChange = options.PreviousAsk < 0.0 ? 0.0 : MathHelpers.PreservePrecision(record.Ask / options.PreviousAsk - 1),
                SpreadChange = options.PreviousSpread > 0.0 ? 0.0 : MathHelpers.PreservePrecision(spread / options.PreviousSpread - 1),

                BidStandardDeviation = MathHelpers.PreservePrecision(Math.Sqrt(options.BidVariance)),
                AskStandardDeviation = MathHelpers.PreservePrecision(Math.Sqrt(options.AskVariance)),
                SpreadStandardDeviation = MathHelpers.PreservePrecision(Math.Sqrt(options.SpreadVariance)),

                BidMovingAverage = MathHelpers.PreservePrecision(options.BidMean),
                AskMovingAverage = MathHelpers.PreservePrecision(options.AskMean),
                SpreadMovingAverage = MathHelpers.PreservePrecision(options.SpreadMean)
            };

            options.PreviousBid = record.Bid;
            options.PreviousAsk = record.Ask;
            options.PreviousSpread = spread;

            return forexTreeData;
        }

        public static string BuildForexNamesFile()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Buy,Sell,Hold.	|classes");
            builder.AppendLine();
            builder.AppendLine("Spread:	continuous.");
            builder.AppendLine("Change:	continuous.");
            builder.AppendLine("Volatility:	continuous.");

            return builder.ToString();
        }

        public static void SetCorrectMarketActions(ForexDto currentDto)
        {
            const int ticks = 30;
            var forexData = currentDto.ForexData;
            var to = forexData.Count - ticks;
            for (var i = 0; i < to; i++)
            {
                var record = forexData[i];
                if (record.Action != default(MarketAction))
                {
                    continue;
                }
                var baseBid = record.Bid;
                var maxDifference = double.MinValue;
                var maxIndex = -1;
                for (var j = 1; j < ticks; j++)
                {
                    var index = i + j;
                    var ask = forexData[index].Ask;
                    var difference = baseBid - ask;
                    if (difference > maxDifference)
                    {
                        maxDifference = difference;
                        maxIndex = index;
                    }
                }
                if (maxIndex > -1)
                {
                    record.Action = MarketAction.Buy;
                    forexData[maxIndex].Action = MarketAction.Sell;
                }
                else
                {
                    record.Action = MarketAction.Hold;
                }
            }
        }

        public static ForexTrackData InitializeForexTrackData(ForexRecord record, ForexTrackData trackData = null)
        {
            return new ForexTrackData
            {
                CurrentRecord = 1,
                AskMean = record.Ask,
                BidMean = record.Bid,
                SpreadMean = record.Ask - record.Bid,
                AskVariance = 0.0,
                BidVariance = 0.0,
                SpreadVariance = 0.0,
                PreviousBid = trackData == null ? -1.0 : trackData.PreviousBid,
                PreviousAsk = trackData == null ? -1.0 : trackData.PreviousAsk,
                PreviousSpread = trackData == null ? 1.0 : trackData.PreviousSpread
            };
        }

    }
}
