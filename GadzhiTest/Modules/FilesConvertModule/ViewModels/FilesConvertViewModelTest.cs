using System;
using System.Collections.Generic;
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
        /// Проверка обновления коллекции при додавлении двух файлов
        /// </summary>
        [TestMethod]
        public async Task FilesConvertViewModelTest_AddToPositions()
        {
            // Arrange  
            var mockApplicationGadzhi = new Mock<IApplicationGadzhi>();
            var mockFileInfoProject = new Mock<IFilesData>();

            var filesOutFromApplicationAdd = DefaultFileData.FileDataToTestOnlyPath;
            mockApplicationGadzhi.Setup(app => app.AddFromFilesOrDirectories(It.IsAny<IEnumerable<string>>())).
                                  Callback(() => mockFileInfoProject.Object.AddFiles(filesOutFromApplicationAdd));

            var fileDataChange = new Subject<FileChange>();
            mockFileInfoProject.SetupGet(fileProject => fileProject.FileDataChange)
                               .Returns(fileDataChange);

            IEnumerable<FileData> filesDataFromAddfileProject = new List<FileData>();
            mockFileInfoProject.Setup(fileProject => fileProject.AddFiles(It.IsAny<IEnumerable<string>>())).
                                Callback<IEnumerable<string>>(filesPath =>
                                {
                                    var filesData = filesPath.Select(file => new FileData(file));

                                    mockFileInfoProject.Object.FileDataChange.OnNext(
                                        new FileChange(filesData, ActionType.Add));

                                    filesDataFromAddfileProject = filesData;
                                });

            mockFileInfoProject.SetupGet(fileProject => fileProject.Files).Returns(
                filesOutFromApplicationAdd.Select(file => new FileData(file)).ToList());

            mockApplicationGadzhi.SetupGet(app => app.FilesInfoProject).Returns(mockFileInfoProject.Object);

            var filesConvertViewModel = new FilesConvertViewModel(mockApplicationGadzhi.Object);

            var files = new List<string>(DefaultFileData.FileDataToTestOnlyPath);
            var fileLastExpected = files.Last();

            // Act  
            await mockApplicationGadzhi.Object.AddFromFilesOrDirectories(files);

            // Assert 

            //проверка отправляемых данных
            Assert.AreEqual(filesDataFromAddfileProject.Count(), 2);
            Assert.AreEqual(filesDataFromAddfileProject.Last().FilePath, fileLastExpected);

            //проверка пришедших данных
            Assert.AreEqual(filesConvertViewModel.FilesDataCollection.Count, 2);
            Assert.AreEqual(filesConvertViewModel.FilesDataCollection.Last().FilePath, fileLastExpected);
        }
    }
}
