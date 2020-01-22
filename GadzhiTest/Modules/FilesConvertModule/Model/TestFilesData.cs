using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiModules.Modules.FilesConvertModule.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GadzhiTest.Modules.FilesConvertModule.Model
{
    [TestClass]
    public class TestFilesData
    {
        /// <summary>
        /// Проверка при добавлении позиции в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFile_AddOnePositions()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            var file = new FileData("doc", "firstName", "C:\\folder\\firstName.doc");

            // Act  
            filesInfoProject.AddFile(file);

            // Assert 
            Assert.AreEqual(filesInfoProject.Files.Count, 1);
            Assert.AreSame(filesInfoProject.Files.Last(), file);
        }

        /// <summary>
        /// Проверка при добавлении пустого класса. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFile_AddNull_Exception()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            FileData file = null;

            // Act
            filesInfoProject.AddFile(file);
        }

        /// <summary>
        /// Проверка при добавлении двух позиций в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddTwoPositions()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            var files = new List<FileData>()
            {
                 new FileData ("doc", "firstName", "C:\\folder\\firstName.doc"),
                 new FileData ("dgn", "secondName", "C:\\folder\\secondName.dgn"),
            };

            // Act  
            filesInfoProject.AddFiles(files);

            // Assert 
            Assert.AreEqual(filesInfoProject.Files.Count, files.Count);
            Assert.AreSame(filesInfoProject.Files.Last(), files.Last());
        }

        /// <summary>
        /// Проверка при добавлении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddNull_NoException()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            List<FileData> files = null;

            // Act
            try
            {
                filesInfoProject.AddFiles(files);
            }
            catch (Exception ex)
            {
                // Assert 
                Assert.Fail("Ошибку необходимо игнорировать: " + ex.Message);
            }
        }

        /// <summary>
        /// Проверка при добавлении списка с пустыми полями. Ошибка
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFiles_ByIEnumerableFileData_AddListWithNull_Exception()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            List<FileData> files = new List<FileData>() { null };

            // Act           
            filesInfoProject.AddFiles(files);

        }

        /// <summary>
        /// Проверка при добавлении двух позиций в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_AddTwoPositions()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            var files = new List<string>()
            {
                "C:\\folder\\firstName.doc",
                "C:\\folder\\secondName.dgn",
            };

            // Act  
            filesInfoProject.AddFiles(files);

            // Assert           
            Assert.AreEqual(filesInfoProject.Files.Count, files.Count);

            FileData fileLast = filesInfoProject.Files.Last();
            Assert.AreEqual(fileLast.FileType, "dgn");
            Assert.AreEqual(fileLast.FileName, "secondName");
            Assert.AreEqual(fileLast.FilePath, "C:\\folder\\secondName.dgn");
        }

        /// <summary>
        /// Проверка при добавлении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_AddNull_NoException()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            List<string> files = null;

            // Act
            try
            {
                filesInfoProject.AddFiles(files);
            }
            catch (Exception ex)
            {
                // Assert 
                Assert.Fail("Ошибку необходимо игнорировать: " + ex.Message);
            }
        }

        /// <summary>
        /// Проверка при добавлении списка с пустыми полями. Ошибка
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFiles_ByString_AddListWithNull_Exception()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            List<string> files = new List<string>() { null };

            // Act           
            filesInfoProject.AddFiles(files);

        }

        /// <summary>
        /// Очистка данных
        /// </summary>
        [TestMethod]
        public void ClearFiles()
        {
            // Arrange     
            Mock<FilesData> FilesInfoProject = new FilesData();
            

            // Act  
            FilesInfoProject.AddFiles(files);

            // Assert           
            Assert.AreEqual(FilesInfoProject.Files.Count, files.Count);

            FileData fileLast = FilesInfoProject.Files.Last();
            Assert.AreEqual(fileLast.FileType, "dgn");
            Assert.AreEqual(fileLast.FileName, "secondName");
            Assert.AreEqual(fileLast.FilePath, "C:\\folder\\secondName.dgn");
        }
    }
}
