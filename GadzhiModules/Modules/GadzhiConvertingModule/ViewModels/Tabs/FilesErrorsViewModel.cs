using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesErrorsViewModelItems;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// Представление ошибок конвертации
    /// </summary>
    public class FilesErrorsViewModel : ViewModelBase
    {
        /// <summary>
        /// Текущий статус конвертирования
        /// </summary>        
        private readonly IStatusProcessingInformation _statusProcessingInformation;

        /// <summary>
        /// Подписка на обновление модели
        /// </summary>
        private readonly IDisposable _fileDataChangeSubscribe;

        public FilesErrorsViewModel(IApplicationGadzhi applicationGadzhi, IStatusProcessingInformation statusProcessingInformation)
        {
            _fileDataChangeSubscribe = applicationGadzhi?.FileDataChange.Subscribe(ActionOnTypeStatusChange) ?? throw new ArgumentNullException(nameof(applicationGadzhi));
            _statusProcessingInformation = statusProcessingInformation ?? throw new ArgumentNullException(nameof(statusProcessingInformation));

            FilesErrorsCollection = new ObservableCollection<FileErrorViewModelItem>();
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Ошибочки";

        /// <summary>
        /// Список ошибок
        /// </summary>
        public ObservableCollection<FileErrorViewModelItem> FilesErrorsCollection { get; }

        /// <summary>
        /// Изменения коллекции при корректировке статуса конвертирования
        /// </summary>
        private void ActionOnTypeStatusChange(FilesChange filesChange)
        {
            if (filesChange.IsStatusProcessingProjectChanged &&
                _statusProcessingInformation.StatusProcessingProject == StatusProcessingProject.End)
            {
                FilesErrorsCollection.Clear();
                var errorViewModelItems = filesChange.FilesData.SelectMany(GetErrorViewModelItemsFromFileData);
                FilesErrorsCollection.AddRange(errorViewModelItems);
            }
        }

        /// <summary>
        /// Получить представления об ошибках после конвертирования файлов
        /// </summary>
        private static IEnumerable<FileErrorViewModelItem> GetErrorViewModelItemsFromFileData(IFileData fileData) =>
            fileData.FileConvertErrorType.
            Select(errorType => new FileErrorViewModelItem(fileData.FileName, errorType, String.Empty));
    }
}