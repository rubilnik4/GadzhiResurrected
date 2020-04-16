using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiApplicationCommon.Functional;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Implementations.FilesConvert;
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
        /// Конвертировать файл
        /// </summary>
        public async Task<IFileDataServer> Converting(IFileDataServer fileDataServer) =>
            await fileDataServer?.
            Void(fileData => _messagingService.ShowAndLogMessage($"Конвертация файла {fileDataServer.FileNameClient}")).
            WhereContinueAsync(fileData => fileData.IsValidByAttemptingCount,
                okFunc: fileData => Task.Run(() => ConvertingFile(fileData, _projectSettings.PrintersInformation)),
                badFunc: fileData => Task.FromResult(GetErrorByAttemptingCount(fileDataServer)));

        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        private IFileDataServer ConvertingFile(IFileDataServer fileDataServer, IPrintersInformation printersInformation) =>
            LoadDocument(fileDataServer).
            ResultValueOkBind(_ => CreatePdf(fileDataServer, printersInformation)).
            ResultValueOkBind(_ => CloseFile(fileDataServer.FileNameClient).ToResultValue<IEnumerable<IFileDataSourceServer>>()).
            ToResultCollection().
            Map(result => new FileDataServer(fileDataServer, StatusProcessing.ConvertingComplete, result.Value,
                                             result.Errors.Select(error => error.FileConvertErrorType)));

        /// <summary>
        /// Присвоить ошибку по количеству попыток конвертирования
        /// </summary>      
        private IFileDataServer GetErrorByAttemptingCount(IFileDataServer fileDataServer) =>
            new ErrorCommon(FileConvertErrorType.AttemptingCount, "Превышено количество попыток конвертирования файла").
            Void(errorDataServer => _messagingService.ShowAndLogError(errorDataServer)).
            Map(errorDataServer => new FileDataServer(fileDataServer, StatusProcessing.ConvertingComplete,
                                                      errorDataServer.Select(error => error.FileConvertErrorType)));

        /// <summary>
        /// Загрузить файл
        /// </summary>   
        private IResultValue<IFileDataSourceServer> LoadDocument(IFileDataServer fileDataServer) =>
            new ResultError().
            ResultVoidOk(_ => _messagingService.ShowAndLogMessage("Загрузка файла")).
            ResultValueOkBind(_ => _applicationConverting.OpenDocument(fileDataServer?.FilePathServer)).
            ResultValueOkBind(_ => CreateSavingPathByExtension(fileDataServer.FilePathServer, fileDataServer.FileExtentionType)).
            ResultValueOkBind(savingPath => _applicationConverting.SaveDocument(savingPath)).
            Void(result => _messagingService.ShowAndLogErrors(result.Errors));


        private IResultCollection<IFileDataSourceServer> CreatePdf(IFileDataServer fileDataServer, IPrintersInformation printersInformation) =>
            new ResultError().
            ResultVoidOk(_ => _messagingService.ShowAndLogMessage("Создание файлов PDF")).
            ResultValueOkBind(_ => CreateSavingPathByExtension(fileDataServer.FilePathServer, FileExtention.pdf)).
            ResultValueOkBind(filePath => _applicationConverting.CreatePdfFile(filePath, fileDataServer.ColorPrint,
                                                                               printersInformation?.PrintersPdf.FirstOrDefault())).
            Void(result => _messagingService.ShowAndLogErrors(result.Errors)).
            ToResultCollection();

        /// <summary>
        /// Экспортировать в другие форматы
        /// </summary>
        private (IEnumerable<IFileDataSourceServer>, IEnumerable<IErrorCommon>) ExportFile(IFileDataServer fileDataServer)
        {
            //    _loggerMicrostation.ShowMessage("Создание файла DWG");
            //    _applicationMicrostation.CreateDwgFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
            //  FileExtentionMicrostation.dwg));            
            return (null, null);
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        private IResultError CloseFile(string fileNameClient) =>
            _applicationConverting.CloseDocument().
            Void(_ => _messagingService.ShowAndLogMessage($"Конвертация файла {fileNameClient} завершена"));

        /// <summary>
        /// Создать папку для сохранения отконвертированных файлов по типу расширения
        /// </summary>       
        private IResultValue<string> CreateSavingPathByExtension(string filePathServer, FileExtention fileExtention) =>
            new ResultValue<string>(filePathServer).
            ResultValueOk(filePath => Path.GetDirectoryName(filePath)).
            ResultValueOk(directory => _fileSystemOperations.CreateFolderByName(directory, fileExtention.ToString())).
            ResultValueOk(serverDirectory => FileSystemOperations.CombineFilePath(serverDirectory,
                                                                                  Path.GetFileNameWithoutExtension(filePathServer),
                                                                                  fileExtention.ToString()));
    }
}
