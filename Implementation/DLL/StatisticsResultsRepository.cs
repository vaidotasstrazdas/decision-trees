#region Usings
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
#endregion

namespace Implementation.DLL
{
    public class StatisticsResultsRepository : IStatisticsResultsRepository
    {

        #region Private Fields
        private readonly List<StatisticsResult> _results = new List<StatisticsResult>();
        #endregion

        #region Implemented Interfaces

        #region IStatisticsResultsRepository
        public void Add(StatisticsResult statistics)
        {
            _results.Add(statistics);
        }

        public void Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new DalException("Path can not be null or empty.");
            }

            var builder = new StringBuilder();
            builder.AppendLine("BluePrint,C45Better,C50Better,Equal");
            foreach (var line in _results.Select(result => string.Format("{0},{1},{2},{3}", result.BluePrint, result.C45Better, result.C50Better, result.Equal)))
            {
                builder.AppendLine(line);
            }

            File.WriteAllBytes(path, Encoding.UTF8.GetBytes(builder.ToString()));

        }
        #endregion

        #endregion

    }
}
