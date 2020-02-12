using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces;
using GadzhiDAL.Services.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Инфраструктура для конвертирования файлов
    /// </summary>
    public class ApplicationConverting : IApplicationConverting
    {
        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Основная модель состояния процесса конвертирования
        /// </summary>
        private readonly IConvertingProject _convertingProject;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessageAndLoggingService _messageAndLoggingService;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>
        private readonly IFilesDataServiceServer _filesDataServiceServer;

        /// <summary>
        /// Конвертер из трансферной модели в серверную
        /// </summary>     
        private readonly IConverterServerFilesDataFromDTO _converterServerFilesDataFromDTO;

        /// <summary>
        /// Конвертер из серверной модели в трансферную
        /// </summary>
        private readonly IConverterServerFilesDataToDTO _converterServerFilesDataToDTO;

        public ApplicationConverting(IProjectSettings projectSettings,
                                     IFileSystemOperations fileSystemOperations,
                                     IConvertingProject convertingProject,
                                     IFilesDataServiceServer filesDataServiceServer,
                                     IMessageAndLoggingService messageAndLoggingService,
                                     IExecuteAndCatchErrors executeAndCatchErrors,
                                     IConverterServerFilesDataFromDTO converterServerFilesDataFromDTO,
                                     IConverterServerFilesDataToDTO converterServerFilesDataToDTO)
        {
            _projectSettings = projectSettings;
            _fileSystemOperations = fileSystemOperations;
            _convertingProject = convertingProject;
            _messageAndLoggingService = messageAndLoggingService;
            _executeAndCatchErrors = executeAndCatchErrors;
            _filesDataServiceServer = filesDataServiceServer;
            _converterServerFilesDataFromDTO = converterServerFilesDataFromDTO;
            _converterServerFilesDataToDTO = converterServerFilesDataToDTO;
        }

        /// <summary>
        /// Запуск процесса конвертирования
        /// </summary>
        private CompositeDisposable _convertingUpdaterSubsriptions;

        /// <summary>
        /// Запущен ли процесс конвертации
        /// </summary>
        private bool IsConverting { get; set; }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        public void StartConverting()
        {
            bool isValidStartUpdaParameters = ValidateStartupParameters();
            if (isValidStartUpdaParameters)
            {
                _messageAndLoggingService.ShowMessage("Запуск процесса конвертирования...");

                _convertingUpdaterSubsriptions = new CompositeDisposable
                {
                    Observable.Interval(TimeSpan.FromSeconds(_projectSettings.IntervalSecondsToServer)).
                               TakeWhile(_ => !IsConverting).
                               Subscribe(async _ =>
                                         await _executeAndCatchErrors.
                                         ExecuteAndHandleErrorAsync(ConvertingFirstInQueuePackage,
                                                                    () => IsConverting = false))

                };
            }
        }

        /// <summary>
        /// Проверить параметры запуска, добавить ошибки в модель
        /// </summary>
        private bool ValidateStartupParameters()
        {
            bool isDataBaseExist = _fileSystemOperations.IsFileExist(_projectSettings.SQLiteDataBasePath);
            if (!isDataBaseExist)
            {
                var errorTypeConverting = new ErrorTypeConverting(FileConvertErrorType.FileNotFound,
                                                                  $"Файл базы данных {_projectSettings.SQLiteDataBasePath} не найден");
                _messageAndLoggingService.ShowError(errorTypeConverting);
            }

            return isDataBaseExist;
        }

        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>        
        private async Task ConvertingFirstInQueuePackage()
        {
            FilesDataRequest filesDataRequest = await _filesDataServiceServer.GetFirstInQueuePackage();
            FilesDataServer filesDataServer = await _converterServerFilesDataFromDTO.ConvertToFilesDataServerAndSaveFile(filesDataRequest);

            if (filesDataServer.IsValid)
            {
                foreach (var filedata in filesDataServer.FilesDataInfo)
                {
                    filedata.StatusProcessing = StatusProcessing.Converting;

                    await Task.Delay(2000);

                    if (filedata.IsValid)
                    {
                        filedata.StatusProcessing = StatusProcessing.Completed;
                    }
                    else
                    {
                        filedata.StatusProcessing = StatusProcessing.Error;
                    }
                    filedata.IsCompleted = true;
                }
            }
            else
            {
                filesDataServer.IsCompleted = true;
                filesDataServer.StatusProcessingProject = StatusProcessingProject.Receiving;
            }
        }
    }
}
