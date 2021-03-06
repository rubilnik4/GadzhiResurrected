﻿using GadzhiTest.DefaultData.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting;

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
            var filesInfoProject = new PackageData();
            var file = DefaultFileData.FileDataToTestTwoPositions[0];

            // Act  
            filesInfoProject.AddFile(file);

            // Assert 
            Assert.AreEqual(filesInfoProject.FilesData.Count, 1);
            Assert.AreSame(filesInfoProject.FilesData[filesInfoProject.FilesData.Count - 1], file);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при добавлении пустого класса. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFile_AddNull_NoException()
        {
            // Arrange     
            var filesInfoProject = new PackageData();
            FileData file = null;

            // Act
            filesInfoProject.AddFile(file);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при добавлении двух позиций в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddTwoPositions()
        {
            // Arrange     
            var filesInfoProject = new PackageData();
            var files = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);

            // Act  
            filesInfoProject.AddFiles(files);

            // Assert 
            Assert.AreEqual(filesInfoProject.FilesData.Count, files.Count);
            Assert.AreSame(filesInfoProject.FilesData[filesInfoProject.FilesData.Count - 1], files.Last());

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при добавлении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddNull_NoException()
        {
            // Arrange     
            var filesInfoProject = new PackageData();
            List<FileData> files = null;

            // Act
            filesInfoProject.AddFiles(files);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при добавлении списка с пустыми полями
        /// </summary>
        [TestMethod]
        public void AddFiles_ByIEnumerableFileData_AddListWithNull_NoException()
        {
            // Arrange     
            var filesInfoProject = new PackageData();
            List<FileData> files = new List<FileData>() { null };

            // Act           
            filesInfoProject.AddFiles(files);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при добавлении двух позиций в модель конвертации данных
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_AddTwoPositions()
        {
            // Arrange     
            var filesInfoProject = new PackageData();
            var files = new List<string>(DefaultFileData.FileDataToTestOnlyPath);

            // Act  
            filesInfoProject.AddFiles(files);

            // Assert           
            Assert.AreEqual(filesInfoProject.FilesData.Count, files.Count);

            var fileLast = filesInfoProject.FilesData[filesInfoProject.FilesData.Count - 1];
            Assert.AreEqual(fileLast.FileExtension, "Dgn");
            Assert.AreEqual(fileLast.FileName, "secondName");
            Assert.AreEqual(fileLast.FilePath, "C:\\folder\\secondName.Dgn");

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при добавлении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_AddNull_NoException()
        {
            // Arrange     
            var filesInfoProject = new PackageData();
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
            finally
            {
                filesInfoProject?.Dispose();
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
            var filesInfoProject = new PackageData();
            List<string> files = new List<string>() { null };

            // Act           
            filesInfoProject.AddFiles(files);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при добавлении существующего файла. ДОбавить 2 из 4. Итого 4
        /// </summary>
        [TestMethod]
        public void AddFiles_ByString_Existing_AddsTwoOfFour_TotalFour()
        {
            // Arrange     
            var files = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);
            var filesInfoProject = new PackageData(files);
            var filesToAdd = DefaultFileData.FileDataToTestFourPositions;

            // Act  
            filesInfoProject.AddFiles(filesToAdd);

            // Assert           
            Assert.AreEqual(filesInfoProject.FilesData.Count, 4);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка очистки списка. Пустой список
        /// </summary>
        [TestMethod]
        public void ClearFiles_CountNullResult()
        {
            // Arrange 
            var files = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);
            var filesInfoProject = new PackageData(files);

            // Act           
            filesInfoProject.ClearFiles();

            // Assert    
            Assert.AreEqual(filesInfoProject.FilesData.Count, 0);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Удаление файлов. Остается только третий (последний файл)
        /// </summary>
        [TestMethod]
        public void RemoveFiles_ByIEnumerableFileData_OnlyLastResult()
        {
            // Arrange 
            var files = new List<FileData>(DefaultFileData.FileDataToTestThreePositions);
            var filesInfoProject = new PackageData(files);
            var filesToRemove = new List<FileData>(DefaultFileData.FileDataToTestTwoPositions);
            FileData lastExpectedFile = DefaultFileData.FileDataToTestThreePositions[DefaultFileData.FileDataToTestThreePositions.Count - 1];

            // Act
            filesInfoProject.RemoveFiles(filesToRemove);

            // Assert    
            Assert.AreEqual(filesInfoProject.FilesData.Count, 1);
            Assert.AreEqual(filesInfoProject.FilesData[DefaultFileData.FileDataToTestThreePositions.Count - 1], lastExpectedFile);

            filesInfoProject?.Dispose();
        }

        /// <summary>
        /// Проверка при удалении пустого списка. Отсутствие ошибки
        /// </summary>
        [TestMethod]
        public void RemoveFiles_ByIEnumerableFileData_NoException()
        {
            // Arrange     
            List<FileData> files = null;
            var filesInfoProject = new PackageData(files);

            // Act
            filesInfoProject.RemoveFiles(files);

            filesInfoProject?.Dispose();
        }
    }
}
