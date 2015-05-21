#region Usings
using System;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Interfaces;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Implementation.BLL
{
    public class ForexMarketService : IForexMarketService
    {

        #region Private Fields
        private readonly ICsvDataRepository<ForexTreeData> _forexTreeCsvDataRepository;
        private readonly IForexMarketPathRepository _forexMarketPathRepository;
        private int _index = -1;
        private int _pathIndex;
        #endregion

        #region Constructors and Destructors
        public ForexMarketService(
            ICsvDataRepository<ForexTreeData> forexTreeCsvDataRepository,
            IForexMarketPathRepository forexMarketPathRepository)
        {
            _forexTreeCsvDataRepository = forexTreeCsvDataRepository;
            _forexMarketPathRepository = forexMarketPathRepository;
        }
        #endregion

        #region Implemented Interfaces

        #region IForexBaseService
        public string Period { get; set; }
        public int StartingMonth { get; set; }
        public int StartingChunk { get; set; }
        #endregion

        #region IForexMarketService

        public void SetForexTreesPath(string forexTreesPath)
        {
            _forexMarketPathRepository.ForexTreesPath = forexTreesPath;
        }

        public bool IsDone()
        {
            if (_forexTreeCsvDataRepository.CsvLinesNormalized == null)
            {
                return false;
            }
            return _pathIndex == _forexMarketPathRepository.Paths.Count &&
                   _index == _forexTreeCsvDataRepository.CsvLinesNormalized.Count;
        }

        public ForexTreeData NextRecord()
        {
            if (_index == -1)
            {
                _forexMarketPathRepository.SetPaths(StartingMonth, Period, StartingChunk);
                _index = 0;
            }

            try
            {
                ReadNextChunk();
                return _forexTreeCsvDataRepository.CsvLinesNormalized[_index++];
            }
            catch (Exception)
            {
                throw new BllException("No records left.");
            }
        }

        public void Clear()
        {
            _forexMarketPathRepository.Paths.Clear();
            _index = -1;
            _pathIndex = 0;
        }

        #endregion

        #endregion

        #region Methods
        private void ReadNextChunk()
        {
            if (_forexTreeCsvDataRepository.CsvLinesNormalized != null)
            {
                if (_index < _forexTreeCsvDataRepository.CsvLinesNormalized.Count && _pathIndex != 0)
                {
                    return;
                }
            }

            var path = _forexMarketPathRepository.Paths[_pathIndex++];
            _forexTreeCsvDataRepository.LoadData(path);
            _forexTreeCsvDataRepository.NormalizeData(0);
            _index = 0;

        }
        #endregion

    }
}
