using System.Collections.Generic;
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces.Base;

namespace Bridge.IBLL.Interfaces
{
    public interface IForexService : IService
    {

        void PrepareData(int recalculationPeriodSeconds, int splitPeriodSeconds);
        void SaveForexData(IList<ForexDto> forexRecords, string path);

    }
}
