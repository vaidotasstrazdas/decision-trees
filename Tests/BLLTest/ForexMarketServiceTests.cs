#region Usings
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Bridge.IBLL.Data;
using Bridge.IBLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class ForexMarketServiceTests
    {

        #region Private Fields
        private Mock<ICsvDataRepository<ForexTreeData>> _forexTreeCsvDataRepositoryMock;
        private Mock<IForexMarketPathRepository> _forexMarketRepositoryMock;
        private ForexMarketService _service;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _forexTreeCsvDataRepositoryMock = new Mock<ICsvDataRepository<ForexTreeData>>();
            _forexMarketRepositoryMock = new Mock<IForexMarketPathRepository>();

            _forexTreeCsvDataRepositoryMock
                .Setup(x => x.CsvLinesNormalized)
                .Returns(new List<ForexTreeData>
                {
                    new ForexTreeData
                    {
                        Bid = 1.1111,
                        Ask = 1.1114
                    },
                    new ForexTreeData
                    {
                        Bid = 1.1112,
                        Ask = 1.1115
                    }
                });

            _forexMarketRepositoryMock
                .Setup(x => x.Paths)
                .Returns(new List<string>
                {
                    "01\\300\\Forex_0.data",
                    "01\\300\\Forex_1.data",
                    "02\\300\\Forex_0.data"
                });

            _service = new ForexMarketService(
                _forexTreeCsvDataRepositoryMock.Object,
                _forexMarketRepositoryMock.Object)
            {
                Period = "300",
                StartingChunk = 0,
                StartingMonth = 1
            };
            _service.SetForexTreesPath(string.Empty);
        }
        #endregion

        #region TestSuite

        #region NextRecord Tests

        #region NextRecord_ShouldGetTheFirstRecord
        [TestMethod]
        public void NextRecord_ShouldGetTheFirstRecord()
        {
            var record = _service.NextRecord();

            Assert.AreEqual(1.1111, record.Bid);
            Assert.AreEqual(1.1114, record.Ask);
        }
        #endregion

        #region NextRecord_ShouldGetTheSecondRecord
        [TestMethod]
        public void NextRecord_ShouldGetTheSecondRecord()
        {
            _service.NextRecord();
            var record = _service.NextRecord();

            Assert.AreEqual(1.1112, record.Bid);
            Assert.AreEqual(1.1115, record.Ask);
        }
        #endregion

        #region NextRecord_NoRecordsLeft_ShouldThrowBllException
        [TestMethod]
        [ExpectedException(typeof(BllException), "No records left.")]
        public void NextRecord_NoRecordsLeft_ShouldThrowBllException()
        {
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();
        }
        #endregion

        #region NextRecord_ShouldReadRecords
        [TestMethod]
        public void NextRecord_ShouldReadRecords()
        {
            _service.NextRecord();

            _forexTreeCsvDataRepositoryMock
                .Verify(x => x.LoadData("01\\300\\Forex_0.data"), Times.Once());
            _forexTreeCsvDataRepositoryMock
                .Verify(x => x.NormalizeData(0), Times.Once());
        }
        #endregion

        #region NextRecord_ShouldUpdateRecords
        [TestMethod]
        public void NextRecord_ShouldUpdateRecords()
        {
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();

            _forexTreeCsvDataRepositoryMock
                .Verify(x => x.LoadData("01\\300\\Forex_1.data"), Times.Once());
            _forexTreeCsvDataRepositoryMock
                .Verify(x => x.NormalizeData(0), Times.Exactly(2));
        }
        #endregion

        #region NextRecord_ShouldUpdateMonth
        [TestMethod]
        public void NextRecord_ShouldUpdateMonth()
        {
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();
            _service.NextRecord();

            _forexTreeCsvDataRepositoryMock
                .Verify(x => x.LoadData("02\\300\\Forex_0.data"));
            _forexTreeCsvDataRepositoryMock
                .Verify(x => x.NormalizeData(0), Times.Exactly(3));
        }
        #endregion

        #endregion

        #region IsDone Tests

        #region IsDone_MustBeFalseOnTheFirstCall
        [TestMethod]
        public void IsDone_MustBeFalseOnTheFirstCall()
        {
            Assert.IsFalse(_service.IsDone());
        }
        #endregion

        #region IsDone_MustPreserveCorrectStatusOnEveryCall
        [TestMethod]
        public void IsDone_MustPreserveCorrectStatusOnEveryCall()
        {
            _service.NextRecord();
            Assert.IsFalse(_service.IsDone());

            _service.NextRecord();
            Assert.IsFalse(_service.IsDone());

            _service.NextRecord();
            Assert.IsFalse(_service.IsDone());

            _service.NextRecord();
            Assert.IsFalse(_service.IsDone());

            _service.NextRecord();
            Assert.IsFalse(_service.IsDone());

            _service.NextRecord();
            Assert.IsTrue(_service.IsDone());

        }
        #endregion

        #endregion

        #region Traversion Tests

        #region IterateAllRecords_ShouldGetCorrectCount
        [TestMethod]
        public void IterateAllRecords_ShouldGetCorrectCount()
        {
            var records = new List<ForexTreeData>();
            while (!_service.IsDone())
            {
                var record = _service.NextRecord();
                records.Add(record);
            }

            Assert.AreEqual(6, records.Count);
        }
        #endregion

        #region IterateAllRecords_ShouldGetCorrectBidPrices
        [TestMethod]
        public void IterateAllRecords_ShouldGetCorrectBidPrices()
        {
            var records = new List<ForexTreeData>();
            while (!_service.IsDone())
            {
                var record = _service.NextRecord();
                records.Add(record);
            }

            var bidsExpected = new List<double> { 1.1111, 1.1112, 1.1111, 1.1112, 1.1111, 1.1112 };
            var bidsActual = records.Select(x => x.Bid).ToList();

            CollectionAssert.AreEqual(bidsExpected, bidsActual);
        }
        #endregion

        #region IterateAllRecords_ShouldGetCorrectAskPrices
        [TestMethod]
        public void IterateAllRecords_ShouldGetCorrectAskPrices()
        {
            var records = new List<ForexTreeData>();
            while (!_service.IsDone())
            {
                var record = _service.NextRecord();
                records.Add(record);
            }

            var asksExpected = new List<double> { 1.1114, 1.1115, 1.1114, 1.1115, 1.1114, 1.1115 };
            var asksActual = records.Select(x => x.Ask).ToList();

            CollectionAssert.AreEqual(asksExpected, asksActual);
        }
        #endregion

        #endregion

        #endregion

    }
}
