using ConvertingModels.Models.Implementations.FilesConvert;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiWord.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле для приложения
    /// </summary>
    public class FileDataServerWord : FileDataServer, IFileDataServerWord
    {
        public FileDataServerWord(string filePathServer, string filePathClient, ColorPrint colorPrint)
              : this(filePathServer, filePathClient, colorPrint, new List<FileConvertErrorType>())
        {

        }

        public FileDataServerWord(string filePathServer, string filePathClient,
                                  ColorPrint colorPrint, IEnumerable<FileConvertErrorType> fileConvertErrorType)
            : base(filePathServer, filePathClient, colorPrint, fileConvertErrorType)
        {

        }

        /// <summary>
        /// Добавить путь к отконвертированному файлу Word
        /// </summary>
        public void AddConvertedFilePath(IFileDataSourceServerWord fileDataSourceServer) => AddConvertedFilePathBase(fileDataSourceServer);

        /// <summary>
        /// Добавить пути к отконвертированным файлам Word
        /// </summary>
        public void AddRangeConvertedFilePath(IEnumerable<IFileDataSourceServerWord> fileDatasSourceServer) =>
            AddRangeConvertedFilePathBase(fileDatasSourceServer.Cast<IFileDataSourceServer>());
    }
}
