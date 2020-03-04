using ConvertingModels.Models.Implementations.FilesConvert;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле для Word
    /// </summary>
    public interface IFileDataServerWord : IFileDataServer
    {
        /// <summary>
        /// Добавить путь к отконвертированному файлу
        /// </summary>
        void AddConvertedFilePath(IFileDataSourceServerWord fileDataSourceServer);

        /// <summary>
        /// Добавить пути к отконвертированным файлам
        /// </summary>
        void AddRangeConvertedFilePath(IEnumerable<IFileDataSourceServerWord> fileDatasSourceServer);       
    }
}
