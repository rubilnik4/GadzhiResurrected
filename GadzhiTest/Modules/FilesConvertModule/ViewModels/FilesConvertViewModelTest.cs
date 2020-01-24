using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations.ReactiveSubjects;
using GadzhiModules.Modules.FilesConvertModule.ViewModels;
using GadzhiTest.DefaultData.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GadzhiTest.Modules.FilesConvertModule.ViewModels
{
    [TestClass]
    public class FilesConvertViewModelTest
    {
        /// <summary>
        /// Инициализация инфраструктуры и модели данных для добавления
        /// </summary>       
        private Mock<IApplicationGadzhi> ApplicationGadzhiTestAddInitialize()
        {
            var mockApplicationGadzhi = new Mock<IApplicationGadzhi>();
            var mockFileInfoProject = new Mock<IFilesData>();

            var filesOutFromApplicationAdd = DefaultFileData.FileDataToTestOnlyPath;
            mockApplicationGadzhi.Setup(app => app.AddFromFilesOrDirectories(It.IsAny<IEnumerable<string>>())).
                                  Callback(() => mockFileInfoProject.Object.AddFiles(filesOutFromApplicationAdd));
            mockApplicationGadzhi.SetupGet(app => app.FilesInfoProject).Returns(mockFileInfoProject.Object);

            mockFileInfoProject.Setup(fileProject => fileProject.AddFiles(It.IsAny<IEnumerable<string>>())).
                               Callback<IEnumerable<string>>(filesPathToAdd =>
                               {
                                   var filesData = filesPathToAdd.Select(file => new FileData(file));
                                   var filesDataProject = new List<FileData>(filesData); //добавляем данные в модель

                                   mockFileInfoProject.Object.FileDataChange.OnNext(
                                       new FileChange(filesData, filesData, ActionType.Add));
                               });
            var fileDataChange = new Subject<FileChange>();
            mockFileInfoProject.SetupGet(fileProject => fileProject.FileDataChange)
                               .Returns(fileDataChange);

            return mockApplicationGadzhi;
        }

        /// <summary>
        /// Инициализация инфраструктуры и модели данных для удаления
        /// </summary>       
        private Mock<IApplicationGadzhi> ApplicationGadzhiTestRemoveInitialize(IEnumerable<FileData> defaultFileData)
        {
            var mockApplicationGadzhi = new Mock<IApplicationGadzhi>();
            var mockFileInfoProject = new Mock<IFilesData>();

            var filesOutFromApplicationAdd = defaultFileData;
            mockApplicationGadzhi.Setup(app => app.RemoveFiles(It.IsAny<IEnumerable<FileData>>())).
                                  Callback<IEnumerable<FileData>>(filesDataToRemove =>
                                        mockFileInfoProject.Object.RemoveFiles(filesDataToRemove));
            mockApplicationGadzhi.SetupGet(app => app.FilesInfoProject).Returns(mockFileInfoProject.Object);

            mockFileInfoProject.Setup(fileProject => fileProject.RemoveFiles(It.IsAny<IEnumerable<FileData>>())).
                               Callback<IEnumerable<FileData>>(filesDataToRemove =>
                               {
                                   var filesDataProject = new List<FileData>(defaultFileData); //инициализируем модель
                                   filesDataProject.RemoveAll(fileData => filesDataToRemove.Contains(fileData)); //удаляем из модели

                                   mockFileInfoProject.Object.FileDataChange.OnNext(
                                       new FileChange(filesDataProject, filesDataToRemove, ActionType.Remove));
                               });
            var fileDataChange = new Subject<FileChange>();
            mockFileInfoProject.SetupGet(fileProject => fileProject.FileDataChange)
                               .Returns(fileDataChange);

            return mockApplicationGadzhi;
        }

        /// <summary>
        /// Проверка обновления коллекции при добавлении двух файлов
        /// </summary>
        [TestMethod]
        public async Task FilesConvertViewModelTest_AddToPositions()
        {

            Mock<IApplicationGadzhi> mockApplicationGadzhi = ApplicationGadzhiTestAddInitialize();

            // Arrange
            var filesConvertViewModel = new FilesConvertViewModel(mockApplicationGadzhi.Object);

            var filesInput = new List<string>(DefaultFileData.FileDataToTestOnlyPath);
            var fileLastExpected = filesInput.Last();

            // Act  
            await mockApplicationGadzhi.Object.AddFromFilesOrDirectories(filesInput);

            // Assert 
            Assert.AreEqual(filesConvertViewModel.FilesDataCollection.Count, 2);
            Assert.AreEqual(filesConvertViewModel.FilesDataCollection.Last().FilePath, fileLastExpected);
        }

        /// <summary>
        /// Проверка обновления коллекции при удалении двух файлов в коллекции из четырех
        /// </summary>
        [TestMethod]
        public void FilesConvertViewModelTest_RemoveToPositions_RemainTwo()
        {
            // Arrange
            IEnumerable<FileData> defaultFileData = DefaultFileData.FileDataToTestFourPositions;
            Mock<IApplicationGadzhi> mockApplicationGadzhi = ApplicationGadzhiTestRemoveInitialize(defaultFileData);

            var filesConvertViewModel = new FilesConvertViewModel(mockApplicationGadzhi.Object);                     
            filesConvertViewModel.FilesDataCollection.AddRange(defaultFileData); //заполняем ViewModel теми же данными, что и модель   

            var filesInput = new List<FileData>(DefaultFileData.FileDataToTestFourPositions.
                                                                Take(2).
                                                                ToList());
            var fileLastExpected = defaultFileData.Last();

            // Act  
            mockApplicationGadzhi.Object.RemoveFiles(filesInput);

            // Assert 
            Assert.AreEqual(filesConvertViewModel.FilesDataCollection.Count, 2);
            Assert.AreEqual(filesConvertViewModel.FilesDataCollection.Last(), fileLastExpected);
        }

        /// <summary>    
        /// Проверка включения кнопки после операции
        /// </summary>
        [TestMethod]
        public void FilesConvertViewModelTest_DisableButtons()
        {
            // Arrange
            var mockApplicationGadzhi = new Mock<IApplicationGadzhi>();
            var mockFileInfoProject = new Mock<IFilesData>();

            mockApplicationGadzhi.Setup(app => app.AddFromFolders());
            mockApplicationGadzhi.SetupGet(app => app.FilesInfoProject).
                                  Returns(mockFileInfoProject.Object);

            mockFileInfoProject.SetupGet(fileProject => fileProject.FileDataChange).
                                Returns(new Subject<FileChange>());

            var filesConvertViewModel = new FilesConvertViewModel(mockApplicationGadzhi.Object);

            // Act  
            filesConvertViewModel.AddFromFoldersDelegateCommand.Execute();

            // Assert
            Assert.IsTrue(filesConvertViewModel.AddFromFoldersDelegateCommand.CanExecute());
        }
    }
}
