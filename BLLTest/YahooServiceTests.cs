using System;
using System.Collections.Generic;
using BLL;
using IBLL.Exceptions;
using IDLL.Data;
using IDLL.Exceptions;
using IDLL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BLLTest
{
    [TestClass]
    public class YahooServiceTests
    {

        #region Private Fields
        private Mock<ICsvDataRepository<YahooRecord>> _yahooDataRepositoryMock;
        private YahooService _service;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _yahooDataRepositoryMock = new Mock<ICsvDataRepository<YahooRecord>>();

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

            _service = new YahooService(_yahooDataRepositoryMock.Object);
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
            var data = _service.PrepareData();

            Assert.AreEqual(5, data.Count);
        }
        #endregion

        #region PrepareData_ShouldReverseListOrder
        [TestMethod]
        public void PrepareData_ShouldReverseListOrder()
        {
            var data = _service.PrepareData();

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
            var data = _service.PrepareData();

            Assert.AreEqual(0.0, data[0].Volatility);
        }
        #endregion

        #region PrepareData_ShouldCalculateCorrectOtherVolatilities
        [TestMethod]
        public void PrepareData_ShouldCalculateCorrectOtherVolatilities()
        {
            var data = _service.PrepareData();

            Assert.AreEqual(0.067115, data[1].Volatility);
            Assert.AreEqual(0.311068041, data[2].Volatility);
            Assert.AreEqual(0.399625539, data[3].Volatility);
            Assert.AreEqual(0.470485581, data[4].Volatility);
        }
        #endregion

        #endregion

        #endregion
    }
}
