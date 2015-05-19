#region Usings
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bridge.IDLL.Exceptions;
using Implementation.DLL;
#endregion

namespace Tests.DLLTest
{
    [TestClass]
    public class HistogramResultsRepositoryTests
    {

        #region Private Fields
        private HistogramResultsRepository _repository;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new HistogramResultsRepository();
        }
        #endregion

        #region TestSuite

        #region Save Tests

        #region Save_NullPathProvided_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Path can not be null or empty.")]
        public void Save_NullPathProvided_ShouldThrowDalException()
        {
            _repository.Save(null);
        }
        #endregion

        #region Save_EmptyPathProvided_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Path can not be null or empty.")]
        public void Save_EmptyPathProvided_ShouldThrowDalException()
        {
            _repository.Save(string.Empty);
        }
        #endregion

        #endregion

        #endregion

    }
}
