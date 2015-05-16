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
        private string _c45NoSubTrees;
        private string _c45WithSubTrees;
        private string _c45NormalizedNoSubTrees;
        private string _c45NormalizedWithSubTrees;
        private string _c50NoSubTrees;
        private string _c50WithSubTrees;
        private string _c50NormalizedNoSubTrees;
        private string _c50NormalizedWithSubTrees;

        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _reader = new DecisionTreeReader();
            var dataDirectory = ConfigurationManager.AppSettings["TestDataDirectory"];

            _c45NoSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C4.5NoSubTrees.txt"));
            _c45WithSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C4.5SubTrees.txt"));
            _c45NormalizedNoSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C4.5NormalizedNoSubTrees.txt"));
            _c45NormalizedWithSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C4.5NormalizedSubTrees.txt"));
            _c50NoSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C5.0NoSubTrees.txt"));
            _c50WithSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C5.0SubTrees.txt"));
            _c50NormalizedNoSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C5.0NormalizedNoSubTrees.txt"));
            _c50NormalizedWithSubTrees = File.ReadAllText(Path.Combine(dataDirectory, "C5.0NormalizedSubTrees.txt"));

        }
        #endregion

        #region TestSuite

        #region NormalizeTreeSource Tests

        #region NormalizeTreeSource_C45TreeProvidedWithSubTrees_ShouldNormalizeC45
        [TestMethod]
        public void NormalizeTreeSource_C45TreeProvidedWithSubTrees_ShouldNormalizeC45()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);

            Assert.AreEqual(_c45NormalizedWithSubTrees, normalizedTree);
        }
        #endregion

        #region NormalizeTreeSource_C45TreeProvidedWithNotSubTrees_ShouldNormalizeC45
        [TestMethod]
        public void NormalizeTreeSource_C45TreeProvidedWithNotSubTrees_ShouldNormalizeC45()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45NoSubTrees);

            Assert.AreEqual(_c45NormalizedNoSubTrees, normalizedTree);
        }
        #endregion

        #region NormalizeTreeSource_C50TreeProvidedWithSubTrees_ShouldNormalizeC50
        [TestMethod]
        public void NormalizeTreeSource_C50TreeProvidedWithSubTrees_ShouldNormalizeC50()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);

            Assert.AreEqual(_c50NormalizedWithSubTrees, normalizedTree);
        }
        #endregion

        #region NormalizeTreeSource_C50TreeProvidedWithNotSubTrees_ShouldNormalizeC50
        [TestMethod]
        public void NormalizeTreeSource_C50TreeProvidedWithNotSubTrees_ShouldNormalizeC50()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50NoSubTrees);

            Assert.AreEqual(_c50NormalizedNoSubTrees, normalizedTree);
        }
        #endregion

        #endregion

        #region ReadSubTrees Tests

        #region ReadSubTrees_C45_ShouldGetCorrectCountOfSubTrees
        [TestMethod]
        public void ReadSubTrees_C45_ShouldGetCorrectCountOfSubTrees()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            Assert.AreEqual(4, _reader.SubTrees.Count);
        }
        #endregion

        #region ReadSubTrees_C50_ShouldGetCorrectCountOfSubTrees
        [TestMethod]
        public void ReadSubTrees_C50_ShouldGetCorrectCountOfSubTrees()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            Assert.AreEqual(5, _reader.SubTrees.Count);
        }
        #endregion

        #region ReadSubTrees_C45_ShouldGetCorrectKeysOfSubTrees
        [TestMethod]
        public void ReadSubTrees_C45_ShouldGetCorrectKeysOfSubTrees()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var keysExpected = new List<string> { "[S1]", "[S2]", "[S3]", "[S4]" };

            CollectionAssert.AreEqual(keysExpected, _reader.SubTrees.Keys);
        }
        #endregion

        #region ReadSubTrees_C50_ShouldGetCorrectKeysOfSubTrees
        [TestMethod]
        public void ReadSubTrees_C50_ShouldGetCorrectKeysOfSubTrees()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var keysExpected = new List<string> { "[S1]", "[S2]", "[S3]", "[S4]", "[S5]" };

            CollectionAssert.AreEqual(keysExpected, _reader.SubTrees.Keys);
        }
        #endregion

        #region ReadSubTrees_C45_ShouldGetCorrectS1SubTree
        [TestMethod]
        public void ReadSubTrees_C45_ShouldGetCorrectS1SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("BidChange <= -2.196e-05:Sell (3.0/1.0)");
            builder.AppendLine("BidChange > -2.196e-05:[S2]");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S1]"]);
        }
        #endregion

        #region ReadSubTrees_C50_ShouldGetCorrectS1SubTree
        [TestMethod]
        public void ReadSubTrees_C50_ShouldGetCorrectS1SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("BidMovingAverage <= 1.367662:Buy (25)");
            builder.AppendLine("BidMovingAverage > 1.367662:Hold (8/1)");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S1]"]);
        }
        #endregion

        #region ReadSubTrees_C45_ShouldGetCorrectS2SubTree
        [TestMethod]
        public void ReadSubTrees_C45_ShouldGetCorrectS2SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("Bid <= 1.3661:Hold (58.0/7.0)");
            builder.AppendLine("Bid > 1.3661:[S3]");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S2]"]);
        }
        #endregion

        #region ReadSubTrees_C50_ShouldGetCorrectS2SubTree
        [TestMethod]
        public void ReadSubTrees_C50_ShouldGetCorrectS2SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("BidMovingAverage > 1.367597:[S3]");
            builder.AppendLine("BidMovingAverage <= 1.367597:");
            builder.AppendLine("    Bid > 1.36746:Buy (43/5)");
            builder.AppendLine("    Bid <= 1.36746:[S4]");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S2]"]);
        }
        #endregion

        #region ReadSubTrees_C45_ShouldGetCorrectS3SubTree
        [TestMethod]
        public void ReadSubTrees_C45_ShouldGetCorrectS3SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("Ask <= 1.36612:Sell (5.0/1.0)");
            builder.AppendLine("Ask > 1.36612:Hold (7.0)");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S3]"]);
        }
        #endregion

        #region ReadSubTrees_C50_ShouldGetCorrectS3SubTree
        [TestMethod]
        public void ReadSubTrees_C50_ShouldGetCorrectS3SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("BidStandardDeviation <= 4.3927e-05:Buy (15/1)");
            builder.AppendLine("BidStandardDeviation > 4.3927e-05:");
            builder.AppendLine("    Ask <= 1.36754:Sell (28)");
            builder.AppendLine("    Ask > 1.36754:");
            builder.AppendLine("        Ask <= 1.3676:Buy (9)");
            builder.AppendLine("        Ask > 1.3676:Sell (20)");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S3]"]);
        }
        #endregion

        #region ReadSubTrees_C45_ShouldGetCorrectS4SubTree
        [TestMethod]
        public void ReadSubTrees_C45_ShouldGetCorrectS4SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("Bid <= 1.3661:Hold (13.0/1.0)");
            builder.AppendLine("Bid > 1.3661:Buy (4.0/1.0)");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S4]"]);
        }
        #endregion

        #region ReadSubTrees_C50_ShouldGetCorrectS4SubTree
        [TestMethod]
        public void ReadSubTrees_C50_ShouldGetCorrectS4SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("SpreadMovingAverage > -2.9543e-05:Buy (14/1)");
            builder.AppendLine("SpreadMovingAverage <= -2.9543e-05:");
            builder.AppendLine("    Ask > 1.36749:Hold (4)");
            builder.AppendLine("    Ask <= 1.36749:[S5]");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S4]"]);
        }
        #endregion

        #region ReadSubTrees_C50_ShouldGetCorrectS5SubTree
        [TestMethod]
        public void ReadSubTrees_C50_ShouldGetCorrectS5SubTree()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);

            var builder = new StringBuilder();
            builder.AppendLine("BidMovingAverage <= 1.367539:Sell (13/1)");
            builder.AppendLine("BidMovingAverage > 1.367539:");
            builder.AppendLine("    Ask <= 1.36745:Sell (2)");
            builder.AppendLine("    Ask > 1.36745:Hold (7/3)");
            var subTreeExpected = builder.ToString();

            Assert.AreEqual(subTreeExpected, _reader.SubTrees["[S5]"]);
        }
        #endregion

        #endregion

        #region NormalizeTree Tests

        #region NormalizeTree_C45TreeWithSubTreesProvided_ShouldBeTheSameAsNormalizedTreeSource
        [TestMethod]
        public void NormalizeTree_C45TreeWithSubTreesProvided_ShouldBeTheSameAsNormalizedTreeSource()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c45WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);
            var normalizedDecisionTree = _reader.NormalizeTree(normalizedTree);

            Assert.AreEqual(normalizedDecisionTree, _c45NormalizedNoSubTrees);
        }
        #endregion

        #region NormalizeTree_C50TreeWithSubTreesProvided_ShouldBeTheSameAsNormalizedTreeSource
        [TestMethod]
        public void NormalizeTree_C50TreeWithSubTreesProvided_ShouldBeTheSameAsNormalizedTreeSource()
        {
            var normalizedTree = _reader.NormalizeTreeSource(_c50WithSubTrees);
            _reader.ReadSubTrees(normalizedTree);
            var normalizedDecisionTree = _reader.NormalizeTree(normalizedTree);

            Assert.AreEqual(normalizedDecisionTree, _c50NormalizedNoSubTrees);
        }
        #endregion

        #endregion

        #endregion


    }
}
