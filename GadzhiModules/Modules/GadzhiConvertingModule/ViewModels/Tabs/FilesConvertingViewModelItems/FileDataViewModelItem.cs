using System;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesConvertingViewModelItems
{
    public class FileDataViewModelItem : BindableBase
    {
        public FileDataViewModelItem(IFileData fileData, Action<ColorPrint> setColorPrint)
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
        private readonly Action<ColorPrint> _setColorPrint;

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
        [Logger]
        public string ColorPrintName
        {
            get => ColorPrintConverter.ColorPrintToString(FileData.ColorPrint);
            set => _setColorPrint(ColorPrintConverter.ConvertStringToColorPrint(value));
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
            FileData.FileErrors.Select(error => error.ErrorConvertingType).Select(ConverterErrorType.ErrorTypeToString).
            Map(errors => String.Join("\n", errors));

        /// <summary>
        /// Завершилось ли конвертирование удачно
        /// </summary>
        private bool IsEndStatus => FileData.StatusProcessing == StatusProcessing.End;

        /// <summary>
        /// Изменить цвет печати
        /// </summary>
        public void ChangeColorPrint(ColorPrint colorPrint)
        {
            FileData.SetColorPrint(colorPrint) ;
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
