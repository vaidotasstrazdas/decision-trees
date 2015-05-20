using System.Collections.Generic;
using Bridge.IBLL.Data;
using Shared.DecisionTrees.DataStructure;

namespace Bridge.IBLL.Interfaces
{
    public interface IForexTradingService
    {

        double MarginRatio { get; set; }
        double BidSize { get; set; }
        List<double> BuyQuantities { get; }
        List<double> Profits { get; }
        List<TradeLogRecord> TradeLog { get; }

        void PlaceBid(ForexTreeData record, MarketAction action);

    }
}
