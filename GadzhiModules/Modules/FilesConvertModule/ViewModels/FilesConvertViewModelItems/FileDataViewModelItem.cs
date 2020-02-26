using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using Prism.Mvvm;

namespace GadzhiModules.Modules.FilesConvertModule.ViewModels.FilesConvertViewModelItems
{
    public class FileDataViewModelItem : BindableBase
    {
        public FileDataViewModelItem(FileData filedata)
        {
            FileData = filedata;
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
            get => ColorPrintConverter.ConvertColorPrintToString(FileData.ColorPrint);
            set
            {
                FileData.ColorPrint = ColorPrintConverter.ConvertStringToColorPrint(value);
            }
        }

        /// <summary>
        /// Статус обработки строковое значение
        /// </summary>
        public string StatusProcessingName => StatusProcessingConverter.
                                              ConvertStatusProcessingToString(FileData.StatusProcessing);

        /// <summary>
        /// Есть ошибка при конвертировании
        /// </summary>
        public bool IsErrorStatus => FileData.StatusProcessing == StatusProcessing.Error;

        /// <summary>
        /// Завершилось ли конвертирование удачно
        /// </summary>
        public bool IsEndStatus => FileData.StatusProcessing == StatusProcessing.End;

        /// <summary>
        /// Обновление статуса обработки через событие
        /// </summary>
        public void UpdateStatusProcessing()
        {
            RaisePropertyChanged(nameof(StatusProcessingName));
            RaisePropertyChanged(nameof(IsErrorStatus));
            RaisePropertyChanged(nameof(IsEndStatus));
        }

    }
}
