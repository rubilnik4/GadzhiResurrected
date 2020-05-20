using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesConvertingViewModelItems
{
    public class FileDataViewModelItem : BindableBase
    {
        public FileDataViewModelItem(FileData fileData)
        {
            FileData = fileData;
        }

        /// <summary>
        /// Модель данных для хранения информации о конвертируемом файле
        /// </summary>
        public FileData FileData { get; }

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
            set => FileData.ColorPrint = ColorPrintConverter.ConvertStringToColorPrint(value);
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
        /// Информационная ошибка
        /// </summary>
        public bool IsInformationError => IsEndStatus &&  FileData.StatusError == StatusError.InformationError;

        /// <summary>
        /// Ошибка отсутствует
        /// /// </summary>
        public bool IsCriticalError => IsEndStatus &&  FileData.StatusError == StatusError.CriticalError;

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
            RaisePropertyChanged(nameof(IsInformationError));
            RaisePropertyChanged(nameof(IsCriticalError));
        }

    }
}
