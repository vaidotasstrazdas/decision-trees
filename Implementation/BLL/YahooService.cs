#region Usings
using System;
using System.Collections.Generic;
using System.Linq;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL.Helpers;
#endregion

namespace Implementation.BLL
{
    public class YahooService : IYahooService
    {
        #region Private Fields
        private readonly ICsvDataRepository<YahooRecord> _yahooDataRepository;
        private readonly ITreeDataRepository<YahooTreeData> _yahooTreeDataRepository;
        #endregion

        #region Constructors and Destructors
        public YahooService(
            ICsvDataRepository<YahooRecord> yahooDataRepository,
            ITreeDataRepository<YahooTreeData> yahooTreeDataRepository)
        {
            _yahooDataRepository = yahooDataRepository;
            _yahooTreeDataRepository = yahooTreeDataRepository;
        }
        #endregion

        #region Implemented Interfaces

        #region IService
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
        #endregion

        #region IYahooService

        public IEnumerable<YahooNormalized> PrepareData(int updatePeriod = 100)
        {
            var yahooRecords = Enumerable.Reverse(_yahooDataRepository.CsvLinesNormalized).ToList();
            var data = new List<YahooNormalized>();

            var period = 0;
            double mean = 0.0;
            double variance = 0.0;
            var index = 0;
            
            foreach (var record in yahooRecords)
            {
                var change = index > 0 ? Math.Round((record.Close / yahooRecords[index - 1].Close - 1.0) * 1000000000.0) / 1000000000.0 : 0.0;
                if (period == 0)
                {
                    mean = record.Close;
                    variance = 0.0;
                    data.Add(YahooHelper.BuildYahooNormalized(record, change, mean, 0.0));
                }
                else
                {
                    var prevSize = period;
                    var sizeNow = period + 1;

                    mean = (prevSize * mean + record.Close) / sizeNow;
                    var difference = record.Close - mean;
                    variance = (double)prevSize / sizeNow * variance + 1.0 / prevSize * difference * difference;
                    var volatility = Math.Round(Math.Sqrt(variance) * 1000000000) / 1000000000;
                    data.Add(YahooHelper.BuildYahooNormalized(record, change, Math.Round(mean * 1000000000.0) / 1000000000.0, volatility));
                }
                index++;
                period++;
                period %= updatePeriod;
            }

            return data;
        }

        public void SaveYahooData(IList<YahooNormalized> yahooRecords, string path)
        {
            var yahooTreeData = YahooHelper.BuildYahooTreeDataList(yahooRecords);

            _yahooTreeDataRepository.CollectionName = "Yahoo";
            _yahooTreeDataRepository.Path = path;
            _yahooTreeDataRepository.NamesFileContents = YahooHelper.BuildYahooNamesFile();

            _yahooTreeDataRepository.SaveData(yahooTreeData);
        }
        #endregion

        #endregion

    }
}
