using System.Collections.Generic;
using Bridge.IDLL.Data;

namespace Tests.BLLTest.DataBuilders
{
    public class ListOfStatisticsRecords
    {
        private readonly List<StatisticsRecord> _list = new List<StatisticsRecord>();

        public List<StatisticsRecord> Build()
        {
            return _list;
        }

        public ListOfStatisticsRecords AddRecord(string algorithm, int cases, int errors, int chunk)
        {

            _list.Add(new StatisticsRecord
            {
                Algorithm = algorithm,
                Cases = cases,
                Errors = errors,
                Chunk = chunk
            });

            return this;
        }

    }
}
