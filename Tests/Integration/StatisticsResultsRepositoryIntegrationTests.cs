#region Usings
using System.Configuration;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bridge.IDLL.Data;
using Implementation.DLL;
#endregion

namespace Tests.Integration
{
    [TestClass]
    public class StatisticsResultsRepositoryIntegrationTests
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

        #region Add and Save Tests

        #region AddThreeElementsAndSaveThem
        [TestMethod]
        [TestCategory("Integration")]
        public void AddThreeElementsAndSaveThem()
        {
            var resultsFilePath = Path.Combine(ConfigurationManager.AppSettings["TestDataDirectory"], "StatisticsResults.csv");
            var builder = new StringBuilder();
            builder.AppendLine("BluePrint,C45Better,C50Better,Equal");
            builder.AppendLine("test,123,321,10");
            builder.AppendLine("test1,113,311,11");
            builder.AppendLine("test2,153,351,15");

            _repository.Add(new StatisticsResult { BluePrint = "test", C45Better = 123, C50Better = 321, Equal = 10 });
            _repository.Add(new StatisticsResult { BluePrint = "test1", C45Better = 113, C50Better = 311, Equal = 11 });
            _repository.Add(new StatisticsResult { BluePrint = "test2", C45Better = 153, C50Better = 351, Equal = 15 });

            _repository.Save(resultsFilePath);

            var result = File.ReadAllText(resultsFilePath);

            Assert.AreEqual(builder.ToString(), result);
        }
        #endregion

        #endregion

        #endregion

    }
}
