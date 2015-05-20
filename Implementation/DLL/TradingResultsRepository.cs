#region Usings
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Implementation.DLL
{
    public class TradingResultsRepository : ITradingResultsRepository
    {

        #region Private Fields
        private readonly List<TradeLogRecord> _results = new List<TradeLogRecord>();
        #endregion

        #region Implemented Interfaces

        #region ITradingResultsRepository
        public void Add(TradeLogRecord statistics)
        {
            _results.Add(statistics);
        }

        public void Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new DalException("Path can not be null or empty.");
            }

            var builder = new StringBuilder();
            builder.AppendLine("QuantityBought,QuantitySold,Profit,ExecutedAction,CorrectAction");
            foreach (var line in _results.Select(result => string.Format("{0},{1},{2},{3},{4}", result.QuantityBought, result.QuantitySold, result.Profit, result.ExecutedAction, result.CorrectAction)))
            {
                builder.AppendLine(line);
            }

            File.WriteAllBytes(path, Encoding.UTF8.GetBytes(builder.ToString()));
        }

        public void Clear()
        {
            _results.Clear();
        }
        #endregion

        #endregion

    }
}
