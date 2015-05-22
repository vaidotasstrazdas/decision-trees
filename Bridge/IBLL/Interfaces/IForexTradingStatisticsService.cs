#region Usings
using System.Collections.Generic;

using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces.Base;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IForexTradingStatisticsService : IService
    {

        List<TradeLogRecord> TradeLog { get; }

        TradeStatistics CalculateTradeStatistics();
        void CleanTradeLog();
        void AddToRepository();
        void CommitToRepository(string path);
        void Clear();

    }
}
