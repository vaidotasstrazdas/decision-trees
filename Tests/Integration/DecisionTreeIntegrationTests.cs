#region Usings
using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Implementation.DLL.RepositoryBase;
using Shared.DecisionTrees;
using Shared.DecisionTrees.DataStructure;
using Tests.Integration.Data;
#endregion

namespace Tests.Integration
{
    [TestClass]
    public class DecisionTreeIntegrationTests
    {

        #region Private Fields
        private static DecisionTree<ForexTreeData> _tree;
        private static CsvDataRepository<ForexTreeData> _repository;
        #endregion

        #region ClassInitialize
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var dataDirectory = ConfigurationManager.AppSettings["TestDataDirectory"];
            var c45Source = File.ReadAllText(Path.Combine(dataDirectory, "C4.5.txt"));
            var forexTreePath = Path.Combine(ConfigurationManager.AppSettings["TestDataDirectory"], "ForexTree.data");
            _repository = new CsvDataRepository<ForexTreeData>();
            _repository.LoadData(forexTreePath);
            _repository.NormalizeData(0);

            _tree = new DecisionTree<ForexTreeData>(
                        new DecisionTreeReader(),
                        new RuleBuilder(),
                        new Classifier<ForexTreeData>()
                    );

            _tree.SaveDecisionTree(c45Source);
        }
        #endregion

        #region TestSuite

        #region ShouldGetCorrectNumberOfClassificationErrors
        [TestMethod]
        [TestCategory("Integration")]
        public void ShouldGetCorrectNumberOfClassificationErrors()
        {
            var errors = 0;
            foreach (var record in _repository.CsvLinesNormalized)
            {
                var action = _tree.ClassifyRecord(record);
                var marketAction = (MarketAction) Enum.Parse(typeof (MarketAction), record.Action);
                if (action != marketAction)
                {
                    errors++;
                }
            }

            Assert.AreEqual(427, errors);
        }
        #endregion

        #endregion

    }
}
