#region Usings
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bridge.IBLL.Data;
using Bridge.IBLL.Data.Base;
using Implementation.BLL.Helpers;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class YahooHelperTests
    {

        #region Private Fields
        private List<YahooNormalized> _yahooRecords;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _yahooRecords = new List<YahooNormalized>
            {
                new YahooNormalized
                {
                    Close = 2106.8501,
                    Volatility = 1.25
                },
                new YahooNormalized
                {
                    Close = 2114.76001,
                    Volatility = 1.26
                },
                new YahooNormalized
                {
                    Close = 2108.91992,
                    Volatility = 1.27
                },
                new YahooNormalized
                {
                    Close = 2117.68994,
                    Volatility = 1.28
                },
                new YahooNormalized
                {
                    Close = 2112.92993,
                    Volatility = 1.29
                }
            };
        }
        #endregion

        #region TestSuite

        #region BuildYahooTreeDataList Tests

        #region BuildYahooTreeDataList_ShouldGetCorrectCount
        [TestMethod] public void BuildYahooTreeDataList_ShouldGetCorrectCount()
        {
            var yahooTreeDataList = YahooHelper.BuildYahooTreeDataList(_yahooRecords).ToList();

            Assert.AreEqual(5, yahooTreeDataList.Count);
        }
        #endregion

        #region BuildYahooTreeDataList_ShouldPreserveVolatilities
        [TestMethod]
        public void BuildYahooTreeDataList_ShouldPreserveVolatilities()
        {
            var yahooTreeDataList = YahooHelper.BuildYahooTreeDataList(_yahooRecords).ToList();

            Assert.AreEqual(1.25, yahooTreeDataList[0].Volatility);
            Assert.AreEqual(1.26, yahooTreeDataList[1].Volatility);
            Assert.AreEqual(1.27, yahooTreeDataList[2].Volatility);
            Assert.AreEqual(1.28, yahooTreeDataList[3].Volatility);
            Assert.AreEqual(1.29, yahooTreeDataList[4].Volatility);
        }
        #endregion

        #region BuildYahooTreeDataList_ShouldSaveCorrectActions
        [TestMethod]
        public void BuildYahooTreeDataList_ShouldSaveCorrectActions()
        {
            var yahooTreeDataList = YahooHelper.BuildYahooTreeDataList(_yahooRecords).ToList();

            Assert.AreEqual(MarketAction.Buy, yahooTreeDataList[0].Action);
            Assert.AreEqual(MarketAction.Sell, yahooTreeDataList[1].Action);
            Assert.AreEqual(MarketAction.Buy, yahooTreeDataList[2].Action);
            Assert.AreEqual(MarketAction.Sell, yahooTreeDataList[3].Action);
            Assert.AreEqual(MarketAction.Hold, yahooTreeDataList[4].Action);
        }
        #endregion

        #endregion

        #region BuildYahooNamesFile Tests

        #region BuildYahooNamesFile_ShouldBuildCorrectFile
        [TestMethod]
        public void BuildYahooNamesFile_ShouldBuildCorrectFile()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Buy,Sell,Hold.	|classes");
            builder.AppendLine();
            builder.AppendLine("Volatility:	continuous.");

            var namesFile = YahooHelper.BuildYahooNamesFile();

            Assert.AreEqual(builder.ToString(), namesFile);
        }
        #endregion

        #endregion

        #endregion

    }
}
