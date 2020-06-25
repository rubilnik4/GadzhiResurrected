﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using GadzhiCommon.Enums.FilesConvert;
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
    public class FilesErrorsViewModel : ViewModelBase, IDisposable
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
            BindingOperations.EnableCollectionSynchronization(FilesErrorsCollection, _filesErrorsCollectionLock);
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Ошибки";

        /// <summary>
        /// Видимость
        /// </summary>
        public override bool Visibility => FilesErrorsCollection.Count > 0;

        /// <summary>
        /// Блокировка списка ошибок для других потоков
        /// </summary>
        private readonly object _filesErrorsCollectionLock = new object();

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

                VisibilityChange.OnNext(Visibility);
            }
        }

        /// <summary>
        /// Получить представления об ошибках после конвертирования файлов
        /// </summary>
        private static IEnumerable<FileErrorViewModelItem> GetErrorViewModelItemsFromFileData(IFileData fileData) =>
            fileData.FileErrors.
            Select(errorType => new FileErrorViewModelItem(fileData.FileName, errorType.FileConvertErrorType, errorType.ErrorDescription));

        #region IDisposable Support
        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_disposedValue) return;

            if (disposing)
            {
                _fileDataChangeSubscribe?.Dispose();
            }

            _disposedValue = true;
        }

        public new void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}