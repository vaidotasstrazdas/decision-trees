#region Usings
using System.Configuration;
using System.IO;
using Bridge.IDLL.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Implementation.DLL;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Tests.DLLTest
{

    [TestClass]
    public class DecisionTreesRepositoryTests
    {

        #region Private Fields
        private string _sourcePath;
        private DecisionTreesRepository _repository;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _sourcePath = Path.Combine(ConfigurationManager.AppSettings["TestDataDirectory"], "ForexTrees");
            _repository = new DecisionTreesRepository
            {
                DecisionTreesPath = _sourcePath
            };
        }
        #endregion

        #region TestSuite

        #region ReadSource Tests

        #region ReadSource_IncorrectAlgorithmProvided_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Incorrect algorithm provided.")]
        public void ReadSource_IncorrectAlgorithmProvided_ShouldThrowDalException()
        {
            _repository.ReadSource("300", 1, 0, default(DecisionTreeAlgorithm));
        }
        #endregion

        #region ReadSource_M1P300CH0C45_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P300CH0C45_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_300_0_C45.txt"));
            var sourceActual = _repository.ReadSource("300", 1, 0, DecisionTreeAlgorithm.C45);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P300CH1C45_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P300CH1C45_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_300_1_C45.txt"));
            var sourceActual = _repository.ReadSource("300", 1, 1, DecisionTreeAlgorithm.C45);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P300CH2C45_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P300CH2C45_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_300_2_C45.txt"));
            var sourceActual = _repository.ReadSource("300", 1, 2, DecisionTreeAlgorithm.C45);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P600CH0C45_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P600CH0C45_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_600_0_C45.txt"));
            var sourceActual = _repository.ReadSource("600", 1, 0, DecisionTreeAlgorithm.C45);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P600CH1C45_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P600CH1C45_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_600_1_C45.txt"));
            var sourceActual = _repository.ReadSource("600", 1, 1, DecisionTreeAlgorithm.C45);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P600CH2C45_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P600CH2C45_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_600_2_C45.txt"));
            var sourceActual = _repository.ReadSource("600", 1, 2, DecisionTreeAlgorithm.C45);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P300CH0C50_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P300CH0C50_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_300_0_C50.txt"));
            var sourceActual = _repository.ReadSource("300", 1, 0, DecisionTreeAlgorithm.C50);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P300CH1C50_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P300CH1C50_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_300_1_C50.txt"));
            var sourceActual = _repository.ReadSource("300", 1, 1, DecisionTreeAlgorithm.C50);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P300CH2C50_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P300CH2C50_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_300_2_C50.txt"));
            var sourceActual = _repository.ReadSource("300", 1, 2, DecisionTreeAlgorithm.C50);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P600CH0C50_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P600CH0C50_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_600_0_C50.txt"));
            var sourceActual = _repository.ReadSource("600", 1, 0, DecisionTreeAlgorithm.C50);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P600CH1C50_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P600CH1C50_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_600_1_C50.txt"));
            var sourceActual = _repository.ReadSource("600", 1, 1, DecisionTreeAlgorithm.C50);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #region ReadSource_M1P600C2CH50_ShouldReadSource
        [TestMethod]
        public void ReadSource_M1P600C2CH50_ShouldReadSource()
        {
            var sourceExpetced = File.ReadAllText(Path.Combine(_sourcePath, "01_600_2_C50.txt"));
            var sourceActual = _repository.ReadSource("600", 1, 2, DecisionTreeAlgorithm.C50);

            Assert.AreEqual(sourceExpetced, sourceActual);
        }
        #endregion

        #endregion

        #endregion

    }

}
