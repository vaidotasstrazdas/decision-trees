#region Usings
using System.Collections.Generic;
using System.Text;

using Bridge.IBLL.Data;
using Bridge.IBLL.Data.Base;
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
                    yahooTreeData.Add(new YahooTreeData
                    {
                        Volatility = record.Volatility,
                        Action = MarketAction.Hold
                    });
                    break;
                }
                var nextRecord = yahooRecords[i + 1];
                if (record.Close < nextRecord.Close)
                {
                    yahooTreeData.Add(new YahooTreeData
                    {
                        Volatility = record.Volatility,
                        Action = MarketAction.Buy
                    });
                    yahooTreeData.Add(new YahooTreeData
                    {
                        Volatility = nextRecord.Volatility,
                        Action = MarketAction.Sell
                    });
                    i += 1;
                }
                else
                {
                    yahooTreeData.Add(new YahooTreeData
                    {
                        Volatility = record.Volatility,
                        Action = MarketAction.Hold
                    });
                }
            }

            return yahooTreeData;
        }

        public static string BuildYahooNamesFile()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Buy,Sell,Hold.	|classes");
            builder.AppendLine();
            builder.AppendLine("Volatility:	continuous.");

            return builder.ToString();
        }

    }
}
