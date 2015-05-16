#region Usings
using System.Collections.Generic;
using System.Text;

using Bridge.IBLL.Data;
using Bridge.IDLL.Data;
using Shared.DecisionTrees.DataStructure;

#endregion

namespace Implementation.BLL.Helpers
{
    public static class YahooHelper
    {

        public static IEnumerable<YahooTreeData> BuildYahooTreeDataList(IList<YahooNormalized> yahooRecords)
        {
            var yahooTreeData = new List<YahooTreeData>();

            for (var i = 0; i < yahooRecords.Count; i++)
            {
                var record = yahooRecords[i];
                if (i + 1 >= yahooRecords.Count)
                {
                    yahooTreeData.Add(BuildYahooTreeData(record, MarketAction.Hold));
                    break;
                }
                var nextRecord = yahooRecords[i + 1];
                if (record.Close < nextRecord.Close)
                {
                    yahooTreeData.Add(BuildYahooTreeData(record, MarketAction.Buy));
                    yahooTreeData.Add(BuildYahooTreeData(nextRecord, MarketAction.Sell));
                    i += 1;
                }
                else
                {
                    yahooTreeData.Add(BuildYahooTreeData(record, MarketAction.Hold));
                }
            }

            return yahooTreeData;
        }

        public static string BuildYahooNamesFile()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Buy,Sell,Hold.	|classes");
            builder.AppendLine();
            builder.AppendLine("Spread:	continuous.");
            builder.AppendLine("Change:	continuous.");
            builder.AppendLine("Volatility:	continuous.");

            return builder.ToString();
        }

        public static YahooNormalized BuildYahooNormalized(YahooRecord record, double change, double movingAverage, double volatility)
        {
            return new YahooNormalized
            {
                Date = record.Date,
                Open = record.Open,
                High = record.High,
                Low = record.Low,
                Close = record.Close,
                Volume = record.Volume,
                Change = change,
                MovingAverage = movingAverage,
                Volatility = volatility
            };
        }

        private static YahooTreeData BuildYahooTreeData(YahooNormalized record, MarketAction action)
        {
            return new YahooTreeData
            {
                Spread = record.High - record.Low,
                Volatility = record.Volatility,
                Change = record.Change,
                Action = action
            };
        }

    }
}
