#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IDLL.Data;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Implementation.BLL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class YahooServiceTests
    {

        #region Private Fields
        private Mock<ICsvDataRepository<YahooRecord>> _yahooDataRepositoryMock;
        private Mock<ITreeDataRepository<YahooTreeData>> _yahooTreeDataRepositoryMock;
        private YahooService _service;

        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _yahooDataRepositoryMock = new Mock<ICsvDataRepository<YahooRecord>>();
            _yahooTreeDataRepositoryMock = new Mock<ITreeDataRepository<YahooTreeData>>();

            _yahooDataRepositoryMock
                .Setup(x => x.LoadData("test"));

            _yahooDataRepositoryMock
                .Setup(x => x.CsvLinesNormalized)
                .Returns(
                    new List<YahooRecord>
                    {
                        new YahooRecord { Close = 1.25765 },
                        new YahooRecord { Close = 1.51123 },
                        new YahooRecord { Close = 1.75987 },
                        new YahooRecord { Close = 2.34231 },
                        new YahooRecord { Close = 2.47654 }
                    }
                );

            _service = new YahooService(
                _yahooDataRepositoryMock.Object,
                _yahooTreeDataRepositoryMock.Object
            );
        }
        #endregion

        #region TestSuite

        #region ReadCsv Tests

        #region ReadCsv_DalExceptionThrownByLoadData_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Exception of DAL: success")]
        public void ReadCsv_DalExceptionThrown_ShouldThrowBllException()
        {
            _yahooDataRepositoryMock
                .Setup(x => x.LoadData("test"))
                .Throws(new DalException("success"));

            _service.ReadCsv("test");

            Assert.Fail();
        }
        #endregion

        #region ReadCsv_ExceptionThrownByNormalizeData_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "Exception of other origin: success")]
        public void ReadCsv_ExceptionThrownByNormalizeData_ShouldThrowBllException()
        {
            _yahooDataRepositoryMock
                .Setup(x => x.NormalizeData(1))
                .Throws(new Exception("success"));

            _service.ReadCsv("test");

            Assert.Fail();
        }
        #endregion

        #endregion

        #region PrepareData Tests

        #region PrepareData_ShouldGetCorrectResultsCount
        [TestMethod]
        public void PrepareData_ShouldGetCorrectResultsCount()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(5, data.Count);
        }
        #endregion

        #region PrepareData_ShouldReverseListOrder
        [TestMethod]
        public void PrepareData_ShouldReverseListOrder()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(2.47654, data[0].Close);
            Assert.AreEqual(2.34231, data[1].Close);
            Assert.AreEqual(1.75987, data[2].Close);
            Assert.AreEqual(1.51123, data[3].Close);
            Assert.AreEqual(1.25765, data[4].Close);
        }
        #endregion

        #region PrepareData_FirstVolatilityMustBeZero
        [TestMethod]
        public void PrepareData_FirstVolatilityMustBeZero()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(0.0, data[0].Volatility);
        }
        #endregion

        #region PrepareData_ShouldCalculateCorrectOtherVolatilities
        [TestMethod]
        public void PrepareData_ShouldCalculateCorrectOtherVolatilities()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(0.067115, data[1].Volatility);
            Assert.AreEqual(0.311068041, data[2].Volatility);
            Assert.AreEqual(0.399625539, data[3].Volatility);
            Assert.AreEqual(0.470485581, data[4].Volatility);
        }
        #endregion

        #region PrepareData_FirstChangeShouldBeZero
        [TestMethod]
        public void PrepareData_FirstChangeShouldBeZero()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(0.0, data[0].Change);
        }
        #endregion

        #region PrepareData_ShouldCalculateCorrectChanges
        [TestMethod]
        public void PrepareData_ShouldCalculateCorrectChanges()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(-0.054200619, data[1].Change);
            Assert.AreEqual(-0.24866051, data[2].Change);
            Assert.AreEqual(-0.141283163, data[3].Change);
            Assert.AreEqual(-0.167797092, data[4].Change);
        }
        #endregion

        #region PrepareData_FirstMovingAverageShouldBeLastValue
        [TestMethod]
        public void PrepareData_FirstMovingAverageShouldBeLastValue()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(2.47654, data[0].MovingAverage);
        }
        #endregion

        #region PrepareData_ShouldCalculateCorrectMovingAverages
        [TestMethod]
        public void PrepareData_ShouldCalculateCorrectMovingAverages()
        {
            var data = _service.PrepareData().ToList();

            Assert.AreEqual(2.409425, data[1].MovingAverage);
            Assert.AreEqual(2.192906667, data[2].MovingAverage);
            Assert.AreEqual(2.0224875, data[3].MovingAverage);
            Assert.AreEqual(1.86952, data[4].MovingAverage);
        }
        #endregion

        #region PrepareData_PeriodProvided_ShouldRefreshVolatilities
        [TestMethod]
        public void PrepareData_PeriodProvided_ShouldRefreshVolatilities()
        {
            const int period = 3;
            var data = _service.PrepareData(period).ToList();

            Assert.AreEqual(0.0, data[0].Volatility);
            Assert.AreEqual(0.067115, data[1].Volatility);
            Assert.AreEqual(0.311068041, data[2].Volatility);
            Assert.AreEqual(0.0, data[3].Volatility);
            Assert.AreEqual(0.12679, data[4].Volatility);
        }
        #endregion

        #region PrepareData_PeriodProvided_ShouldRefreshMovingAverages
        [TestMethod]
        public void PrepareData_PeriodProvided_ShouldRefreshMovingAverages()
        {
            const int period = 3;
            var data = _service.PrepareData(period).ToList();

            Assert.AreEqual(2.47654, data[0].MovingAverage);
            Assert.AreEqual(2.409425, data[1].MovingAverage);
            Assert.AreEqual(2.192906667, data[2].MovingAverage);
            Assert.AreEqual(1.51123, data[3].MovingAverage);
            Assert.AreEqual(1.38444, data[4].MovingAverage);
        }
        #endregion

        #endregion

        #region SaveYahooData Tests

        #region SaveYahooData_ShouldSetYahooTreeMetaData
        [TestMethod]
        public void SaveYahooData_ShouldSetYahooTreeMetaData()
        {
            _service.SaveYahooData(new List<YahooNormalized>(), "testPath");

            _yahooTreeDataRepositoryMock.VerifySet(x => x.CollectionName = "Yahoo", Times.Once);
            _yahooTreeDataRepositoryMock.VerifySet(x => x.Path = "testPath", Times.Once);
            _yahooTreeDataRepositoryMock.VerifySet(x => x.NamesFileContents = YahooHelper.BuildYahooNamesFile(), Times.Once);
        }
        #endregion

        #region SaveYahooData_ShouldCallSaveData
        [TestMethod]
        public void SaveYahooData_ShouldCallSaveData()
        {
            _service.SaveYahooData(new List<YahooNormalized>(), "testPath");

            _yahooTreeDataRepositoryMock.Verify(x => x.SaveData(It.IsAny<IEnumerable<YahooTreeData>>()), Times.Once);
        }
        #endregion

        #endregion

        #endregion

    }
}
