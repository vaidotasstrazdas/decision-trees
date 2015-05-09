#region Usings
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
#endregion

namespace Implementation.DLL.RepositoryBase
{
    public class TreeDataRepository<TRecord> : ITreeDataRepository<TRecord>
    {

        #region Private Fields
        private string _namesFilePath;
        private string _dataFilePath;
        #endregion

        #region Public Fields
        public string CollectionName { get; set; }
        public string Path { get; set; }
        public string NamesFileContents { get; set; }
        #endregion

        #region Implemented Interfaces

        #region ITreeDataRepository
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
        #endregion

        #endregion

        #region Methods
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
        #endregion

    }
}
