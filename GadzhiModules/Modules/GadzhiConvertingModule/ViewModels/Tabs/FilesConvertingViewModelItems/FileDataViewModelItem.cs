using System;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesConvertingViewModelItems
{
    public class FileDataViewModelItem : BindableBase
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        public FileDataViewModelItem(IFileData fileData)
        {
            FileData = fileData;
        }

        /// <summary>
        /// Модель данных для хранения информации о конвертируемом файле
        /// </summary>
        public IFileData FileData { get; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileExtension => FileData.FileExtension.ToString();

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName => FileData.FileName;

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath => FileData.FilePath;

        /// <summary>
        /// Цвет печати строковое значение
        /// </summary>
        public string ColorPrintName
        {
            get => ColorPrintConverter.ColorPrintToString(FileData.ColorPrint);
            set => FileData.ColorPrint = ColorPrintConverter.ConvertStringToColorPrint(value).
                   Void(_ => _loggerService.LogProperty(nameof(ColorPrintName), nameof(FileDataViewModelItem), LoggerInfoLevel.Info, value));
        }

        /// <summary>
        /// Статус обработки строковое значение
        /// </summary>
        public string StatusProcessingName => StatusProcessingConverter.
                                              StatusProcessingToString(FileData.StatusProcessing);

        /// <summary>
        /// Ошибка отсутствует
        /// </summary>
        public bool IsNoError => IsEndStatus && FileData.StatusError == StatusError.NoError;

        /// <summary>
        /// Ошибка отсутствует
        /// /// </summary>
        public bool IsCriticalError => IsEndStatus && FileData.StatusError == StatusError.CriticalError;

        /// <summary>
        /// Список ошибок
        /// </summary>
        public string ErrorsDescription =>
            FileData.FileErrors.Select(error => error.FileConvertErrorType).Select(ConverterErrorType.ErrorTypeToString).
            Map(errors => String.Join("\n", errors));

        /// <summary>
        /// Завершилось ли конвертирование удачно
        /// </summary>
        private bool IsEndStatus => FileData.StatusProcessing == StatusProcessing.End;

        /// <summary>
        /// Обновление статуса обработки через событие
        /// </summary>
        public void UpdateStatusProcessing()
        {
            RaisePropertyChanged(nameof(StatusProcessingName));
            RaisePropertyChanged(nameof(IsEndStatus));
            RaisePropertyChanged(nameof(IsNoError));
            RaisePropertyChanged(nameof(IsCriticalError));
            RaisePropertyChanged(nameof(ErrorsDescription));
        }

    }
}
