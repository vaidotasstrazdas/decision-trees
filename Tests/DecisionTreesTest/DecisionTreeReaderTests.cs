#region Usings
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.DecisionTrees;
#endregion

namespace Tests.DecisionTreesTest
{
    [TestClass]
    public class DecisionTreeReaderTests
    {

        #region Private Fields
        private DecisionTreeReader _reader;
        private string _normalizedTreeSource;
        private string _treeWithSubTrees;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _reader = new DecisionTreeReader();

            _normalizedTreeSource = File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["TestDataDirectory"], "C4.5NoSubTrees.txt"));
            _treeWithSubTrees = File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["TestDataDirectory"], "C4.5SubTrees.txt"));

        }
        #endregion

        #region TestSuite

        #region ReadSubTrees Tests

        #region ReadSubTrees_ShouldGetCorrectCountOfSubTrees
        [TestMethod]
        public void ReadSubTrees_ShouldGetCorrectCountOfSubTrees()
        {
            _reader.ReadSubTrees(_treeWithSubTrees);

            Assert.AreEqual(4, _reader.SubTrees.Count);
        }
        #endregion

        #region ReadSubTrees_ShoulgGetCorrectKeysOfSubTrees
        [TestMethod]
        public void ReadSubTrees_ShoulgGetCorrectKeysOfSubTrees()
        {
            _reader.ReadSubTrees(_treeWithSubTrees);

            var keysExpected = new List<string> { "[S1]", "[S2]", "[S3]", "[S4]" };

            CollectionAssert.AreEqual(keysExpected, _reader.SubTrees.Keys);
        }
        #endregion

        #region ReadSubTrees_ShouldGetCorrectS1SubTree
        [TestMethod]
        public void ReadSubTrees_ShouldGetCorrectS1SubTree()
        {
            _reader.ReadSubTrees(_treeWithSubTrees);

            var builder = new StringBuilder();
            builder.AppendLine("BidChange <= -2.196e-05 : Sell (3.0/1.0)");
            builder.AppendLine("BidChange > -2.196e-05 :[S2]");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S1]"]);
        }
        #endregion

        #region ReadSubTrees_ShouldGetCorrectS2SubTree
        [TestMethod]
        public void ReadSubTrees_ShouldGetCorrectS2SubTree()
        {
            _reader.ReadSubTrees(_treeWithSubTrees);

            var builder = new StringBuilder();
            builder.AppendLine("Bid <= 1.3661 : Hold (58.0/7.0)");
            builder.AppendLine("Bid > 1.3661 :[S3]");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S2]"]);
        }
        #endregion

        #region ReadSubTrees_ShouldGetCorrectS3SubTree
        [TestMethod]
        public void ReadSubTrees_ShouldGetCorrectS3SubTree()
        {
            _reader.ReadSubTrees(_treeWithSubTrees);

            var builder = new StringBuilder();
            builder.AppendLine("Ask <= 1.36612 : Sell (5.0/1.0)");
            builder.AppendLine("Ask > 1.36612 : Hold (7.0)");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S3]"]);
        }
        #endregion

        #region ReadSubTrees_ShouldGetCorrectS4SubTree
        [TestMethod]
        public void ReadSubTrees_ShouldGetCorrectS4SubTree()
        {
            _reader.ReadSubTrees(_treeWithSubTrees);

            var builder = new StringBuilder();
            builder.AppendLine("Bid <= 1.3661 : Hold (13.0/1.0)");
            builder.AppendLine("Bid > 1.3661 : Buy (4.0/1.0)");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S4]"]);
        }
        #endregion

        #endregion

        #region NormalizeTree Tests

        #region NormalizeTree_TreeWithSubTreesProvided_ShouldBeTheSameAsNormalizedTreeSource
        [TestMethod]
        public void NormalizeTree_TreeWithSubTreesProvided_ShouldBeTheSameAsNormalizedTreeSource()
        {
            _reader.ReadSubTrees(_treeWithSubTrees);
            var normalizedTree = _reader.NormalizeTree(_treeWithSubTrees);

            Assert.AreEqual(normalizedTree, _normalizedTreeSource);
        }
        #endregion

        #endregion

        #endregion


    }
}
