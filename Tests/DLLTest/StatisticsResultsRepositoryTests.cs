#region Usings
using Bridge.IDLL.Exceptions;
using Implementation.DLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Tests.DLLTest
{
    [TestClass]
    public class StatisticsResultsRepositoryTests
    {

        #region Private Fields
        private StatisticsResultsRepository _repository;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new StatisticsResultsRepository();
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
