#region Usings
using Bridge.IDLL.Data;
#endregion

namespace Bridge.IDLL.Interfaces
{
    public interface IStatisticsResultsRepository
    {

        void Add(StatisticsResult statistics);
        void Save(string path);

    }
}
