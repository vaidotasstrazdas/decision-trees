using System;
using System.Collections.Generic;
using System.Linq;
using IBLL.Data;
using IBLL.Exceptions;
using IBLL.Interfaces;
using IDLL.Data;
using IDLL.Exceptions;
using IDLL.Interfaces;

namespace BLL
{
    public class YahooService : IYahooService
    {
        private readonly ICsvDataRepository<YahooRecord> _yahooDataRepository;

        public YahooService(ICsvDataRepository<YahooRecord> yahooDataRepository)
        {
            _yahooDataRepository = yahooDataRepository;
        }

        public void ReadCsv(string filePath)
        {
            try
            {
                _yahooDataRepository.LoadData(filePath);
                _yahooDataRepository.NormalizeData();
            }
            catch (DalException exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of DAL", exception.Message));
            }
            catch (Exception exception)
            {
                throw new BllException(string.Format("{0}: {1}", "Exception of other origin", exception.Message));
            }
        }

        public List<YahooNormalized> PrepareData()
        {
            var yahooRecords = Enumerable.Reverse(_yahooDataRepository.CsvLinesNormalized).ToList();
            var data = new List<YahooNormalized>();
            var firstRecord = yahooRecords[0];
            double mean = firstRecord.Close;
            double variance = 0.0;
            data.Add(new YahooNormalized
            {
                Date = firstRecord.Date,
                Close = firstRecord.Close,
                Volatility = 0.0
            });

            for (var sampleSize = 2; sampleSize <= yahooRecords.Count; sampleSize++)
            {
                var index = sampleSize - 1;
                var prevSize = sampleSize - 1;
                var record = yahooRecords[index];

                mean = (prevSize * mean + yahooRecords[index].Close) / sampleSize;
                var difference = yahooRecords[index].Close - mean;
                variance = (double) prevSize / sampleSize * variance + 1.0 / prevSize * difference * difference;

                var normalizedRecord = new YahooNormalized
                {
                    Date = record.Date,
                    Close = record.Close,
                    Volatility = Math.Round(Math.Sqrt(variance) * 1000000000) / 1000000000
                };
                data.Add(normalizedRecord);
            }

            return data;
        }

    }
}
