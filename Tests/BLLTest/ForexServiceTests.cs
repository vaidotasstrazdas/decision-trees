#region Usings
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Bridge.IBLL.Data;
using Bridge.IBLL.Data.Base;
using Bridge.IBLL.Exceptions;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Implementation.BLL.Helpers;
using Tests.BLLTest.DataBuilders;
#endregion

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

            var forexRecords = ForexRecordsBuilder.BuildRecords();
            _forexCsvRepositoryMock
                .Setup(x => x.CsvLinesNormalized)
                .Returns(forexRecords);

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

        #region PrepareData_Split1800Provided_ShouldPreserveAllRecords
        [TestMethod]
        public void PrepareData_Split1800Provided_ShouldPreserveAllRecords()
        {
            var data = _service.PrepareData(1800);

            var records = data.Sum(forexDto => forexDto.ForexData.Count);

            Assert.AreEqual(200, records);
        }
        #endregion

        #region PrepareData_Split100Provided_ShouldPreserveAllRecords
        [TestMethod]
        public void PrepareData_Split100Provided_ShouldPreserveAllRecords()
        {
            var data = _service.PrepareData(100);

            var records = data.Sum(forexDto => forexDto.ForexData.Count);

            Assert.AreEqual(200, records);
        }
        #endregion

        #region PrepareData_Split1800Provided_ShouldGetCorrectCountOfSplits
        [TestMethod]
        public void PrepareData_Split1800Provided_ShouldGetCorrectCountOfSplits()
        {
            var data = _service.PrepareData(1800);

            Assert.AreEqual(1, data.Count);
        }
        #endregion

        #region PrepareData_Split900Provided_ShouldGetCorrectCountOfSplits
        [TestMethod]
        public void PrepareData_Split900Provided_ShouldGetCorrectCountOfSplits()
        {
            var data = _service.PrepareData(900);

            Assert.AreEqual(2, data.Count);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldGetCorrectCountOfSplits
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldGetCorrectCountOfSplits()
        {
            var data = _service.PrepareData(60);

            Assert.AreEqual(12, data.Count);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldGetCorrectCountOfRecordsInTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldGetCorrectCountOfRecordsInTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            Assert.AreEqual(10, data[0].ForexData.Count);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldGetCorrectCountOfRecordsInTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldGetCorrectCountOfRecordsInTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            Assert.AreEqual(20, data[1].ForexData.Count);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectBidMovingAveragesOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectBidMovingAveragesOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var averagesExpected = new List<double>
            {
                1.37622, 1.37623, 1.376216667, 1.3762025, 1.376194, 1.376188333, 1.37619, 1.376185, 1.376181111, 1.376178
            };
            var averagesActual = data[0].ForexData.Select(x => x.BidMovingAverage).ToList();
            
            CollectionAssert.AreEqual(averagesExpected, averagesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectBidMovingAveragesOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectBidMovingAveragesOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var averagesExpected = new List<double>
            {
                1.37616, 1.37616, 1.37615, 1.3761125, 1.37609, 1.376051667, 1.376051429, 1.37605125, 1.376044444, 1.375953, 1.375941818, 1.375948333, 1.375938462, 1.375943571, 1.375830667, 1.37574625, 1.375687647, 1.375635556, 1.375595263, 1.375562
            };
            var averagesActual = data[1].ForexData.Select(x => x.BidMovingAverage).ToList();
            
            CollectionAssert.AreEqual(averagesExpected, averagesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectAskMovingAveragesOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectAskMovingAveragesOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var averagesExpected = new List<double>
            {
                1.37693, 1.376955, 1.376956667, 1.3769575, 1.376954, 1.376948333, 1.376948571, 1.376945, 1.376941111, 1.376936
            };
            var averagesActual = data[0].ForexData.Select(x => x.AskMovingAverage).ToList();

            CollectionAssert.AreEqual(averagesExpected, averagesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectAskMovingAveragesOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectAskMovingAveragesOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var averagesExpected = new List<double>
            {
                1.37697, 1.376945, 1.37693, 1.3769, 1.376754, 1.376653333, 1.376584286, 1.37660875, 1.376578889, 1.376552, 1.376521818, 1.376498333, 1.376471538, 1.376495, 1.376417333, 1.376349375, 1.37632, 1.376301667, 1.376285263, 1.3762755
            };
            var averagesActual = data[1].ForexData.Select(x => x.AskMovingAverage).ToList();

            CollectionAssert.AreEqual(averagesExpected, averagesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadsOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadsOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var spreadsExpected = new List<double>
            {
                -0.00071, -0.00074, -0.00077, -0.0008, -0.00078, -0.00076, -0.00075, -0.00077, -0.00076, -0.00074
            };
            var spreadsActual = data[0].ForexData.Select(x => x.Spread).ToList();

            CollectionAssert.AreEqual(spreadsExpected, spreadsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadsOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadsOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var spreadsExpected = new List<double>
            {
                -0.00081, -0.00076, -0.00077, -0.00081, -0.00017, -0.00029, -0.00012, -0.00073, -0.00035, -0.00118, -0.00039, -0.00022, -0.00033, -0.00079, -0.00108, -0.00085, -0.0011, -0.00124, -0.00112, -0.00116
            };
            var spreadsActual = data[1].ForexData.Select(x => x.Spread).ToList();

            CollectionAssert.AreEqual(spreadsExpected, spreadsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadMovingAveragesOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadMovingAveragesOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var averagesExpected = new List<double>
            {
                -0.00071, -0.000725, -0.00074, -0.000755, -0.00076, -0.00076, -0.000758571, -0.00076, -0.00076, -0.000758
            };
            var averagesActual = data[0].ForexData.Select(x => x.SpreadMovingAverage).ToList();

            CollectionAssert.AreEqual(averagesExpected, averagesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadMovingAveragesOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadMovingAveragesOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var averagesExpected = new List<double>
            {
                -0.00081, -0.000785, -0.00078, -0.0007875, -0.000664, -0.000601667, -0.000532857, -0.0005575, -0.000534444, -0.000599, -0.00058, -0.00055, -0.000533077, -0.000551429, -0.000586667, -0.000603125, -0.000632353, -0.000666111, -0.00069, -0.0007135
            };
            var averagesActual = data[1].ForexData.Select(x => x.SpreadMovingAverage).ToList();

            CollectionAssert.AreEqual(averagesExpected, averagesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldPreserveBidsOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldPreserveBidsOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var bidsExpected = new List<double>
            {
                1.37622, 1.37624, 1.37619, 1.37616, 1.37616, 1.37616, 1.3762, 1.37615, 1.37615, 1.37615
            };
            var bidsActual = data[0].ForexData.Select(x => x.Bid).ToList();

            CollectionAssert.AreEqual(bidsExpected, bidsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldPreserveBidsOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldPreserveBidsOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var bidsExpected = new List<double>
            {
                1.37616, 1.37616, 1.37613, 1.376, 1.376, 1.37586, 1.37605, 1.37605, 1.37599, 1.37513, 1.37583, 1.37602, 1.37582, 1.37601, 1.37425, 1.37448, 1.37475, 1.37475, 1.37487, 1.37493
            };
            var bidsActual = data[1].ForexData.Select(x => x.Bid).ToList();

            CollectionAssert.AreEqual(bidsExpected, bidsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldPreserveAsksOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldPreserveAsksOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var asksExpected = new List<double>
            {
                1.37693, 1.37698, 1.37696, 1.37696, 1.37694, 1.37692, 1.37695, 1.37692, 1.37691, 1.37689
            };
            var asksActual = data[0].ForexData.Select(x => x.Ask).ToList();

            CollectionAssert.AreEqual(asksExpected, asksActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldPreserveAsksOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldPreserveAsksOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var asksExpected = new List<double>
            {
                1.37697, 1.37692, 1.3769, 1.37681, 1.37617, 1.37615, 1.37617, 1.37678, 1.37634, 1.37631, 1.37622, 1.37624, 1.37615, 1.3768, 1.37533, 1.37533, 1.37585, 1.37599, 1.37599, 1.37609
            };
            var asksActual = data[1].ForexData.Select(x => x.Ask).ToList();

            CollectionAssert.AreEqual(asksExpected, asksActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectBidChangesOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectBidChangesOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var changesExpected = new List<double>
            {
                0.000000000, 0.000014533, -0.000036331, -0.000021799, 0.000000000, 0.000000000, 0.000029066, -0.000036332, 0.000000000, 0.000000000
            };
            var changesActual = data[0].ForexData.Select(x => x.BidChange).ToList();

            CollectionAssert.AreEqual(changesExpected, changesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectBidChangesOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectBidChangesOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var changesExpected = new List<double>
            {
                0.000007267, 0.000000000, -0.000021800, -0.000094468, 0.000000000, -0.000101744, 0.000138095, 0.000000000, -0.000043603, -0.000625005, 0.000509043, 0.000138098, -0.000145347, 0.000138099, -0.001279060, 0.000167364, 0.000196438, 0.000000000, 0.000087289, 0.000043640
            };
            var changesActual = data[1].ForexData.Select(x => x.BidChange).ToList();

            CollectionAssert.AreEqual(changesExpected, changesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectAskChangesOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectAskChangesOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var changesExpected = new List<double>
            {
                0.000000000, 0.000036313, -0.000014525, 0.000000000, -0.000014525, -0.000014525, 0.000021788, -0.000021787, -0.000007263, -0.000014525
            };
            var changesActual = data[0].ForexData.Select(x => x.AskChange).ToList();

            CollectionAssert.AreEqual(changesExpected, changesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectAskChangesOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectAskChangesOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var changesExpected = new List<double>
            {
                0.000058102, -0.000036312, -0.000014525, -0.000065364, -0.000464843, -0.000014533, 0.000014533, 0.000443259, -0.000319586, -0.000021797, -0.000065392, 0.000014533, -0.000065396, 0.000472332, -0.001067693, 0.000000000, 0.000378091, 0.000101755, 0.000000000, 0.000072675
            };
            var changesActual = data[1].ForexData.Select(x => x.AskChange).ToList();

            CollectionAssert.AreEqual(changesExpected, changesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadChangesOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadChangesOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var changesExpected = new List<double>
            {
                0.000000000, 0.042253521, 0.040540541, 0.038961039, -0.025000000, -0.025641026, -0.013157895, 0.026666667, -0.012987013, -0.026315789
            };
            var changesActual = data[0].ForexData.Select(x => x.SpreadChange).ToList();

            CollectionAssert.AreEqual(changesExpected, changesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadChangesOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadChangesOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var changesExpected = new List<double>
            {
                0.094594595, -0.061728395, 0.013157895, 0.051948052, -0.790123457, 0.705882353, -0.586206897, 5.083333333, -0.520547945, 2.371428571, -0.669491525, -0.435897436, 0.500000000, 1.393939394, 0.367088608, -0.212962963, 0.294117647, 0.127272727, -0.096774194, 0.035714286
            };
            var changesActual = data[1].ForexData.Select(x => x.SpreadChange).ToList();

            CollectionAssert.AreEqual(changesExpected, changesActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectBidStdDevOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectBidStdDevOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var deviationsExpected = new List<double>
            {
                0.000000000, 0.000010000, 0.000020548, 0.000030311, 0.000032000, 0.000031842, 0.000029761, 0.000030822, 0.000031071, 0.000030919
            };
            var deviationsActual = data[0].ForexData.Select(x => x.BidStandardDeviation).ToList();

            CollectionAssert.AreEqual(deviationsExpected, deviationsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectBidStdDevOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectBidStdDevOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var deviationsExpected = new List<double>
            {
                0.000000000, 0.000000000, 0.000014142, 0.000066097, 0.000074297, 0.000109303, 0.000101197, 0.000094662, 0.000091301, 0.000287682, 0.000276564, 0.000265670, 0.000257528, 0.000248843, 0.000486065, 0.000573050, 0.000603339, 0.000624440, 0.000631368, 0.000632231
            };
            var deviationsActual = data[1].ForexData.Select(x => x.BidStandardDeviation).ToList();

            CollectionAssert.AreEqual(deviationsExpected, deviationsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectAskStdDevOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectAskStdDevOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var deviationsExpected = new List<double>
            {
                0.000000000, 0.000025000, 0.000020548, 0.000017854, 0.000017436, 0.000020344, 0.000018844, 0.000020000, 0.000021830, 0.000025768
            };
            var deviationsActual = data[0].ForexData.Select(x => x.AskStandardDeviation).ToList();

            CollectionAssert.AreEqual(deviationsExpected, deviationsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectAskStdDevOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectAskStdDevOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var deviationsExpected = new List<double>
            {
                0.000000000, 0.000025000, 0.000029439, 0.000057879, 0.000296554, 0.000352073, 0.000367223, 0.000349551, 0.000340211, 0.000332680, 0.000331246, 0.000326569, 0.000327199, 0.000326447, 0.000428851, 0.000491623, 0.000491205, 0.000483313, 0.000475543, 0.000465451
            };
            var deviationsActual = data[1].ForexData.Select(x => x.AskStandardDeviation).ToList();

            CollectionAssert.AreEqual(deviationsExpected, deviationsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadStdDevOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadStdDevOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            var deviationsExpected = new List<double>
            {
                0.000000000, 0.000015000, 0.000024495, 0.000033541, 0.000031623, 0.000028868, 0.000026954, 0.000025495, 0.000024037, 0.000023580
            };
            var deviationsActual = data[0].ForexData.Select(x => x.SpreadStandardDeviation).ToList();

            CollectionAssert.AreEqual(deviationsExpected, deviationsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldCalculateCorrectSpreadStdDevOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldCalculateCorrectSpreadStdDevOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            var deviationsExpected = new List<double>
            {
                0.000000000, 0.000025000, 0.000021602, 0.000022776, 0.000247839, 0.000265733, 0.000298219, 0.000286476, 0.000277853, 0.000327092, 0.000317605, 0.000319948, 0.000312936, 0.000308727, 0.000326102, 0.000322116, 0.000333652, 0.000352864, 0.000358094, 0.000363748
            };
            var deviationsActual = data[1].ForexData.Select(x => x.SpreadStandardDeviation).ToList();

            CollectionAssert.AreEqual(deviationsExpected, deviationsActual);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldGetCorrectNameOfTheFirstSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldGetCorrectNameOfTheFirstSplit()
        {
            var data = _service.PrepareData(60);

            Assert.AreEqual("0", data[0].FileName);
        }
        #endregion

        #region PrepareData_Split60Provided_ShouldGetCorrectNameOfTheSecondSplit
        [TestMethod]
        public void PrepareData_Split60Provided_ShouldGetCorrectNameOfTheSecondSplit()
        {
            var data = _service.PrepareData(60);

            Assert.AreEqual("1", data[1].FileName);
        }
        #endregion

        #region PrepareData_Split1800Provided_BuyAndSellQuantityMustBeTheSame
        [TestMethod]
        public void PrepareData_Split1800Provided_BuyAndSellQuantityMustBeTheSame()
        {
            var data = _service.PrepareData(1800);

            var allActions = data[0].ForexData.Select(x => x.Action.ToString()).ToList();
            var buyActions = allActions.Count(x => x == "Buy");
            var sellActions = allActions.Count(x => x == "Sell");

            Assert.AreEqual(buyActions, sellActions);
        }
        #endregion

        #region PrepareData_Split1800ProvidedAndMiniTradingMade_ShouldMakeProfit
        [TestMethod]
        public void PrepareData_Split1800ProvidedAndMiniTradingMade_ShouldMakeProfit()
        {
            var data = _service.PrepareData(1800);
            var forexTreeData = data[0].ForexData;

            const double spending = 10000.0;
            const double margin = 0.02;
            const double leverage = 1.0 / margin;
            var eurosSpent = 0.0;
            var profit = 0.0;
            List<double> dollarsList = new List<double>();
            foreach (var record in forexTreeData)
            {
                double dollars;
                switch (record.Action)
                {
                    case MarketAction.Hold:
                        continue;
                    case MarketAction.Buy:
                        eurosSpent += spending;
                        dollars = spending * leverage * record.Bid;
                        dollarsList.Add(MathHelpers.PreservePrecision(dollars));
                        continue;
                }

                dollars = dollarsList.First();
                dollarsList.RemoveAt(0);
                profit += dollars / record.Ask - spending * leverage;
                profit = MathHelpers.CurrencyPrecision(profit);
            }

            if (profit < 0)
            {
                Assert.Fail();
            }

            var eurosNow = eurosSpent + profit;

            Assert.AreEqual(460000.0, eurosSpent);
            Assert.AreEqual(462671.64, eurosNow);
        }
        #endregion

        #endregion

        #endregion

    }
}
