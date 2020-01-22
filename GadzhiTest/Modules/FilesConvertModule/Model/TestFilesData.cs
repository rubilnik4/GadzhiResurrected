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
        /// Проверка при добавлении двух позиций в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddTwoPositions()
        {
            // Arrange     
            FilesData FilesInfoProject = new FilesData();
            var files = new List<FileData>()
            {
                 new FileData ("firstType", "firstName", "firstPath"),
                 new FileData ("secondType", "secondName", "secondPath"),
            };

            // Act  
            FilesInfoProject.AddFiles(files);

            // Assert 
            Assert.AreEqual(FilesInfoProject.Files.Count, files.Count);
            Assert.AreSame(FilesInfoProject.Files.Last(), files.Last());
        }

        /// <summary>
        /// Проверка при добавлении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddNull_NoException()
        {
            // Arrange     
            FilesData FilesInfoProject = new FilesData();
            List<FileData> files = null;

            // Act
            try
            {
                FilesInfoProject.AddFiles(files);
            }
            catch (Exception ex)
            {
                // Assert 
                Assert.Fail("Ошибка добавления данных в коллекцию: " + ex.Message);
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
            FilesData FilesInfoProject = new FilesData();
            List<FileData> files = new List<FileData>() { null };

            // Act           
            FilesInfoProject.AddFiles(files);

        }

        /// <summary>
        /// Проверка при добавлении двух позиций в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_AddTwoPositions()
        {
            // Arrange     
            FilesData FilesInfoProject = new FilesData();
            var files = new List<string>()
            {
                "firstPath",
                "secondPath",
            };

            // Act  
            FilesInfoProject.AddFiles(files);

            // Assert 
            Assert.AreEqual(FilesInfoProject.Files.Count, files.Count);
            Assert.AreSame(FilesInfoProject.Files.Last(), files.Last());
        }

        /// <summary>
        /// Проверка при добавлении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_AddNull_NoException()
        {
            // Arrange     
            FilesData FilesInfoProject = new FilesData();
            List<string> files = null;

            // Act
            try
            {
                FilesInfoProject.AddFiles(files);
            }
            catch (Exception ex)
            {
                // Assert 
                Assert.Fail("Ошибка добавления данных в коллекцию: " + ex.Message);
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
            FilesData FilesInfoProject = new FilesData();
            List<FileData> files = new List<FileData>() { null };

            // Act           
            FilesInfoProject.AddFiles(files);

        }
    }
}
