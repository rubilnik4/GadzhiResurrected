using System;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiResurrected.Infrastructure.Implementations.Converters;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using Prism.Mvvm;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesConvertingViewModelItems
{
    public class FileDataViewModelItem : BindableBase
    {
        public FileDataViewModelItem(IFileData fileData, Action<ColorPrintType> setColorPrint)
        {
            FileData = fileData;
            _setColorPrint = setColorPrint;
        }

        /// <summary>
        /// Модель данных для хранения информации о конвертируемом файле
        /// </summary>
        public IFileData FileData { get; }

        /// <summary>
        /// Установка цвета печати выделенных файлов
        /// </summary>
        private readonly Action<ColorPrintType> _setColorPrint;

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileExtension => FileData.FileExtensionType.ToString();

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
        [Logger]
        public string ColorPrintName
        {
            get => ColorPrintConverter.ColorPrintToString(FileData.ColorPrintType);
            set => _setColorPrint(ColorPrintConverter.ColorPrintFromString(value));
        }

        /// <summary>
        /// Статус обработки строковое значение
        /// </summary>
        public string StatusProcessingName => StatusProcessingConverter.
                                              StatusProcessingToString(FileData.StatusProcessing);

        /// <summary>
        /// Ошибка отсутствует
        /// </summary>
        public bool IsNoError => IsEndStatus && FileData.StatusErrorType == StatusErrorType.NoError;

        /// <summary>
        /// Ошибка отсутствует
        /// /// </summary>
        public bool IsCriticalError => IsEndStatus && FileData.StatusErrorType == StatusErrorType.CriticalError;

        /// <summary>
        /// Список ошибок
        /// </summary>
        public string ErrorsDescription =>
            FileData.FileErrors.Select(error => error.ErrorConvertingType).Select(ConverterErrorType.ErrorTypeToString).
            Map(errors => String.Join("\n", errors));

        /// <summary>
        /// Завершилось ли конвертирование удачно
        /// </summary>
        private bool IsEndStatus => FileData.StatusProcessing == StatusProcessing.End;

        /// <summary>
        /// Изменить цвет печати
        /// </summary>
        public void ChangeColorPrint(ColorPrintType colorPrintType)
        {
            FileData.SetColorPrint(colorPrintType) ;
            RaisePropertyChanged(nameof(ColorPrintName));
        }

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
