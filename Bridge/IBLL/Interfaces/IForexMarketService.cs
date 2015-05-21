#region Usings
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces.Base;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IForexMarketService : IForexBaseService
    {

        void SetForexTreesPath(string forexTreesPath);
        bool IsDone();
        ForexTreeData NextRecord();
        void Clear();

    }
}
