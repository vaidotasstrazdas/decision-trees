#region Usings
using System;
using System.Collections.Generic;
using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL.Helpers;
#endregion

namespace Implementation.BLL
{
    public class ForexService : IForexService
    {

        #region Private Fields
        private readonly IForexCsvRepository _forexCsvRepository;
        private readonly ITreeDataRepository<ForexTreeData> _forexTreeDataRepository;
        #endregion

        #region Constructors and Destructors
        public ForexService(
            IForexCsvRepository forexCsvRepository,
            ITreeDataRepository<ForexTreeData> forexTreeDataRepository)
        {
            _forexCsvRepository = forexCsvRepository;
            _forexTreeDataRepository = forexTreeDataRepository;
        }
        #endregion

        #region Implemented Interfaces

        #region IService
        public void ReadCsv(string filePath)
        {
            try
            {
                _forexCsvRepository.LoadData(filePath);
                _forexCsvRepository.NormalizeData();
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

        #region IForexService
        public List<ForexDto> PrepareData(int splitPeriodSeconds)
        {
            var forexRecords = _forexCsvRepository.CsvLinesNormalized;
            var firstRecord = forexRecords[0];
            var options = ForexHelper.InitializeForexTrackData(firstRecord);
            int index = 0;
            var forexSplitData = new List<ForexDto>();
            var currentDto = new ForexDto
            {
                FileName = index.ToString(),
                ForexData = new List<ForexTreeData>()
            };
            DateTime differenceTime = DateTime.ParseExact(firstRecord.Date, "yyyyMMdd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

            for (var i = 0; i < forexRecords.Count; i++)
            {
                var record = forexRecords[i];
                var dateTime = DateTime.ParseExact(record.Date, "yyyyMMdd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture).Subtract(differenceTime);
                var seconds = (int)dateTime.TotalSeconds;
                if (seconds >= splitPeriodSeconds)
                {
                    if (i < forexRecords.Count)
                    {
                        differenceTime = DateTime.ParseExact(forexRecords[i + 1].Date, "yyyyMMdd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    options = ForexHelper.InitializeForexTrackData(record, options);
                    forexSplitData.Add(currentDto);
                    ForexHelper.SetCorrectMarketActions(currentDto);
                    index++;
                    currentDto = new ForexDto
                    {
                        FileName = index.ToString(),
                        ForexData = new List<ForexTreeData>()
                    };
                }

                var treeRecord = ForexHelper.BuildForexTreeRecord(record, options);

                currentDto.ForexData.Add(treeRecord);
                options.CurrentRecord++;
            }

            if (forexSplitData.Contains(currentDto))
            {
                return forexSplitData;
            }

            forexSplitData.Add(currentDto);
            ForexHelper.SetCorrectMarketActions(currentDto);

            return forexSplitData;
        }

        public void SaveForexData(IList<ForexDto> forexRecords, string path)
        {
            _forexTreeDataRepository.Path = path;
            _forexTreeDataRepository.NamesFileContents = ForexHelper.BuildForexNamesFile();
            foreach (var record in forexRecords)
            {
                _forexTreeDataRepository.CollectionName = "Forex_" + record.FileName;
                _forexTreeDataRepository.SaveData(record.ForexData);
            }
        }
        #endregion

        #endregion

    }
}
