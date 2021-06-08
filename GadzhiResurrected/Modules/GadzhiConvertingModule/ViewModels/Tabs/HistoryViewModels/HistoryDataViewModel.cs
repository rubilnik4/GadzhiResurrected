using System.Collections.Generic;
using GadzhiResurrected.Infrastructure.Implementations.Services;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Infrastructure.Interfaces.Converters;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems;
using Prism.Mvvm;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels
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