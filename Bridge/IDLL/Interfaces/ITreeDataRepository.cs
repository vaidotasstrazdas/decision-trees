using System.Collections.Generic;

namespace Bridge.IDLL.Interfaces
{
    public interface ITreeDataRepository<in TRecord>
    {

        string CollectionName { get; set; }
        string Path { get; set; }
        string NamesFileContents { get; set; }

        void SaveData(IEnumerable<TRecord> records);

    }
}
