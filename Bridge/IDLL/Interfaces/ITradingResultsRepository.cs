#region Usings
using Bridge.IDLL.Interfaces.Base;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Bridge.IDLL.Interfaces
{
    public interface ITradingResultsRepository : IResultsRepository<TradeLogRecord>
    {
    }
}
