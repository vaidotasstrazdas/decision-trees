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
    public class HistogramResultsRepository : IHistogramResultsRepository
    {

        #region Private Fields
        private readonly List<HistogramResult> _results = new List<HistogramResult>();
        #endregion

        #region Implemented Interfaces

        #region IHistogramResultsRepository
        public void Add(HistogramResult statistics)
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
            builder.AppendLine("BluePrint,C45Cases,C50Cases");
            foreach (var line in _results.Select(result => string.Format("{0},{1},{2}", result.BluePrint, result.C45Cases, result.C50Cases)))
            {
                builder.AppendLine(line);
            }

            File.WriteAllBytes(path, Encoding.UTF8.GetBytes(builder.ToString()));
        }
        #endregion

        #endregion

    }
}
