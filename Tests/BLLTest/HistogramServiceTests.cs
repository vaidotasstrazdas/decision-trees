#region Usings
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
    public class HistogramServiceTests
    {

        #region Private Fields
        private Mock<ICsvDataRepository<StatisticsRecord>> _statisticsRepositoryMock;
        private Mock<IHistogramResultsRepository> _histogramResultsRepositoryMock;
        private List<HistogramResult> _histogramResults;
        private List<StatisticsRecord> _recordsList;
        private HistogramService _service;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _statisticsRepositoryMock = new Mock<ICsvDataRepository<StatisticsRecord>>();
            _histogramResultsRepositoryMock = new Mock<IHistogramResultsRepository>();
            _histogramResults = new List<HistogramResult>();

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

            _histogramResultsRepositoryMock
                .Setup(x => x.Add(It.IsAny<HistogramResult>()))
                .Callback<HistogramResult>(x => _histogramResults.Add(x));

            _service = new HistogramService(_statisticsRepositoryMock.Object, _histogramResultsRepositoryMock.Object)
                        {
                            IntervalLength = 0.0125
                        };
        }
        #endregion

        #region TestSuite

        #region CalculateStatistics Tests

        #region CalculateStatistics_ShouldGetCorrectNumberOfElementsInHistogram
        [TestMethod]
        public void CalculateStatistics_ShouldGetCorrectNumberOfElementsInHistogram()
        {
            var histogram = _service.CalculateStatistics();

            Assert.AreEqual(4, histogram.Count);
        }
        #endregion

        #region CalculateStatistics_IntervalsFromMustBeCorrect
        [TestMethod]
        public void CalculateStatistics_IntervalsFromMustBeCorrect()
        {
            var histogram = _service.CalculateStatistics();

            var intervalsExpected = new List<double> { 0.0, 0.0125, 0.025, 0.05};
            var intervalsActual = histogram.Select(x => x.IntervalFrom).ToList();
            CollectionAssert.AreEqual(intervalsExpected, intervalsActual);
        }
        #endregion

        #region CalculateStatistics_IntervalsToMustBeCorrect
        [TestMethod]
        public void CalculateStatistics_IntervalsToMustBeCorrect()
        {
            var histogram = _service.CalculateStatistics();

            var intervalsExpected = new List<double> { 0.0125, 0.025, 0.0375, 0.0625 };
            var intervalsActual = histogram.Select(x => x.IntervalTo).ToList();
            CollectionAssert.AreEqual(intervalsExpected, intervalsActual);
        }
        #endregion

        #region CalculateStatistics_FirstIntervalMustHaveCorrectC45Cases
        [TestMethod]
        public void CalculateStatistics_FirstIntervalMustHaveCorrectC45Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.0) < tolerane && Math.Abs(x.IntervalTo - 0.0125) < tolerane);

            Assert.AreEqual(2, histogramDto.C45Cases);
        }
        #endregion

        #region CalculateStatistics_SecondIntervalMustHaveCorrectC45Cases
        [TestMethod]
        public void CalculateStatistics_SecondIntervalMustHaveCorrectC45Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.0125) < tolerane && Math.Abs(x.IntervalTo - 0.025) < tolerane);

            Assert.AreEqual(1, histogramDto.C45Cases);
        }
        #endregion

        #region CalculateStatistics_ThirdIntervalMustHaveCorrectC45Cases
        [TestMethod]
        public void CalculateStatistics_ThirdIntervalMustHaveCorrectC45Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.025) < tolerane && Math.Abs(x.IntervalTo - 0.0375) < tolerane);

            Assert.AreEqual(1, histogramDto.C45Cases);
        }
        #endregion

        #region CalculateStatistics_FourthIntervalMustHaveCorrectC45Cases
        [TestMethod]
        public void CalculateStatistics_FourthIntervalMustHaveCorrectC45Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.05) < tolerane && Math.Abs(x.IntervalTo - 0.0625) < tolerane);

            Assert.AreEqual(1, histogramDto.C45Cases);
        }
        #endregion

        #region CalculateStatistics_FirstIntervalMustHaveCorrectC50Cases
        [TestMethod]
        public void CalculateStatistics_FirstIntervalMustHaveCorrectC50Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.0) < tolerane && Math.Abs(x.IntervalTo - 0.0125) < tolerane);

            Assert.AreEqual(2, histogramDto.C50Cases);
        }
        #endregion

        #region CalculateStatistics_SecondIntervalMustHaveCorrectC50Cases
        [TestMethod]
        public void CalculateStatistics_SecondIntervalMustHaveCorrectC50Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.0125) < tolerane && Math.Abs(x.IntervalTo - 0.025) < tolerane);

            Assert.AreEqual(2, histogramDto.C50Cases);
        }
        #endregion

        #region CalculateStatistics_ThirdIntervalMustHaveCorrectC50Cases
        [TestMethod]
        public void CalculateStatistics_ThirdIntervalMustHaveCorrectC50Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.025) < tolerane && Math.Abs(x.IntervalTo - 0.0375) < tolerane);

            Assert.AreEqual(1, histogramDto.C50Cases);
        }
        #endregion

        #region CalculateStatistics_FourthIntervalMustHaveCorrectC50Cases
        [TestMethod]
        public void CalculateStatistics_FourthIntervalMustHaveCorrectC50Cases()
        {
            const double tolerane = 0.00000001;
            var histogram = _service.CalculateStatistics();

            var histogramDto = histogram.Single(x => Math.Abs(x.IntervalFrom - 0.05) < tolerane && Math.Abs(x.IntervalTo - 0.0625) < tolerane);

            Assert.AreEqual(0, histogramDto.C50Cases);
        }
        #endregion

        #endregion

        #region AddToRepository Tests

        #region AddToRepository_ShouldAddCorrectBluePrints
        [TestMethod]
        public void AddToRepository_ShouldAddCorrectBluePrints()
        {
            var histogram = _service.CalculateStatistics();
            _service.AddToRepository(histogram);

            var bluePrintsExpected = new List<string> { "0,00%-1,25%", "1,25%-2,50%", "2,50%-3,75%", "5,00%-6,25%" };
            var bluePrintsActual = _histogramResults.Select(x => x.BluePrint).ToList();

            CollectionAssert.AreEqual(bluePrintsExpected, bluePrintsActual);
        }
        #endregion

        #region AddToRepository_ShouldAddCorrectC45Cases
        [TestMethod]
        public void AddToRepository_ShouldAddCorrectC45Cases()
        {
            var histogram = _service.CalculateStatistics();
            _service.AddToRepository(histogram);

            var casesExpected = new List<int> { 2, 1, 1, 1 };
            var casesActual = _histogramResults.Select(x => x.C45Cases).ToList();

            CollectionAssert.AreEqual(casesExpected, casesActual);
        }
        #endregion

        #region AddToRepository_ShouldAddCorrectC50Cases
        [TestMethod]
        public void AddToRepository_ShouldAddCorrectC50Cases()
        {
            var histogram = _service.CalculateStatistics();
            _service.AddToRepository(histogram);

            var casesExpected = new List<int> { 2, 2, 1, 0 };
            var casesActual = _histogramResults.Select(x => x.C50Cases).ToList();

            CollectionAssert.AreEqual(casesExpected, casesActual);
        }
        #endregion

        #endregion

        #region CommitToRepository Tests

        #region CommitToRepository_DalExceptionThrown_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Exception of DAL: Commit is forbidden.")]
        public void CommitToRepository_DalExceptionThrown_ShouldThrowBllException()
        {
            _histogramResultsRepositoryMock
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

            _histogramResultsRepositoryMock
                .Verify(x => x.Save("Test"), Times.Once());
        }
        #endregion

        #endregion

        #endregion

    }
}
