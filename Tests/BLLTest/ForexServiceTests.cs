using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;

namespace Tests.BLLTest
{
    [TestClass]
    public class ForexServiceTests
    {
        #region Private Fields
        private Mock<IForexCsvRepository> _forexCsvRepositoryMock;
        private Mock<ITreeDataRepository<ForexTreeData>> _treeRepositoryMock;
        private ForexService _service;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _forexCsvRepositoryMock = new Mock<IForexCsvRepository>();
            _treeRepositoryMock = new Mock<ITreeDataRepository<ForexTreeData>>();
            
            _service = new ForexService(
                _forexCsvRepositoryMock.Object,
                _treeRepositoryMock.Object
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
            _forexCsvRepositoryMock
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
            _forexCsvRepositoryMock
                .Setup(x => x.NormalizeData())
                .Throws(new Exception("success"));

            _service.ReadCsv("test");

            Assert.Fail();
        }
        #endregion

        #endregion

        #region PrepareData Tests
        #endregion

        #endregion

    }
}
