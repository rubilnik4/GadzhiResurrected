using GadzhiMicrostation.Factory;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using MicroStationDGN;
using System;
using System.IO;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationMicrostation
    {
        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        private readonly IExecuteAndCatchErrorsMicrostation _executeAndCatchErrorsMicrostation;

        /// <summary>
        /// Проверка состояния папок и файлов, архивация, сохранение
        /// </summary>
        private readonly IFileSystemOperationsMicrostation _fileSystemOperationsMicrostation;

        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        public IErrorMessagingMicrostation ErrorMessagingMicrostation { get; }

        /// <summary>
        /// Отображение системных сообщений
        /// </summary>
        public  ILoggerMicrostation LoggerMicrostation { get; }

        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        /// <summary>
        /// Управление печатью пдф
        /// </summary>
        private readonly IPdfCreatorService _pdfCreatorService;

        public ApplicationMicrostation(IExecuteAndCatchErrorsMicrostation executeAndCatchErrorsMicrostation,
                                       IFileSystemOperationsMicrostation fileSystemOperationsMicrostation,
                                       IErrorMessagingMicrostation errorMessagingMicrostation,
                                       ILoggerMicrostation loggerMicrostation,
                                       IMicrostationProject microstationProject,
                                       IPdfCreatorService pdfCreatorService)
        {
            _executeAndCatchErrorsMicrostation = executeAndCatchErrorsMicrostation;
            _fileSystemOperationsMicrostation = fileSystemOperationsMicrostation;
            ErrorMessagingMicrostation = errorMessagingMicrostation;
            LoggerMicrostation = loggerMicrostation;
            _microstationProject = microstationProject;
            _pdfCreatorService = pdfCreatorService;
        }

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application _application;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application Application
        {
            get
            {
                if (_application == null)
                {
                    _executeAndCatchErrorsMicrostation.ExecuteAndHandleError(() => _application = MicrostationInstance.Instance(),
                                                          errorMicrostation: new ErrorMicrostation(ErrorMicrostationType.ApplicationNotLoad,
                                                                                                   "Ошибка загрузки приложения Microstation"));
                }
                return _application;
            }
        }

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => Application != null;

        /// <summary>
        /// Текущий файл Microstation
        /// </summary>
        public IDesignFileMicrostation ActiveDesignFile =>
            new DesignFileMicrostation(_application.ActiveDesignFile, this, _microstationProject);

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            _application.Quit();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _pdfCreatorService.Dispose();
                }               

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion



    }
}
