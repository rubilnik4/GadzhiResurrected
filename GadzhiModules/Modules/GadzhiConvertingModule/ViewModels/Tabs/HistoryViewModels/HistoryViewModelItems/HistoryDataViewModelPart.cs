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
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using Prism.Commands;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems
{
    /// <summary>
    /// Данные пакетов поиска истории
    /// </summary>
    public class HistoryDataViewModelPart: BindableBase
    {
        public HistoryDataViewModelPart(HistoryClientServiceFactory historyClientServiceFactory,
                                    ConvertingClientServiceFactory convertingClientServiceFactory,
                                    IConverterClientPackageDataFromDto converterClientPackageDataFromDto,
                                    IDialogService dialogService, Action<bool> setLoading,
                                    Action<IReadOnlyCollection<HistoryFileDataViewModelItem>> setHistoryFileItems)
        {
            _historyClientServiceFactory = historyClientServiceFactory;
            _convertingClientServiceFactory = convertingClientServiceFactory;
            _converterClientPackageDataFromDto = converterClientPackageDataFromDto;
            _dialogService = dialogService;
            _setLoading = setLoading;
            _setHistoryFileItems = setHistoryFileItems;
            DownloadFilesDataCommand = new DelegateCommand(async () => await GetFilesData(), CanDownloadFilesData);
            ShowFilesDataCommand = new DelegateCommand(async () => await ShowFilesData(),
                                                       () => SelectedHistoryViewModelItem != null);
        }


        /// <summary>
        /// Фабрика для создания подключения к WCF сервису получения истории конвертирования
        /// </summary>
        private readonly HistoryClientServiceFactory _historyClientServiceFactory;

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису конвертирования для клиента
        /// </summary>
        private readonly ConvertingClientServiceFactory _convertingClientServiceFactory;

        /// <summary>
        /// Конвертеры из трансферной модели в локальную
        /// </summary>  
        private readonly IConverterClientPackageDataFromDto _converterClientPackageDataFromDto;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        /// Установить статус загрузки
        /// </summary>
        private readonly Action<bool> _setLoading;

        /// <summary>
        /// Установить режим просмотра файлов
        /// </summary>
        private readonly Action<IReadOnlyCollection<HistoryFileDataViewModelItem>> _setHistoryFileItems;

        /// <summary>
        /// Команда получения файлов
        /// </summary>
        public DelegateCommand DownloadFilesDataCommand { get; }

        /// <summary>
        /// Команда просмотра файлов
        /// </summary>
        public DelegateCommand ShowFilesDataCommand { get; }

        /// <summary>
        /// Данные истории конвертаций
        /// </summary>
        private IReadOnlyCollection<HistoryDataViewModelItem> _historyViewModelItems =
            new List<HistoryDataViewModelItem>();

        /// <summary>
        /// Данные истории конвертаций
        /// </summary>
        public IReadOnlyCollection<HistoryDataViewModelItem> HistoryViewModelItems
        {
            get => _historyViewModelItems;
            private set => SetProperty(ref _historyViewModelItems, value);
        }

        /// <summary>
        /// Выбранная позиция данных истории
        /// </summary>
        private HistoryDataViewModelItem _selectedHistoryViewModelItem;

        /// <summary>
        /// Выбранная позиция данных истории
        /// </summary>
        public HistoryDataViewModelItem SelectedHistoryViewModelItem
        {
            get => _selectedHistoryViewModelItem;
            set
            {
                SetProperty(ref _selectedHistoryViewModelItem, value);
                DownloadFilesDataCommand.RaiseCanExecuteChanged();
                ShowFilesDataCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Обновить данные истории конвертаций
        /// </summary>
        public Task UpdateHistoryData(HistoryDataRequest historyDataRequest) =>
            new ResultValue<HistoryDataRequest>(historyDataRequest).
            ResultVoidOk(_ => _setLoading(true)).
            ResultValueOkAsync(GetHistoryData).
            ResultVoidOkAsync(historyViewModelItems => HistoryViewModelItems = historyViewModelItems).
            ResultVoidAsync(_ => _setLoading(false));

        /// <summary>
        /// Получить данные истории конвертаций
        /// </summary>
        private async Task<IReadOnlyCollection<HistoryDataViewModelItem>> GetHistoryData(HistoryDataRequest historyDataRequest) =>
            await _historyClientServiceFactory.UsingServiceRetry(service => service.Operations.GetHistoryData(historyDataRequest)).
            ResultValueOkAsync(HistoryDataConverter.ToClients).
            ResultValueOkAsync(histories => histories.Select(history => new HistoryDataViewModelItem(history))).
            WhereContinueAsync(result => result.OkStatus,
                               okFunc: result => result.Value.ToList(),
                               badFunc: result => new List<HistoryDataViewModelItem>());

        /// <summary>
        /// Получить завершенные файлы
        /// </summary>
        private async Task GetFilesData() =>
            await new ResultValue<HistoryDataViewModelItem>(SelectedHistoryViewModelItem,
                                                            new ErrorCommon(ErrorConvertingType.ValueNotInitialized, "Строка не выделена")).
            ResultVoidOk(_ => _setLoading(true)).
            ResultValueOkBindAsync(package => _convertingClientServiceFactory.
                UsingServiceRetry(service => service.Operations.GetCompleteFiles(package.HistoryData.PackageId))).
            ResultValueOkAsync(package => _converterClientPackageDataFromDto.ToFilesStatusAndSaveFiles(package)).
            ResultValueOkAsync(packageStatus => Path.GetDirectoryName(packageStatus.FileStatus.First().FilePath)).
            ResultVoidAsync(_ => _setLoading(false)).
            ResultVoidOkAsync(filePath => _dialogService.ShowMessage($"Файлы загружены\nПуть: {filePath}")).
            ResultVoidBadAsync(errors => _dialogService.ShowErrors(errors));

        /// <summary>
        /// Доступность загрузки данных
        /// </summary>
        private bool CanDownloadFilesData() =>
            SelectedHistoryViewModelItem != null &&
            CheckStatusProcessing.CompletedStatusProcessingProject.Contains(SelectedHistoryViewModelItem.HistoryData.StatusProcessingProject);
        
        /// <summary>
        /// Просмотреть файлы из выбранного пакета
        /// </summary>
        private async Task ShowFilesData() =>
            await new ResultValue<HistoryDataViewModelItem>(SelectedHistoryViewModelItem,
                                                            new ErrorCommon(ErrorConvertingType.ValueNotInitialized, "Строка не выделена")).
            ResultVoidOk(_ => _setLoading(true)).
            ResultValueOkAsync(historyData => GetHistoryFileData(historyData.HistoryData.PackageId)).
            ResultVoidAsync(historyFileViewModelItems => _setHistoryFileItems(historyFileViewModelItems)).
            ResultVoidAsync(_ => _setLoading(false));

        /// <summary>
        /// Получить данные истории конвертаций
        /// </summary>
        private async Task<IReadOnlyCollection<HistoryFileDataViewModelItem>> GetHistoryFileData(Guid packageId) =>
            await _historyClientServiceFactory.UsingServiceRetry(service => service.Operations.GetHistoryFileData(packageId)).
            ResultValueOkAsync(HistoryFileDataConverter.ToClients).
            ResultValueOkAsync(historyFiles => historyFiles.Select(historyFile => new HistoryFileDataViewModelItem(historyFile))).
            WhereContinueAsync(result => result.OkStatus,
                               okFunc: result => result.Value.ToList(),
                               badFunc: result => new List<HistoryFileDataViewModelItem>());
    }
}