#region Usings
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Implementation.DLL;
#endregion

namespace Tests.DLLTest
{

    [TestClass]
    public class ForexMarketPathRepositoryTests
    {

        #region Private Fields
        private ForexMarketPathRepository _repository;
        private string _pathDirectory;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _pathDirectory = Path.Combine(ConfigurationManager.AppSettings["TestDataDirectory"], "ForexTrees");
            _repository = new ForexMarketPathRepository
            {
                ForexTreesPath = _pathDirectory
            };
        }
        #endregion

        #region TestSuite

        #region SetPaths Tests

        #region SetPaths_GetAllPaths_ShouldSetCorrectNumberOfPaths
        [TestMethod]
        public void SetPaths_GetAllPaths_ShouldSetCorrectNumberOfPaths()
        {
            _repository.SetPaths(1, "300", 0);

            Assert.AreEqual(6, _repository.Paths.Count);
        }
        #endregion

        #region SetPaths_StartingChunkSelected_ShouldSetCorrectNumberOfPaths
        [TestMethod]
        public void SetPaths_StartingChunkSelected_ShouldSetCorrectNumberOfPaths()
        {
            _repository.SetPaths(1, "300", 2);

            Assert.AreEqual(4, _repository.Paths.Count);
        }
        #endregion

        #region SetPaths_StartingMonthSelected_ShouldSetCorrectNumberOfPaths
        [TestMethod]
        public void SetPaths_StartingMonthSelected_ShouldSetCorrectNumberOfPaths()
        {
            _repository.SetPaths(2, "300", 0);

            Assert.AreEqual(3, _repository.Paths.Count);
        }
        #endregion

        #region SetPaths_StartingMonthAndChunkSelected_ShouldSetCorrectNumberOfPaths
        [TestMethod]
        public void SetPaths_StartingMonthAndChunkSelected_ShouldSetCorrectNumberOfPaths()
        {
            _repository.SetPaths(2, "300", 2);

            Assert.AreEqual(1, _repository.Paths.Count);
        }
        #endregion

        #region SetPaths_GetAllPaths_ShouldSetPathsInCorrectOrder
        [TestMethod]
        public void SetPaths_GetAllPaths_ShouldSetPathsInCorrectOrder()
        {
            _repository.SetPaths(1, "300", 0);

            var pathsExpected = new List<string>
            {
                "\\01\\300\\Forex_0.data",
                "\\01\\300\\Forex_1.data",
                "\\01\\300\\Forex_2.data",
                "\\02\\300\\Forex_0.data",
                "\\02\\300\\Forex_1.data",
                "\\02\\300\\Forex_2.data"
            };
            var pathsActual = _repository.Paths.Select(x => x.Replace(_pathDirectory, string.Empty)).ToList();

            CollectionAssert.AreEqual(pathsExpected, pathsActual);
        }
        #endregion

        #region SetPaths_StartingChunkSelected_ShouldSetPathsInCorrectOrder
        [TestMethod]
        public void SetPaths_StartingChunkSelected_ShouldSetPathsInCorrectOrder()
        {
            _repository.SetPaths(1, "300", 2);

            var pathsExpected = new List<string>
            {
                "\\01\\300\\Forex_2.data",
                "\\02\\300\\Forex_0.data",
                "\\02\\300\\Forex_1.data",
                "\\02\\300\\Forex_2.data"
            };
            var pathsActual = _repository.Paths.Select(x => x.Replace(_pathDirectory, string.Empty)).ToList();

            CollectionAssert.AreEqual(pathsExpected, pathsActual);
        }
        #endregion

        #region SetPaths_StartingMonthSelected_ShouldSetPathsInCorrectOrder
        [TestMethod]
        public void SetPaths_StartingMonthSelected_ShouldSetPathsInCorrectOrder()
        {
            _repository.SetPaths(2, "300", 0);

            var pathsExpected = new List<string>
            {
                "\\02\\300\\Forex_0.data",
                "\\02\\300\\Forex_1.data",
                "\\02\\300\\Forex_2.data"
            };
            var pathsActual = _repository.Paths.Select(x => x.Replace(_pathDirectory, string.Empty)).ToList();

            CollectionAssert.AreEqual(pathsExpected, pathsActual);
        }
        #endregion

        #region SetPaths_StartingMonthAndChunkSelected_ShouldSetPathsInCorrectOrder
        [TestMethod]
        public void SetPaths_StartingMonthAndChunkSelected_ShouldSetPathsInCorrectOrder()
        {
            _repository.SetPaths(2, "300", 2);

            var pathsExpected = new List<string>
            {
                "\\02\\300\\Forex_2.data"
            };
            var pathsActual = _repository.Paths.Select(x => x.Replace(_pathDirectory, string.Empty)).ToList();

            CollectionAssert.AreEqual(pathsExpected, pathsActual);
        }
        #endregion

        #endregion

        #region GetChunk Tests

        #region GetChunk_ShouldGetCorrectChunkNumber
        [TestMethod]
        public void GetChunk_ShouldGetCorrectChunkNumber()
        {
            var chunk = _repository.GetChunk("\\Forex_123.data");

            Assert.AreEqual(123, chunk);
        }
        #endregion

        #endregion

        #endregion

    }

}
