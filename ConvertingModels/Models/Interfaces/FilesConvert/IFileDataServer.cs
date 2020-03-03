using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public interface IFileDataServer
    {
        /// <summary>
        /// Тип расширения файла
        /// </summary>
        FileExtention FileExtentionType { get; }

        /// <summary>
        /// Путь файла на сервере
        /// </summary>
        string FilePathServer { get; }

        /// <summary>
        /// Путь файла на клиенте
        /// </summary>
        string FilePathClient { get; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        ColorPrint ColorPrint { get; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        IEnumerable<FileConvertErrorType> FileConvertErrorTypes { get; }

        /// <summary>
        /// Путь и тип отконвертированных файлов
        /// </summary>
        IEnumerable<IFileDataSourceServer> FileDatasSourceServer { get; }     
    }
}
