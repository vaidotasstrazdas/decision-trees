#region Usings
using System.Collections.Generic;
using System.Configuration;
using Bridge.IDLL.Data;
using Implementation.DLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Tests.DLLTest
{
    [TestClass]
    public class ForexCsvRepositoryTests
    {

        #region Private Fields
        private ForexCsvRepository _forexCsvRepository;
        private string _dataFilePath;
        private List<ForexRecord> _forexLines;

        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _forexCsvRepository = new ForexCsvRepository();
            _dataFilePath = ConfigurationManager.AppSettings["TestDataDirectory"] + "\\Forex.csv";
            _forexCsvRepository.LoadData(_dataFilePath);
            _forexCsvRepository.NormalizeData();
            _forexLines = _forexCsvRepository.CsvLinesNormalized;
        }
        #endregion

        #region TestSuite

        #region NormalizeData Tests

        #region NormalizeData_ShoudGetCorrectCount
        [TestMethod]
        public void NormalizeData_ShoudGetCorrectCount()
        {
            Assert.AreEqual(29, _forexLines.Count);
        }
        #endregion

        #region NormalizeData_ShouldGetFirstCorrectRecord
        [TestMethod]
        public void NormalizeData_ShouldGetFirstCorrectRecord()
        {
            var record = _forexLines[0];

            Assert.AreEqual("EUR/USD", record.CurrencyPair);
            Assert.AreEqual("20140101 21:55:34.378", record.Date);
            Assert.AreEqual(1.37622, record.Bid);
            Assert.AreEqual(1.37693, record.Ask);
        }
        #endregion

        #region NormalizeData_ShouldGetLastCorrectRecord
        [TestMethod]
        public void NormalizeData_ShouldGetLastCorrectRecord()
        {
            var record = _forexLines[28];

            Assert.AreEqual("EUR/USD", record.CurrencyPair);
            Assert.AreEqual("20140101 21:57:53.710", record.Date);
            Assert.AreEqual(1.37487, record.Bid);
            Assert.AreEqual(1.37599, record.Ask);
        }
        #endregion

        #endregion

        #endregion

    }
}
