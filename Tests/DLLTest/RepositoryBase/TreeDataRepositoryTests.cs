#region Usings
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bridge.IDLL.Exceptions;
using Implementation.DLL.RepositoryBase;
using Tests.DLLTest.Models;
#endregion

namespace Tests.DLLTest.RepositoryBase
{
    [TestClass]
    public class TreeDataRepositoryTests
    {

        #region Private Fields
        private TreeDataRepository<FakeDataModel> _repository;
        private List<FakeDataModel> _records;
        private string _data;

        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new TreeDataRepository<FakeDataModel>();

            var builder = new StringBuilder();
            builder.AppendLine("testName1,testValue1,0.123456789012346,Buy");
            builder.AppendLine("testName2,testValue2,1.12345678901235,Hold");
            builder.AppendLine("testName3,testValue3,2.12345678901235,Sell");

            _data = builder.ToString();

            _records = new List<FakeDataModel>
            {
                new FakeDataModel
                {
                    Name = "testName1",
                    Value = "testValue1",
                    Price = 0.1234567890123456,
                    Option = "Buy"
                },
                new FakeDataModel
                {
                    Name = "testName2",
                    Value = "testValue2",
                    Price = 1.1234567890123456,
                    Option = "Hold"
                },
                new FakeDataModel
                {
                    Name = "testName3",
                    Value = "testValue3",
                    Price = 2.1234567890123456,
                    Option = "Sell"
                }
            };
        }
        #endregion

        #region TestSuite

        #region SaveData Tests

        #region SaveData_CollectionNameIsNull_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Collection name is not provided.")]
        public void SaveData_CollectionNameIsNull_ShouldThrowDalException()
        {
            _repository.CollectionName = null;

            _repository.SaveData(null);

            Assert.Fail();
        }
        #endregion

        #region SaveData_CollectionNameIsEmpty_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Collection name is not provided.")]
        public void SaveData_CollectionNameIsEmpty_ShouldThrowDalException()
        {
            _repository.CollectionName = string.Empty;

            _repository.SaveData(null);

            Assert.Fail();
        }
        #endregion

        #region SaveData_CollectionNameHasOnlyWhiteSpaces_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Collection name is not provided.")]
        public void SaveData_CollectionNameHasOnlyWhiteSpaces_ShouldThrowDalException()
        {
            _repository.CollectionName = "   ";

            _repository.SaveData(null);

            Assert.Fail();
        }
        #endregion

        #region SaveData_PathIsNull_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Path is not provided.")]
        public void SaveData_PathIsNull_ShouldThrowDalException()
        {
            _repository.CollectionName = "test";
            _repository.Path = null;

            _repository.SaveData(null);

            Assert.Fail();
        }
        #endregion

        #region SaveData_PathIsEmpty_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Path is not provided.")]
        public void SaveData_PathIsEmpty_ShouldThrowDalException()
        {
            _repository.CollectionName = "test";
            _repository.Path = string.Empty;

            _repository.SaveData(null);

            Assert.Fail();
        }
        #endregion

        #region SaveData_PathHasWhiteSpacesOnly_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Path is not provided.")]
        public void SaveData_PathHasWhiteSpacesOnly_ShouldThrowDalException()
        {
            _repository.CollectionName = "test";
            _repository.Path = "  ";

            _repository.SaveData(null);

            Assert.Fail();
        }
        #endregion

        #region SaveData_PathDoesNotExist_ShouldThrowDalException
        [TestMethod]
        [ExpectedException(typeof(DalException), "Path testTest123 does not exist.")]
        public void SaveData_PathDoesNotExist_ShouldThrowDalException()
        {
            _repository.CollectionName = "test";
            _repository.Path = "testTest123";

            _repository.SaveData(null);

            Assert.Fail();
        }
        #endregion

        #region SaveData_ShouldSaveNamesFile
        [TestMethod]
        public void SaveData_ShouldSaveNamesFile()
        {
            _repository.CollectionName = "FakeData";
            _repository.Path = ConfigurationManager.AppSettings["TestDataDirectory"];
            _repository.NamesFileContents = "Test";

            _repository.SaveData(_records);
            var text = File.ReadAllText(_repository.Path + "/FakeData.names");

            Assert.AreEqual("Test", text);
        }
        #endregion

        #region SaveData_ShouldSaveRecords
        [TestMethod]
        public void SaveData_ShouldSaveRecords()
        {
            _repository.CollectionName = "FakeData";
            _repository.Path = ConfigurationManager.AppSettings["TestDataDirectory"];
            _repository.NamesFileContents = "Test";

            _repository.SaveData(_records);
            var text = File.ReadAllText(_repository.Path + "/FakeData.data");

            Assert.AreEqual(_data, text);
        }
        #endregion

        #endregion

        #endregion

    }
}
