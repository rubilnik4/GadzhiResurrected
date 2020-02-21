using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiTest.DefaultData.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GadzhiTest.Modules.FilesConvertModule.Model
{
    [TestClass]
    public class FileDataTest
    {
        /// <summary>
        /// Проверяем правильность заполенния конструктора
        /// </summary>
        [TestMethod]
        public void FileData_Initialize()
        {
            // Arrange     
            string filePath = "C:\\folder\\firstName.doc";
            var fileDataFirst = new FileData(filePath);

            // Assert   
            Assert.AreEqual(fileDataFirst.FileName, "firstName");
            Assert.AreEqual(fileDataFirst.FilePath, filePath);
            Assert.AreEqual(fileDataFirst.FileExtension, "doc");

        }

        /// <summary>
        /// Проверяем что объекты идентичны, но с разными ссылками
        /// </summary>
        [TestMethod]
        public void FileData_Equals_True()
        {
            // Arrange     
            string filePath = DefaultFileData.FileDataToTestOnlyPath.First();
            var fileDataFirst = new FileData(filePath);
            var fileDataSecond = new FileData(filePath);

            // Assert   
            Assert.IsTrue(fileDataFirst.Equals(fileDataSecond));
            Assert.IsFalse(fileDataFirst == fileDataSecond);
            Assert.AreNotSame(fileDataFirst, fileDataSecond);

        }
    }
}
