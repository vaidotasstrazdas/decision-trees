using System.Collections.Generic;
using System.IO;
using System.Text;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;

namespace Implementation.DLL.RepositoryBase
{
    public class TreeDataRepository<TRecord> : ITreeDataRepository<TRecord>
    {

        private string _namesFilePath;
        private string _dataFilePath;

        public string CollectionName { get; set; }
        public string Path { get; set; }
        public string NamesFileContents { get; set; }

        public void SaveData(IEnumerable<TRecord> records)
        {
            PrepareDataSpace();

            File.WriteAllBytes(_namesFilePath, Encoding.UTF8.GetBytes(NamesFileContents));

            var type = typeof (TRecord);
            var builder = new StringBuilder();
            foreach (var record in records)
            {
                string prefix = "";
                foreach (var property in type.GetProperties())
                {
                    builder.Append(prefix);
                    prefix = ",";
                    var value = record.GetType().GetProperty(property.Name).GetValue(record, null).ToString();
                    builder.Append(string.Format(value.Contains(",") ? "\"{0}\"" : "{0}", value));
                }
                builder.AppendLine();
            }

            File.WriteAllBytes(_dataFilePath, Encoding.UTF8.GetBytes(builder.ToString()));
        }

        private void PrepareDataSpace()
        {

            if (string.IsNullOrWhiteSpace(CollectionName))
            {
                throw new DalException("Collection name is not provided.");
            }

            if (string.IsNullOrWhiteSpace(Path))
            {
                throw new DalException("Path is not provided.");
            }

            if (string.IsNullOrWhiteSpace(NamesFileContents))
            {
                throw new DalException("Contents for names file is not provided.");
            }

            if (!Directory.Exists(Path))
            {
                throw new DalException(string.Format("Path {0} does not exist.", Path));
            }

            _namesFilePath = Path + "/" + CollectionName + ".names";
            _dataFilePath = Path + "/" + CollectionName + ".data";

        }

    }
}
