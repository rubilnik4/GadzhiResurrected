using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Implementations.FilesConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Implementations.Converting;

namespace GadzhiConverting.Infrastructure.Implementations
{
    public class ConvertingFileData : IConvertingFileData
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Обработка и конвертирование файла
        /// </summary>
        private readonly IApplicationConverting _applicationConverting;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConvertingFileData(IMessagingService messagingService, IApplicationConverting applicationConverting,
                                  IFileSystemOperations fileSystemOperations)
        {
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
            _applicationConverting = applicationConverting ?? throw new ArgumentNullException(nameof(applicationConverting));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Конвертировать файл
        /// </summary>
        public IFileDataServer Converting(IFileDataServer fileDataServer, IConvertingSettings convertingSettings) =>
            fileDataServer.
            Void(fileData => _messagingService.ShowMessage($"Конвертация файла {fileDataServer.FileNameClient}")).
            Void(fileData => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), fileDataServer.FileNameServer)).
            Map(fileData => ConvertingFile(fileData, convertingSettings));

        /// <summary>
        /// Закрыть приложения
        /// </summary>
        public void CloseApplication() => _applicationConverting.CloseApplications();
     
        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        private IFileDataServer ConvertingFile(IFileDataServer fileDataServer, IConvertingSettings convertingSettings) =>
            LoadAndSaveDocument(fileDataServer).
            ResultValueOkBind(document =>
                GetSavedFileDataSource(document, fileDataServer).
                ToResultCollection().
                Map(saveResult => MakeConvertingFileActions(saveResult, document, fileDataServer, convertingSettings)).
                Map(filesData => CloseFile(document, fileDataServer).
                                 Map(closeResult => filesData.ConcatErrors(closeResult.Errors)).
                                 ToResultCollection())).
            Map(result => new FileDataServer(fileDataServer, StatusProcessing.ConvertingComplete, result.Value, result.Errors));

        /// <summary>
        /// Печать и экспорт файла
        /// </summary>
        private IResultCollection<IFileDataSourceServer> MakeConvertingFileActions(IResultCollection<IFileDataSourceServer> fileDataSourceServer,
                                                                                   IDocumentLibrary documentLibrary, IFileDataServer fileDataServer,
                                                                                   IConvertingSettings convertingSettings) =>
            fileDataSourceServer.
            ResultValueOkRaw(saveResult => CreatePdfToSaveResult(saveResult, documentLibrary, fileDataServer, convertingSettings).
                                           Map(saveResultPdf => ExportFileToSaveResult(saveResultPdf, documentLibrary, fileDataServer, convertingSettings)).
                                           Map(CheckDataSourceExistence));
       
        /// <summary>
        /// Загрузить файл и сохранить в папку для обработки
        /// </summary>   
        private IResultValue<IDocumentLibrary> LoadAndSaveDocument(IFilePath filePath) =>
            new ResultError().
            ResultVoidOk(_ => _messagingService.ShowMessage("Загрузка файла")).
            ResultVoidOk(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), filePath.FileNameServer)).
            ResultValueOkBind(_ => CreateSavingPathByExtension(filePath.FilePathServer, filePath.FileExtensionType)).
            ResultValueOkBind(savingPath => _fileSystemOperations.CopyFile(filePath.FilePathServer, savingPath).
                                            WhereContinue(copyResult => copyResult,
                                                okFunc: _ => new ResultValue<string>(savingPath),
                                                badFunc: _ => new ErrorCommon(ErrorConvertingType.FileNotSaved, $"Ошибка сохранения файла {savingPath}").
                                            ToResultValue<string>())).
            ResultValueOkBind(_ => _applicationConverting.OpenDocument(filePath.FilePathServer, filePath.FileNameServer)).
            Void(result => _messagingService.ShowAndLogErrors(result.Errors));

        /// <summary>
        /// Получить путь к сохраненному файлу для обработки
        /// </summary>        
        private static IResultValue<IFileDataSourceServer> GetSavedFileDataSource(IDocumentLibrary documentLibrary, IFilePath filePath) =>
            new FileDataSourceServer(documentLibrary.FullName, filePath.FilePathClient).
            Map(fileDataSource => new ResultValue<IFileDataSourceServer>(fileDataSource));

        /// <summary>
        /// Создать PDF и добавить в список обработанных файлов
        /// </summary>
        private IResultCollection<IFileDataSourceServer> CreatePdfToSaveResult(IResultCollection<IFileDataSourceServer> saveResult,
                                                                               IDocumentLibrary documentLibrary, IFileDataServer fileDataServer,
                                                                               IConvertingSettings convertingSettings) =>
            saveResult.ConcatResult(ConvertingModeChoice.IsPdfConvertingNeed(convertingSettings.ConvertingModeType) 
                                        ? CreatePdf(documentLibrary, fileDataServer, convertingSettings)
                                        : saveResult);

        /// <summary>
        /// Создать PDF
        /// </summary>
        private IResultCollection<IFileDataSourceServer> CreatePdf(IDocumentLibrary documentLibrary, IFileDataServer fileDataServer,
                                                                   IConvertingSettings convertingSettings) =>
            new ResultError().
            ResultVoidOk(_ => _messagingService.ShowMessage("Создание файлов PDF")).
            ResultVoidOk(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), fileDataServer.FileNameServer)).
            ResultValueOkBind(_ => CreateSavingPathByExtension(fileDataServer.FilePathServer, FileExtensionType.Pdf)).
            ResultValueOkBind(filePdfPath => _applicationConverting.CreatePdfFile(documentLibrary, fileDataServer.ChangeServerPath(filePdfPath),
                                                                                  convertingSettings, fileDataServer.ColorPrintType)).
            Void(result => _messagingService.ShowAndLogErrors(result.Errors)).
            ToResultCollection();

        /// <summary>
        /// Экспортировать в другие форматы и добавить в список обработанных файлов
        /// </summary>
        private IResultCollection<IFileDataSourceServer> ExportFileToSaveResult(IResultCollection<IFileDataSourceServer> saveResult,
                                                                                IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                                IConvertingSettings convertingSettings) =>
            documentLibrary.GetStampContainer(convertingSettings.ToApplication()).
            WhereContinue(stampContainer => ConvertingModeChoice.IsDwgConvertingNeed(convertingSettings.ConvertingModeType) &&
                                            (stampContainer.StampDocumentType == StampDocumentType.Specification ||
                                             stampContainer.StampDocumentType == StampDocumentType.Drawing),
                okFunc: stampContainer => saveResult.ConcatResultValue(ExportFile(documentLibrary, filePath, stampContainer.StampDocumentType)),
                badFunc: _ => saveResult);

        /// <summary>
        /// Экспортировать в другие форматы
        /// </summary>
        private IResultValue<IFileDataSourceServer> ExportFile(IDocumentLibrary documentLibrary, IFilePath filePath, StampDocumentType stampDocumentType) =>
            new ResultError().
            ResultVoidOk(_ => _messagingService.ShowMessage("Экспорт файла")).
            ResultVoidOk(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), filePath.FileNameServer)).
            ResultValueOkBind(_ => CreateSavingPathByExtension(filePath.FilePathServer,
                                                               _applicationConverting.GetExportFileExtension(filePath.FileExtensionType))).
            ResultValueOkBind(fileExportPath => _applicationConverting.CreateExportFile(documentLibrary, filePath.ChangeServerPath(fileExportPath), 
                                                                                        stampDocumentType)).
            Void(result => _messagingService.ShowErrors(result.Errors));

        /// <summary>
        /// Закрыть файл
        /// </summary>
        private IResultError CloseFile(IDocumentLibrary documentLibrary, IFilePath filePath) =>
            _applicationConverting.CloseDocument(documentLibrary, filePath.FilePathServer).
            Void(_ => _messagingService.ShowMessage($"Конвертация файла {filePath.FileNameClient} завершена")).
            Void(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), filePath.FileNameServer));

        /// <summary>
        /// Создать папку для сохранения отконвертированных файлов по типу расширения
        /// </summary>       
        private IResultValue<string> CreateSavingPathByExtension(string filePathServer, FileExtensionType fileExtensionType) =>
            new ResultValue<string>(filePathServer).
            ResultValueOk(Path.GetDirectoryName).
            ResultValueOk(directory => _fileSystemOperations.CreateFolderByName(Path.Combine(directory, Path.GetFileNameWithoutExtension(filePathServer)),
                                                                                fileExtensionType.ToString())).
            ResultValueOk(serverDirectory => FileSystemOperations.CombineFilePath(serverDirectory,
                                                                                  Path.GetFileNameWithoutExtension(filePathServer),
                                                                                  fileExtensionType.ToString().ToLowerCaseCurrentCulture()));

        /// <summary>
        /// Проверить наличие сохраненных файлов
        /// </summary>
        [Logger]
        private IResultCollection<IFileDataSourceServer> CheckDataSourceExistence(IResultCollection<IFileDataSourceServer> fileDataSourceResult) =>
            fileDataSourceResult.Value.
            Where(fileDataSource => !_fileSystemOperations.IsFileExist(fileDataSource.FilePathServer)).
            ToList().
            Map(fileDataSources => new
            {
                fileDataSources,
                errors = fileDataSources.Select(fileDataSource => new ErrorCommon(ErrorConvertingType.FileNotFound,
                                                                                  $"Файл {fileDataSource.FilePathServer} не найден"))
            }).
            Void(filesOrErrors => _messagingService.ShowAndLogErrors(filesOrErrors.errors)).
            Map(filesOrErrors => new ResultCollection<IFileDataSourceServer>(fileDataSourceResult.Value.Except(filesOrErrors.fileDataSources),
                                                                             fileDataSourceResult.Errors.Concat(filesOrErrors.errors)));

       
    }
}
