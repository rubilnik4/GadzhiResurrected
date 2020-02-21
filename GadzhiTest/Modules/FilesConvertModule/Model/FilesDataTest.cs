using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiTest.DefaultData.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiTest.Modules.FilesConvertModule.Model
{
    [TestClass]
    public class FilesDataTest
    {
        /// <summary>
        /// Проверка при добавлении позиции в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFile_AddOnePositions()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            var file = DefaultFileData.FileDataToTestTwoPositions.First();

            // Act  
            filesInfoProject.AddFile(file);

            // Assert 
            Assert.AreEqual(filesInfoProject.FilesInfo.Count, 1);
            Assert.AreSame(filesInfoProject.FilesInfo.Last(), file);
        }

        /// <summary>
        /// Проверка при добавлении пустого класса. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFile_AddNull_NoException()
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
            var files = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);

            // Act  
            filesInfoProject.AddFiles(files);

            // Assert 
            Assert.AreEqual(filesInfoProject.FilesInfo.Count, files.Count);
            Assert.AreSame(filesInfoProject.FilesInfo.Last(), files.Last());
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
            filesInfoProject.AddFiles(files);
        }

        /// <summary>
        /// Проверка при добавлении списка с пустыми полями
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddListWithNull_NoException()
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
            var files = new List<string>(DefaultFileData.FileDataToTestOnlyPath);

            // Act  
            filesInfoProject.AddFiles(files);

            // Assert           
            Assert.AreEqual(filesInfoProject.FilesInfo.Count, files.Count);

            FileData fileLast = filesInfoProject.FilesInfo.Last();
            Assert.AreEqual(fileLast.FileExtension, "dgn");
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

            try
            {
                // Act
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
        public void AddFiles_ByString_AddListWithNull_NoException()
        {
            // Arrange     
            FilesData filesInfoProject = new FilesData();
            List<string> files = new List<string>() { null };

            // Act           
            filesInfoProject.AddFiles(files);
        }

        /// <summary>
        /// Проверка при добавлении существующего файла. ДОбавить 2 из 4. Итого 4
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_Existing_AddsTwoOfFour_TotalFour()
        {
            // Arrange     
            var files = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);
            FilesData filesInfoProject = new FilesData(files);
            var filesToAdd = DefaultFileData.FileDataToTestFourPositions;

            // Act  
            filesInfoProject.AddFiles(filesToAdd);

            // Assert           
            Assert.AreEqual(filesInfoProject.FilesInfo.Count, 4);
        }

        /// <summary>
        /// Проверка очистки списка. Пустой список
        /// </summary>
        [TestMethod]
        public void ClearFiles_CountNullResult()
        {
            // Arrange 
            var files = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);
            FilesData filesInfoProject = new FilesData(files);

            // Act           
            filesInfoProject.ClearFiles();

            // Assert    
            Assert.AreEqual(filesInfoProject.FilesInfo.Count, 0);
        }

        /// <summary>
        /// Удаление файлов. Остается только третий (последний файл)
        /// </summary>
        [TestMethod]
        public void RemoveFiles_ByIEnumerableFileData_OnlyLastResult()
        {
            // Arrange 
            var files = new List<FileData>(DefaultFileData.FileDataToTestThreePositions);
            FilesData filesInfoProject = new FilesData(files);
            var filesToRemove = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);
            FileData LastExpectedFile = DefaultFileData.FileDataToTestThreePositions.Last();

            // Act
            filesInfoProject.RemoveFiles(filesToRemove);

            // Assert    
            Assert.AreEqual(filesInfoProject.FilesInfo.Count, 1);
            Assert.AreEqual(filesInfoProject.FilesInfo.Last(), LastExpectedFile);
        }

        /// <summary>
        /// Проверка при удалении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void RemoveFiles_ByIEnumerableFileData_NoException()
        {
            // Arrange     
            List<FileData> files = null;
            FilesData filesInfoProject = new FilesData(files);

            // Act
            filesInfoProject.RemoveFiles(files);
        }
    }
}
