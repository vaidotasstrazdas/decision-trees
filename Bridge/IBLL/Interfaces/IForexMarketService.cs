#region Usings
using Bridge.IBLL.Data;
#endregion

namespace Bridge.IBLL.Interfaces
{
    public interface IForexMarketService
    {

        string Period { get; set; }
        int StartingMonth { get; set; }
        int StartingChunk { get; set; }

        bool IsDone();
        ForexTreeData NextRecord();

    }
}
