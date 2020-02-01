using GadzhiModules.Helpers.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string FileType => FileData.FileExtension;

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
        /// Обновление статуса обработки через событие
        /// </summary>
        public void UpdateStatusProcessing()
        {
            RaisePropertyChanged(nameof(StatusProcessingName));
        }

    }
}
