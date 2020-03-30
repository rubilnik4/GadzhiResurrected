using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    public class ConvertingFileData : IConvertingFileData
    {
        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Обработка и конвертирование файла DGN
        /// </summary>
        private readonly IApplicationConverting _applicationConverting;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConvertingFileData(IMessagingService messagingService, IApplicationConverting applicationConverting,
                                  IProjectSettings projectSettings, IFileSystemOperations fileSystemOperations)
        {
            _messagingService = messagingService;
            _applicationConverting = applicationConverting;
            _projectSettings = projectSettings;
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        public async Task<IFileDataServer> Converting(IFileDataServer fileDataServer)
        {
            if (fileDataServer != null)
            {
                fileDataServer = FileDataStartConverting(fileDataServer);

                await FileDataConverting(fileDataServer);

                fileDataServer = FileDataEndConverting(fileDataServer);

                return fileDataServer;
            }
            else
            {
                throw new ArgumentNullException(nameof(fileDataServer));
            }
        }

        /// <summary>
        /// Начать конвертирование файла
        /// </summary>
        private IFileDataServer FileDataStartConverting(IFileDataServer fileDataServer)
        {
            _messagingService.ShowAndLogMessage($"Конвертация файла {fileDataServer.FileNameClient}");

            fileDataServer.StatusProcessing = StatusProcessing.Converting;

            return fileDataServer;
        }

        /// <summary>
        /// Закончить конвертирование файла
        /// </summary>
        private IFileDataServer FileDataEndConverting(IFileDataServer fileDataServer)
        {
            fileDataServer.StatusProcessing = StatusProcessing.ConvertingComplete;
            return fileDataServer;
        }

        /// <summary>
        /// Конвертировать файл
        /// </summary>
        private async Task<IFileDataServer> FileDataConverting(IFileDataServer fileDataServer)
        {

            if (fileDataServer.IsValidByAttemptingCount)
            {
                await Task.Run(() => ConvertingFile(fileDataServer, _projectSettings.PrintersInformation));
            }
            else
            {
                _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.AttemptingCount,
                                                          "Превышено количество попыток конвертирования файла"));
                fileDataServer.AddFileConvertErrorType(FileConvertErrorType.AttemptingCount);
            }

            return fileDataServer;
        }

        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        public IFileDataServer ConvertingFile(IFileDataServer fileDataServer, IPrintersInformation printersInformation)
        {
            (var loadDataSources, var loadErrors) = LoadDocument(fileDataServer);
            (var pdfDataSources, var pdfErrors) = CreatePdf(fileDataServer, printersInformation);
            (var exportDataSources, var exportErrors) = ExportFile(fileDataServer);

            var errors = loadErrors.
                         UnionNotNull(pdfErrors).
                         UnionNotNull(exportErrors).
                         Select(error => error.FileConvertErrorType);
            fileDataServer.AddRangeFileConvertErrorType(errors);

            var dataSources = loadDataSources.
                              UnionNotNull(pdfDataSources).
                              UnionNotNull(exportDataSources);
            fileDataServer.SetFileDatasSourceServerConverting(dataSources);

            CloseFile();

            return fileDataServer;
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>   
        private (IEnumerable<IFileDataSourceServer>, IEnumerable<IErrorConverting>) LoadDocument(IFileDataServer fileDataServer)
        {
            _messagingService.ShowAndLogMessage("Загрузка файла");
            _applicationConverting.OpenDocument(fileDataServer?.FilePathServer);

            (IEnumerable<IFileDataSourceServer> savingSources, IEnumerable<IErrorConverting> savingErrors) =
                _applicationConverting.SaveDocument(CreateSavingPathByExtension(fileDataServer.FilePathServer,
                                                                                fileDataServer.FileExtentionType));
            _messagingService.ShowAndLogErrors(savingErrors);

            return (savingSources, savingErrors);
        }

        /// <summary>
        /// Создать Pdf
        /// </summary>       
        private (IEnumerable<IFileDataSourceServer>, IEnumerable<IErrorConverting>) CreatePdf(IFileDataServer fileDataServer,
                                                                                             IPrintersInformation printersInformation)
        {
            _messagingService.ShowAndLogMessage("Создание файлов PDF");
            (IEnumerable<IFileDataSourceServer> pdfSources, IEnumerable<IErrorConverting> pdfErrors) =
                _applicationConverting.CreatePdfFile(CreateSavingPathByExtension(fileDataServer.FilePathServer, FileExtention.pdf),
                                                                                 fileDataServer.ColorPrint,
                                                                                 printersInformation?.PrintersPdf.FirstOrDefault());
            _messagingService.ShowAndLogErrors(pdfErrors);

            return (pdfSources, pdfErrors);
        }

        /// <summary>
        /// Экспортировать в другие форматы
        /// </summary>
        private (IEnumerable<IFileDataSourceServer>, IEnumerable<IErrorConverting>) ExportFile(IFileDataServer fileDataServer)
        {
            //    _loggerMicrostation.ShowMessage("Создание файла DWG");
            //    _applicationMicrostation.CreateDwgFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
            //  FileExtentionMicrostation.dwg));            
            return (null, null);
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        private void CloseFile()
        {
            _messagingService.ShowAndLogMessage("Конвертирование завершено");
            _applicationConverting.CloseDocument();
        }

        /// <summary>
        /// Создать папку для сохранения отконвертированных файлов по типу расширения
        /// </summary>       
        private string CreateSavingPathByExtension(string filePathServer, FileExtention fileExtention)
        {
            if (!String.IsNullOrWhiteSpace(filePathServer))
            {
                string serverDirectory = _fileSystemOperations.CreateFolderByName(Path.GetDirectoryName(filePathServer),
                                                                                      fileExtention.ToString());
                return _fileSystemOperations.CombineFilePath(serverDirectory, Path.GetFileNameWithoutExtension(filePathServer),
                                                             fileExtention.ToString());
            }
            else
            {
                throw new ArgumentNullException(nameof(filePathServer));
            }
        }
    }
}
