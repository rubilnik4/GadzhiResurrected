using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Implementations;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using GadzhiTest.DefaultData.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace GadzhiTest
{
    [TestClass]
    public class ApplicationGadzhiTest
    {
        /// <summary>
        /// Диалоговые окна. Пустой класс
        /// </summary>
        Mock<IDialogServiceStandard> MockDialogServiceStandard;

        [TestInitialize]
        public void ApplicationGadzhiTestInitialize()
        {
            MockDialogServiceStandard = new Mock<IDialogServiceStandard>();
        }

        /// <summary>
        /// Инициализация класса поиска файлов
        /// <summary>
        private Mock<IFileSeach> mockFileSeachFill(IEnumerable<string> subDirectoryPath = null,
                                                IEnumerable<string> filesInSubDirectory = null,
                                                string subfolderGetFiles = null)
        {
            var mockFileSeach = new Mock<IFileSeach>();
            var fileSeachImplementation = new FileSeach();
            mockFileSeach.Setup(fileSeach => fileSeach.IsDirectoryExist(It.IsAny<string>()))
                         .Returns(true);
            mockFileSeach.Setup(fileSeach => fileSeach.IsFileExist(It.IsAny<string>()))
                         .Returns(true);
            mockFileSeach.Setup(fileSeach => fileSeach.IsDirectory(It.IsAny<string>()))
                         .Returns<string>(path => fileSeachImplementation.IsDirectory(path));
            mockFileSeach.Setup(fileSeach => fileSeach.IsFile(It.IsAny<string>()))
                         .Returns<string>(path => fileSeachImplementation.IsFile(path));
            mockFileSeach.Setup(fileSeach => fileSeach.GetDirectories(It.IsAny<string>()))
                         .Returns(subDirectoryPath);
            mockFileSeach.Setup(fileSeach => fileSeach.GetFiles(subfolderGetFiles))
                         .Returns(filesInSubDirectory);

            return mockFileSeach;
        }

        /// <summary>    
        /// Проверка при добавлении двух позиций в инфраструктуре
        /// </summary>
        [TestMethod]
        public async Task AddFromFilesOrDirectories_AddTwoPositions()
        {
            // Arrange
            Mock<IFileSeach> mockFileSeach = mockFileSeachFill();

            var mockFileInfoProject = new Mock<IFilesData>();
            IEnumerable<string> filesPathOut = new List<string>();
            mockFileInfoProject.Setup(fileInfo => fileInfo.AddFiles(It.IsAny<IEnumerable<string>>()))
                               .Callback<IEnumerable<string>>(filesPath => filesPathOut = filesPath);

            var applicationGadzhi = new ApplicationGadzhi(MockDialogServiceStandard.Object,
                                                          mockFileSeach.Object,
                                                          mockFileInfoProject.Object);

            var files = new List<string>(DefaultFileData.FileDataToTestOnlyPath);
            var fileLastExpected = DefaultFileData.FileDataToTestOnlyPath.Last();

            // Act  
            await applicationGadzhi.AddFromFilesOrDirectories(files);

            // Assert 
            mockFileInfoProject.Verify(fileInfoProject => fileInfoProject.AddFiles(It.IsAny<IEnumerable<string>>()),
                                       Times.Exactly(1),
                                       "Ошибка вызова метода AddFiles");
            Assert.AreEqual(filesPathOut.Count(), 2);
            Assert.AreEqual(filesPathOut.Last(), fileLastExpected);
        }

        /// <summary>    
        /// Проверка при добавлении двух позиций из файлов и двух позиций из поддиректории
        /// </summary>
        [TestMethod]
        public async Task AddFromFilesOrDirectories_AddTwoPositionsFromFiles_And_AddTwoPositionFromSubDirectory()
        {
            // Arrange 
            string subfolderGetFiles = "C:\\folder\\subfolderGetFiles";
            string subfolderNoFiles = "C:\\folder\\subfolderNoFiles";
            var subDirectoryPath = new List<string>()
            {
               subfolderGetFiles,
               subfolderNoFiles,
            };
            var filesInSubDirectory = new List<string>(DefaultFileData.FileDataToTestOnlyPath.
                Select(path => Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "Sub" + Path.GetExtension(path)));

            Mock<IFileSeach> mockFileSeach = mockFileSeachFill(subDirectoryPath, filesInSubDirectory, subfolderGetFiles);

            var mockFileInfoProject = new Mock<IFilesData>();
            IEnumerable<string> filesPathOut = new List<string>();
            mockFileInfoProject.Setup(fileInfo => fileInfo.AddFiles(It.IsAny<IEnumerable<string>>()))
                               .Callback<IEnumerable<string>>(filesPath => filesPathOut = filesPath);

            var applicationGadzhi = new ApplicationGadzhi(MockDialogServiceStandard.Object,
                                                          mockFileSeach.Object,
                                                          mockFileInfoProject.Object);

            var files = new List<string>(DefaultFileData.FileDataToTestOnlyPath);
            files.AddRange(subDirectoryPath); //добавляем к тесту директории

            var fileFirstExpected = files.First();
            var fileLastExpected = filesInSubDirectory.Last();

            // Act  
            await applicationGadzhi.AddFromFilesOrDirectories(files);

            // Assert 
            mockFileInfoProject.Verify(fileInfoProject => fileInfoProject.AddFiles(It.IsAny<IEnumerable<string>>()),
                                       Times.Exactly(1),
                                       "Ошибка вызова метода AddFiles");
            Assert.AreEqual(filesPathOut.Count(), 4);
            Assert.AreEqual(filesPathOut.First(), fileFirstExpected);
            Assert.AreEqual(filesPathOut.Last(), fileLastExpected);
        }

        /// <summary>    
        /// Проверка при добавлении пустого списка в инфраструктуре
        /// </summary>
        [TestMethod]
        public async Task AddFromFilesOrDirectories_AddNull_NoException()
        {
            // Arrange            
            Mock<IFileSeach> mockFileSeach = mockFileSeachFill();
            var mockFileInfoProject = new Mock<IFilesData>();

            var applicationGadzhi = new ApplicationGadzhi(MockDialogServiceStandard.Object,
                                                          mockFileSeach.Object,
                                                          mockFileInfoProject.Object);

            IEnumerable<string> files = null;
            var fileLastExpected = DefaultFileData.FileDataToTestOnlyPath.Last();

            try
            {
                // Act  
                await applicationGadzhi.AddFromFilesOrDirectories(files);

                // Assert 
                mockFileInfoProject.Verify(fileInfoProject => fileInfoProject.AddFiles(It.IsAny<IEnumerable<string>>()),
                                           Times.Exactly(0),
                                           "Ошибочно вызван метод AddFiles");
            }
            catch (Exception ex)
            {
                // Assert 
                Assert.Fail("Ошибку необходимо игнорировать: " + ex.Message);
            }
        }

        /// <summary>    
        /// Проверка при добавлении пустого списка в инфраструктуре
        /// </summary>
        [TestMethod]
        public async Task AddFromFilesOrDirectories_AddListWithNull_NoException()
        {
            // Arrange             
            Mock<IFileSeach> mockFileSeach = mockFileSeachFill();
            var mockFileInfoProject = new Mock<IFilesData>();

            var applicationGadzhi = new ApplicationGadzhi(MockDialogServiceStandard.Object,
                                                          mockFileSeach.Object,
                                                          mockFileInfoProject.Object);

            IEnumerable<string> files = new List<string> { null };
            var fileLastExpected = DefaultFileData.FileDataToTestOnlyPath.Last();

            try
            {
                // Act  
                await applicationGadzhi.AddFromFilesOrDirectories(files);

                // Assert 
                mockFileInfoProject.Verify(fileInfoProject => fileInfoProject.AddFiles(It.IsAny<IEnumerable<string>>()),
                                           Times.Exactly(0),
                                           "Ошибочно вызван метод AddFiles");
            }
            catch (Exception ex)
            {
                // Assert 
                Assert.Fail("Ошибку необходимо игнорировать: " + ex.Message);
            }
        }        
    }
}
