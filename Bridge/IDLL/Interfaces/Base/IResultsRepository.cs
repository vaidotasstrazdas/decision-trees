namespace Bridge.IDLL.Interfaces.Base
{
    public interface IResultsRepository<in TObject>
    {

        void Add(TObject statistics);
        void Save(string path);

    }
}
