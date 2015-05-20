#region Usings
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces.Base;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IForexMarketService : IForexBaseService
    {

        bool IsDone();
        ForexTreeData NextRecord();

    }
}
