#region Usings

using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DLL;
using IDLL.Data;
using IDLL.Exceptions;
#endregion

namespace DLLTest
{
    [TestClass]
    public class CsvDataRepositoryTests
    {
        #region Private Fields
        private CsvDataRepository<YahooRecord> _dataRepository;
        private string _dataFilePath;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _dataRepository = new CsvDataRepository<YahooRecord>();
            _dataFilePath = "E:/Data/Tests/TestData.csv";
        }
        #endregion

        #region TestSuite

        #region LoadData Tests

        #region LoadData_NullPathProvided_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Empty or null string provided.")]
        public void LoadData_NullPathProvided_ShouldThrowDalException()
        {
            _dataRepository.LoadData(null);

            Assert.Fail();
        }
        #endregion

        #region LoadData_EmptyPathProvided_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Empty or null string provided.")]
        public void LoadData_EmptyPathProvided_ShouldThrowDalException()
        {
            _dataRepository.LoadData(string.Empty);

            Assert.Fail();
        }
        #endregion

        #region LoadData_FileDoesNotExist_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "File test does not exist.")]
        public void LoadData_FileDoesNotExist_ShouldThrowDalException()
        {
            _dataRepository.LoadData("test");

            Assert.Fail();
        }
        #endregion

        #region LoadData_LoadCsvFile_ShouldSaveCorrectResultsCount
        [TestMethod]
        public void LoadData_LoadCsvFile_ShouldSaveCorrectResultsCount()
        {
            _dataRepository.LoadData(_dataFilePath);

            Assert.AreEqual(10, _dataRepository.CsvLines.Count);
        }
        #endregion

        #region LoadData_LoadCsvFile_ShouldSaveCorrectHeader
        [TestMethod]
        public void LoadData_LoadCsvFile_ShouldSaveCorrectHeader()
        {
            _dataRepository.LoadData(_dataFilePath);

            Assert.AreEqual("Date", _dataRepository.CsvLines[0][0]);
            Assert.AreEqual("Open", _dataRepository.CsvLines[0][1]);
            Assert.AreEqual("High", _dataRepository.CsvLines[0][2]);
            Assert.AreEqual("Low", _dataRepository.CsvLines[0][3]);
            Assert.AreEqual("Close", _dataRepository.CsvLines[0][4]);
            Assert.AreEqual("Volume", _dataRepository.CsvLines[0][5]);
            Assert.AreEqual("Adj Close", _dataRepository.CsvLines[0][6]);
        }
        #endregion

        #region LoadData_LoadCsvFile_ShouldSaveCorrectData
        [TestMethod]
        public void LoadData_LoadCsvFile_ShouldSaveCorrectData()
        {
            _dataRepository.LoadData(_dataFilePath);

            Assert.AreEqual("2015-05-04", _dataRepository.CsvLines[1][0]);
            Assert.AreEqual("2110.22998", _dataRepository.CsvLines[1][1]);
            Assert.AreEqual("2120.94995", _dataRepository.CsvLines[1][2]);
            Assert.AreEqual("2110.22998", _dataRepository.CsvLines[1][3]);
            Assert.AreEqual("2114.48999", _dataRepository.CsvLines[1][4]);
            Assert.AreEqual("3091580000", _dataRepository.CsvLines[1][5]);
            Assert.AreEqual("2114.48999", _dataRepository.CsvLines[1][6]);
        }
        #endregion

        #endregion

        #region NormalizeData_ShouldGetCorrectCount

        [TestMethod]
        public void NormalizeData_ShouldGetCorrectCount()
        {
            _dataRepository.LoadData(_dataFilePath);
            _dataRepository.NormalizeData();

            Assert.AreEqual(9, _dataRepository.CsvLinesNormalized.Count);
        }

        #endregion

        #region NormalizeData_ShouldNormalizeDataCorrectly

        [TestMethod]
        public void NormalizeData_ShouldNormalizeDataCorrectly()
        {
            _dataRepository.LoadData(_dataFilePath);
            _dataRepository.NormalizeData();

            YahooRecord record = _dataRepository.CsvLinesNormalized[0];
            Assert.AreEqual("05/04/2015 00:00:00", record.Date.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(2110.22998, record.Open);
            Assert.AreEqual(2120.94995, record.High);
            Assert.AreEqual(2110.22998, record.Low);
            Assert.AreEqual(2114.48999, record.Close);
            Assert.AreEqual(3091580000, record.Volume);
            Assert.AreEqual(2114.48999, record.AdjClose);
        }

        #endregion

        #endregion

    }
}
