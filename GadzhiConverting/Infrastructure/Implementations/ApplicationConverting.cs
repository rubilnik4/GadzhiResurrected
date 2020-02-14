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
using Unity;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Инфраструктура для конвертирования файлов
    /// </summary>
    public class ApplicationConverting : IApplicationConverting, IDisposable
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessageAndLoggingService _messageAndLoggingService;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        /// <summary>
        /// Запуск процесса конвертирования
        /// </summary>
        private readonly CompositeDisposable _convertingUpdaterSubsriptions;

        public ApplicationConverting(IUnityContainer container,
                                     IProjectSettings projectSettings,
                                     IFileSystemOperations fileSystemOperations,
                                     IMessageAndLoggingService messageAndLoggingService,
                                     IExecuteAndCatchErrors executeAndCatchErrors)
        {
            _container = container;
            _projectSettings = projectSettings;
            _fileSystemOperations = fileSystemOperations;
            _messageAndLoggingService = messageAndLoggingService;
            _executeAndCatchErrors = executeAndCatchErrors;

            _convertingUpdaterSubsriptions = new CompositeDisposable();
        }



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

                _convertingUpdaterSubsriptions.Add(
                    Observable.Interval(TimeSpan.FromSeconds(_projectSettings.IntervalSecondsToServer)).
                               Where(_ => !IsConverting).
                               Subscribe(async _ =>
                                         await _executeAndCatchErrors.
                                         ExecuteAndHandleErrorAsync(_container.Resolve<IConvertingService>().ConvertingFirstInQueuePackage,
                                                                    ApplicationBeforeMethod: () => IsConverting = true,
                                                                    ApplicationFinallyMethod: () => IsConverting = false)));
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Проверить параметры запуска, добавить ошибки
        /// </summary>
        private bool ValidateStartupParameters()
        {
            bool isDataBaseExist = _fileSystemOperations.IsFileExist(_projectSettings.SQLiteDataBasePath);
            if (!isDataBaseExist)
            {
                _messageAndLoggingService.ShowError(FileConvertErrorType.FileNotFound,
                                                    $"Файл базы данных {_projectSettings.SQLiteDataBasePath} не найден");
            }

            return isDataBaseExist;
        }

        public void Dispose()
        {
            _convertingUpdaterSubsriptions?.Dispose();
        }
    }
}
