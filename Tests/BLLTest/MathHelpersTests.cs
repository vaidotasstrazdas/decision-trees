#region Usings
using Implementation.BLL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Tests.BLLTest
{
    [TestClass]
    public class MathHelpersTests
    {

        #region TestSuite

        #region PreservePrecision_ShouldCorrectlyAddNumbers
        [TestMethod]
        public void PreservePrecision_ShouldCorrectlyAddNumbers()
        {
            var result = MathHelpers.PreservePrecision(1.0/6.0 + 1.0/6.0 + 1.0/6.0 + 1.0/6.0 + 1.0/6.0 + 1.0/6.0);
            Assert.AreEqual(1.0, result);
        }
        #endregion

        #endregion

    }
}
