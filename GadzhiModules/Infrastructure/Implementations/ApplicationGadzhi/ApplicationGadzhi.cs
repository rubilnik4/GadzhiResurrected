using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiModules.Infrastructure.Implementations.ApplicationGadzhi
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public partial class ApplicationGadzhi : IApplicationGadzhi
    {
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        private readonly IPackageData _packageInfoProject;

        /// <summary>
        /// Сервис конвертации
        /// </summary>     
        private readonly IServiceConsumer<IFileConvertingClientService> _fileConvertingClientService;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private readonly IDialogServiceStandard _dialogServiceStandard;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Текущий статус конвертирования
        /// </summary>        
        private readonly IStatusProcessingInformation _statusProcessingInformation;

        /// <summary>
        /// Получение файлов для изменения статуса
        /// </summary>     
        private readonly IFileDataProcessingStatusMark _fileDataProcessingStatusMark;

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов. Таймер с подпиской
        /// </summary>
        private readonly CompositeDisposable _statusProcessingSubscriptions;

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSystemOperations fileSystemOperations,
                                 IPackageData packageInfoProject,
                                 IServiceConsumer<IFileConvertingClientService> fileConvertingClientService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark,
                                 IStatusProcessingInformation statusProcessingInformation)
        {
            _dialogServiceStandard = dialogServiceStandard ?? throw new ArgumentNullException(nameof(dialogServiceStandard));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _packageInfoProject = packageInfoProject ?? throw new ArgumentNullException(nameof(packageInfoProject));
            _fileConvertingClientService = fileConvertingClientService ?? throw new ArgumentNullException(nameof(fileConvertingClientService));
            _fileDataProcessingStatusMark = fileDataProcessingStatusMark ?? throw new ArgumentNullException(nameof(fileDataProcessingStatusMark));
            _statusProcessingInformation = statusProcessingInformation ?? throw new ArgumentNullException(nameof(statusProcessingInformation));

            _statusProcessingSubscriptions = new CompositeDisposable();
        }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            if (_statusProcessingInformation.IsConverting &&
                !_dialogServiceStandard.ShowMessageOkCancel("Бросить все на полпути?"))
            {
                return;
            }
            Application.Current.Shutdown();
        }

        #region IDisposable Support
        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {

            }
            AbortPropertiesConverting(true).ConfigureAwait(false);

            _disposedValue = true;
        }

        ~ApplicationGadzhi()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion      
    }
}
