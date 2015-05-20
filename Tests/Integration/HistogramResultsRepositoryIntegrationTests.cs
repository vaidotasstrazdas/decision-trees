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
    public class HistogramResultsRepositoryIntegrationTests
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

        #region Add and Save Tests

        #region AddThreeElementsAndSaveThem_HistogramResult
        [TestMethod]
        [TestCategory("Integration")]
        public void AddThreeElementsAndSaveThem_HistogramResult()
        {
            var resultsFilePath = Path.Combine(ConfigurationManager.AppSettings["TestDataDirectory"], "StatisticsResults.csv");
            var builder = new StringBuilder();
            builder.AppendLine("BluePrint,C45Cases,C50Cases");
            builder.AppendLine("\"0-0.00125\",123,321");
            builder.AppendLine("\"0.00125-0.0025\",113,311");
            builder.AppendLine("\"0.0025-0.00375\",153,351");

            _repository.Add(new HistogramResult { BluePrint = "0-0.00125", C45Cases = 123, C50Cases = 321 });
            _repository.Add(new HistogramResult { BluePrint = "0.00125-0.0025", C45Cases = 113, C50Cases = 311 });
            _repository.Add(new HistogramResult { BluePrint = "0.0025-0.00375", C45Cases = 153, C50Cases = 351 });

            _repository.Save(resultsFilePath);

            var result = File.ReadAllText(resultsFilePath);

            Assert.AreEqual(builder.ToString(), result);
        }
        #endregion

        #endregion

        #endregion

    }
}
