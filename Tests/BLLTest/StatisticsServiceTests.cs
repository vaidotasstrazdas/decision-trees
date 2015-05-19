#region Usings
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Tests.BLLTest.DataBuilders;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class StatisticsServiceTests
    {

        #region Private Fields
        private Mock<ICsvDataRepository<StatisticsRecord>> _statisticsRepositoryMock;
        private Mock<IStatisticsResultsRepository> _statisticsResultsRepositoryMock;
        private StatisticsService _service;
        private List<StatisticsRecord> _recordsList;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _statisticsRepositoryMock = new Mock<ICsvDataRepository<StatisticsRecord>>();
            _statisticsResultsRepositoryMock = new Mock<IStatisticsResultsRepository>();

            _recordsList = new ListOfStatisticsRecords()
                .AddRecord("C4.5", 39, 1, 0)
                .AddRecord("C5.0", 39, 1, 0)
                .AddRecord("C4.5", 31, 0, 1)
                .AddRecord("C5.0", 31, 0, 1)
                .AddRecord("C4.5", 162, 9, 2)
                .AddRecord("C5.0", 162, 4, 2)
                .AddRecord("C4.5", 117, 1, 3)
                .AddRecord("C5.0", 117, 0, 3)
                .AddRecord("C4.5", 54, 1, 4)
                .AddRecord("C5.0", 54, 1, 4)
                .Build();

            _statisticsRepositoryMock
                .Setup(x => x.CsvLinesNormalized)
                .Returns(_recordsList);

            _service = new StatisticsService(_statisticsRepositoryMock.Object, _statisticsResultsRepositoryMock.Object);
        }
        #endregion

        #region TestSuite

        #region ReadStatisticsData Tests

        #region ReadStatisticsData_ShouldLoadData
        [TestMethod]
        public void ReadStatisticsData_ShouldLoadData()
        {
            _service.ReadStatisticsData("test");

            _statisticsRepositoryMock
                .Verify(x => x.LoadData("test"), Times.Once());
        }
        #endregion

        #region ReadStatisticsData_ShouldNormalizeData
        [TestMethod]
        public void ReadStatisticsData_ShouldNormalizeData()
        {
            _service.ReadStatisticsData("test");

            _statisticsRepositoryMock
                .Verify(x => x.NormalizeData(1), Times.Once());
        }
        #endregion

        #region ReadStatisticsData_LoadDataThrowDalException_ShouldRethrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Exception of DAL: DAL Error.")]
        public void ReadStatisticsData_LoadDataThrowDalException_ShouldRethrowBllException()
        {
            _statisticsRepositoryMock
                .Setup(x => x.LoadData("test"))
                .Throws(new DalException("DAL Error."));

            _service.ReadStatisticsData("test");
        }
        #endregion

        #endregion

        #region PrepareData Tests

        #region PrepareData_ShouldGetCorrectSizeOfStatisticsSequenceCollection
        [TestMethod]
        public void PrepareData_ShouldGetCorrectSizeOfStatisticsSequenceCollection()
        {
            _service.PrepareData();

            Assert.AreEqual(5, _service.StatisticsSequence.Count);
        }
        #endregion

        #region PrepareData_ShouldGetCorrectSequenceOfC45Errors
        [TestMethod]
        public void PrepareData_ShouldGetCorrectSequenceOfC45Errors()
        {
            _service.PrepareData();

            var expectedCollection = new List<int> { 1, 0, 9, 1, 1 };
            var errorsCollection = _service.StatisticsSequence.Select(x => x.C45Errors).ToList();

            CollectionAssert.AreEqual(expectedCollection, errorsCollection);
        }
        #endregion

        #region PrepareData_ShouldGetCorrectSequenceOfC50Errors
        [TestMethod]
        public void PrepareData_ShouldGetCorrectSequenceOfC50Errors()
        {
            _service.PrepareData();

            var expectedCollection = new List<int> { 1, 0, 4, 0, 1 };
            var errorsCollection = _service.StatisticsSequence.Select(x => x.C50Errors).ToList();

            CollectionAssert.AreEqual(expectedCollection, errorsCollection);
        }
        #endregion

        #region PrepareData_ShouldGetCorrectSequenceOfChunks
        [TestMethod]
        public void PrepareData_ShouldGetCorrectSequenceOfChunks()
        {
            _service.PrepareData();

            var expectedCollection = new List<int> { 0, 1, 2, 3, 4 };
            var chunksCollection = _service.StatisticsSequence.Select(x => x.Chunk).ToList();

            CollectionAssert.AreEqual(expectedCollection, chunksCollection);
        }
        #endregion

        #region PrepareData_ShouldGetCorrectSequenceOfCases
        [TestMethod]
        public void PrepareData_ShouldGetCorrectSequenceOfCases()
        {
            _service.PrepareData();

            var expectedCollection = new List<int> { 39, 31, 162, 117, 54 };
            var chunksCollection = _service.StatisticsSequence.Select(x => x.Cases).ToList();

            CollectionAssert.AreEqual(expectedCollection, chunksCollection);
        }
        #endregion

        #region PrepareData_CsvLinesNumberIsOdd_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Incorrect number of elements in CSV file. Number of elements should be multiple of two.")]
        public void PrepareData_CsvLinesNumberIsOdd_ShouldThrowBllException()
        {
            _recordsList.RemoveAt(_recordsList.Count - 1);
            _service.PrepareData();
        }
        #endregion

        #endregion

        #region CalculateStatistics Tests

        #region CalculateStatistics_ShouldGetCorrectCountOfEqualResults
        [TestMethod]
        public void CalculateStatistics_ShouldGetCorrectCountOfEqualResults()
        {
            _service.PrepareData();
            var statistics = _service.CalculateStatistics();

            Assert.AreEqual(3, statistics.Equal);
        }
        #endregion

        #region CalculateStatistics_ShouldGetCorrectCountOfC45Better
        [TestMethod]
        public void CalculateStatistics_ShouldGetCorrectCountOfC45Better()
        {
            _service.PrepareData();
            var statistics = _service.CalculateStatistics();

            Assert.AreEqual(0, statistics.C45Better);
        }
        #endregion

        #region CalculateStatistics_ShouldGetCorrectCountOfC50Better
        [TestMethod]
        public void CalculateStatistics_ShouldGetCorrectCountOfC50Better()
        {
            _service.PrepareData();
            var statistics = _service.CalculateStatistics();

            Assert.AreEqual(2, statistics.C50Better);
        }
        #endregion

        #endregion

        #region AddToRepository Tests

        #region AddToRepository_BluePrintNotSet_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "BluePrint not set.")]
        public void AddToRepository_BluePrintNotSet_ShouldThrowBllException()
        {
            _service.AddToRepository(null);
        }
        #endregion

        #region AddToRepository_BluePrintIsEmpty_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "BluePrint not set.")]
        public void AddToRepository_BluePrintIsEmpty_ShouldThrowBllException()
        {
            _service.BluePrint = string.Empty;
            _service.AddToRepository(null);
        }
        #endregion

        #region AddToRepository_NullResultProvided_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "StatisticsDto can not be null.")]
        public void AddToRepository_NullResultProvided_ShouldThrowBllException()
        {
            _service.BluePrint = "Test";
            _service.AddToRepository(null);
        }
        #endregion

        #region AddToRepository_EverythingIsSetUp_ShouldCallAdd
        [TestMethod]
        public void AddToRepository_EverythingIsSetUp_ShouldCallAdd()
        {
            _service.BluePrint = "Test";
            _service.AddToRepository(new StatisticsDto());

            _statisticsResultsRepositoryMock
                .Verify(x => x.Add(It.IsAny<StatisticsResult>()), Times.Once());
        }
        #endregion

        #region AddToRepository_EverythingIsSetUp_ShouldFormCorrectObjectForRepository
        [TestMethod]
        public void AddToRepository_EverythingIsSetUp_ShouldFormCorrectObjectForRepository()
        {
            StatisticsResult result = null;
            var statistics = new StatisticsDto
            {
                C45Better = 10,
                C50Better = 20,
                Equal = 5
            };

            _statisticsResultsRepositoryMock
                .Setup(x => x.Add(It.IsAny<StatisticsResult>()))
                .Callback<StatisticsResult>(x => result = x);

            _service.BluePrint = "Test";

            _service.AddToRepository(statistics);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.BluePrint);
            Assert.AreEqual(10, result.C45Better);
            Assert.AreEqual(20, result.C50Better);
            Assert.AreEqual(5, result.Equal);
        }
        #endregion

        #endregion

        #region CommitToRepository Tests

        #region CommitToRepository_DalExceptionThrown_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Exception of DAL: Commit is forbidden.")]
        public void CommitToRepository_DalExceptionThrown_ShouldThrowBllException()
        {
            _statisticsResultsRepositoryMock
                .Setup(x => x.Save(It.IsAny<string>()))
                .Throws(new DalException("Commit is forbidden."));

            _service.CommitToRepository(null);
        }
        #endregion

        #region CommitToRepository_ShouldCallSave
        [TestMethod]
        public void CommitToRepository_ShouldCallSave()
        {
            _service.CommitToRepository("Test");

            _statisticsResultsRepositoryMock
                .Verify(x => x.Save("Test"), Times.Once());
        }
        #endregion

        #endregion

        #region ResetSequence Tests

        #region ResetSequence_StatisticsSequenceShouldBeEmpty
        [TestMethod]
        public void ResetSequence_StatisticsSequenceShouldBeEmpty()
        {
            _service.PrepareData();
            _service.ResetSequence();

            Assert.AreEqual(0, _service.StatisticsSequence.Count);
        }
        #endregion

        #endregion

        #endregion

    }
}
