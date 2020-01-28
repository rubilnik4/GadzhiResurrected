using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
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
        /// <summary>
        /// Модель данных для хранения информации о конвертируемом файле
        /// </summary>
        private FileData _filedata;

        public FileDataViewModelItem()
        {

        }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileType => _filedata.FileType;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName => _filedata.FileName;

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath => _filedata.FilePath;

        /// <summary>
        /// Цвет печати строковое значение
        /// </summary>
        public string ColorPrintName => _filedata.ColorPrintName;        

        /// <summary>
        /// Статус обработки строковое значение
        /// </summary>
        public string StatusProcessingName => _filedata.StatusProcessingName;
       
    }
}
