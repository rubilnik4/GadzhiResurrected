using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels
{
    /// <summary>
    /// Данные поиска истории
    /// </summary>
    public class HistoryDataViewModel : BindableBase
    {
        public HistoryDataViewModel(HistoryClientServiceFactory historyClientServiceFactory,
                                    ConvertingClientServiceFactory convertingClientServiceFactory,
                                    IConverterClientPackageDataFromDto converterClientPackageDataFromDto,
                                    IDialogService dialogService)
        {
            HistoryDataViewModelPart = new HistoryDataViewModelPart(historyClientServiceFactory, convertingClientServiceFactory,
                                                                    converterClientPackageDataFromDto, dialogService,
                                                                    isLoading => IsLoading = IsLoading, SetHistoryFiles);
            HistoryFileDataViewModelPart = new HistoryFileDataViewModelPart(() => IsPackageMode = true);
        }

        /// <summary>
        /// Данные пакетов поиска истории
        /// </summary>
        public HistoryDataViewModelPart HistoryDataViewModelPart { get; }

        /// <summary>
        /// Данные истории конвертации файла
        /// </summary>
        public HistoryFileDataViewModelPart HistoryFileDataViewModelPart { get; }

        /// <summary>
        /// Загрузка
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// Загрузка
        /// </summary>
        public bool IsLoading 
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value); 
        }

        /// <summary>
        /// Режим просмотра пакетов
        /// </summary>
        private bool _isPackageMode = true;

        /// <summary>
        /// Режим просмотра пакетов
        /// </summary>
        public bool IsPackageMode
        {
            get => _isPackageMode;
            private set
            {
                SetProperty(ref _isPackageMode, value);
                RaisePropertyChanged(nameof(IsFileMode));
            }
        }

        /// <summary>
        /// Режим просмотра файлов
        /// </summary>
        public bool IsFileMode => !IsPackageMode;

        /// <summary>
        /// Установить режим просмотра файлов
        /// </summary>
        private void SetHistoryFiles(IReadOnlyCollection<HistoryFileDataViewModelItem> historyFiles)
        {
            HistoryFileDataViewModelPart.HistoryFileViewModelItems = historyFiles;
            IsPackageMode = false;
        }
    }
}