using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Converters.LibraryData;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi;
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
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        private readonly IPackageData _packageInfoProject;

        /// <summary>
        /// Параметры приложения
        /// </summary>     
        private readonly IProjectSettings _projectSettings;

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
                                 IProjectSettings projectSettings,
                                 IFileSystemOperations fileSystemOperations,
                                 IPackageData packageInfoProject,
                                 IServiceConsumer<IFileConvertingClientService> fileConvertingClientService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark,
                                 IStatusProcessingInformation statusProcessingInformation)
        {
            _dialogServiceStandard = dialogServiceStandard ?? throw new ArgumentNullException(nameof(dialogServiceStandard));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _packageInfoProject = packageInfoProject ?? throw new ArgumentNullException(nameof(packageInfoProject));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
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

        /// <summary>
        /// Получить параметры приложения из сохраненной конфигурации
        /// </summary>
        public static IConvertingSettings GetConvertingSettingFromConfiguration() =>
            new PersonInformation(Properties.Settings.Default.PersonSurname ?? String.Empty,
                                  Properties.Settings.Default.PersonName ?? String.Empty,
                                  Properties.Settings.Default.PersonPatronymic ?? String.Empty,
                                  ConverterDepartmentType.DepartmentStringToTypeOrUnknown(Properties.Settings.Default.PersonDepartment)).
            Map(personInformation => new ConvertingSettings(new SignatureLibrary(Properties.Settings.Default.PersonId, personInformation),
                                                             (PdfNamingType)Properties.Settings.Default.PdfNamingType));

        /// <summary>
        /// Сохранить конфигурацию приложения
        /// </summary>
        private void SaveConfiguration()
        {
            Properties.Settings.Default.PersonId = _projectSettings.ConvertingSettings.PersonSignature.PersonId;
            Properties.Settings.Default.PersonSurname = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Surname;
            Properties.Settings.Default.PersonName = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Name;
            Properties.Settings.Default.PersonPatronymic = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Patronymic;
            Properties.Settings.Default.PersonDepartment = _projectSettings.ConvertingSettings.PersonSignature.PersonInformation.Department;
            Properties.Settings.Default.PdfNamingType = (int)_projectSettings.ConvertingSettings.PdfNamingType;
            Properties.Settings.Default.Save();
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
            SaveConfiguration();

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
