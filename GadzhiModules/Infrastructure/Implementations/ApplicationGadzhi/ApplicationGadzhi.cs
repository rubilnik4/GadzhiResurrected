using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Converters.LibraryData;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiModules.Infrastructure.Interfaces.Services;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiModules.Infrastructure.Implementations.ApplicationGadzhi
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public partial class ApplicationGadzhi : IApplicationGadzhi
    {
        public ApplicationGadzhi(IDialogService dialogService,
                                 IProjectSettings projectSettings,
                                 IFileSystemOperations fileSystemOperations,
                                 IPackageData packageInfoProject,
                                 IWcfClientServicesFactory wcfClientServiceFactory,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark,
                                 IStatusProcessingInformation statusProcessingInformation)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _packageData = packageInfoProject ?? throw new ArgumentNullException(nameof(packageInfoProject));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            _wcfClientServiceFactory = wcfClientServiceFactory ?? throw new ArgumentNullException(nameof(wcfClientServiceFactory));
            _fileDataProcessingStatusMark = fileDataProcessingStatusMark ?? throw new ArgumentNullException(nameof(fileDataProcessingStatusMark));
            _statusProcessingInformation = statusProcessingInformation ?? throw new ArgumentNullException(nameof(statusProcessingInformation));

            _statusProcessingSubscriptions = new CompositeDisposable();
        }

        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        private readonly IPackageData _packageData;

        /// <summary>
        /// Параметры приложения
        /// </summary>     
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Фабрика для создания сервисов WCF
        /// </summary>
        private readonly IWcfClientServicesFactory _wcfClientServiceFactory;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private readonly IDialogService _dialogService;

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

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        [Logger]
        public async Task CloseApplication()
        {
            if (_statusProcessingInformation.IsConverting)
            {
                bool okResult = await _dialogService.ShowMessageOkCancel("Бросить все на полпути?");
                if (!okResult) return;
            }

            Application.Current.Shutdown();
        }

        /// <summary>
        /// Получить параметры приложения из сохраненной конфигурации
        /// </summary>
        [Logger]
        public static IConvertingSettings GetConvertingSettingFromConfiguration() =>
            new PersonInformation(Properties.Settings.Default.PersonSurname ?? String.Empty,
                                  Properties.Settings.Default.PersonName ?? String.Empty,
                                  Properties.Settings.Default.PersonPatronymic ?? String.Empty,
                                  ConverterDepartmentType.DepartmentStringToTypeOrUnknown(Properties.Settings.Default.PersonDepartment)).
            Map(personInformation => new ConvertingSettings(new SignatureLibrary(Properties.Settings.Default.PersonId, personInformation),
                                                            (PdfNamingType)Properties.Settings.Default.PdfNamingType,
                                                            (ColorPrint)Properties.Settings.Default.ColorPrint));

        /// <summary>
        /// Сохранить конфигурацию приложения
        /// </summary>
        [Logger]
        private void SaveConfiguration()
        {
            Properties.Settings.Default.PersonId = _projectSettings.ConvertingSettings.PersonSignature.PersonId;
            Properties.Settings.Default.PersonSurname = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Surname;
            Properties.Settings.Default.PersonName = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Name;
            Properties.Settings.Default.PersonPatronymic = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Patronymic;
            Properties.Settings.Default.PersonDepartment = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Department;
            Properties.Settings.Default.PdfNamingType = (int)_projectSettings.ConvertingSettings.PdfNamingType;
            Properties.Settings.Default.ColorPrint = (int)_projectSettings.ConvertingSettings.ColorPrint;
            Properties.Settings.Default.Save();
        }

        #region IDisposable Support
        private bool _disposedValue;

        [Logger]
        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            AbortPropertiesCancellation().ConfigureAwait(false);
            _wcfClientServiceFactory?.Dispose();
            SaveConfiguration();

            if (disposing)
            {
                // Ликвидация модели после отключения всех подписок
                _packageData.Dispose();
                _statusProcessingSubscriptions.Dispose();
            }
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
