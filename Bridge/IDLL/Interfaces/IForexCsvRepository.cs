using Bridge.IDLL.Data;

namespace Bridge.IDLL.Interfaces
{
    public interface IForexCsvRepository : ICsvDataRepository<ForexRecord>
    {

        void NormalizeData();

    }
}
