using Bridge.IDLL.Data;
using Implementation.BLL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.BLLTest
{
    [TestClass]
    public class ForexHelperTests
    {

        [TestMethod]
        public void PreviousSpreadIsZero_ShoudGetCorrectSpreadChange()
        {
            var record = new ForexRecord
            {
                Ask = 1.02,
                Bid = 1.01,
                CurrencyPair = "EURUSD",
                Date = "123"
            };

            var options = new ForexTrackData
            {
                PreviousSpread = 0.0
            };

            var forexTreeData = ForexHelper.BuildForexTreeRecord(record, options);

            Assert.AreEqual(0.0, forexTreeData.SpreadChange);
        }

    }
}
