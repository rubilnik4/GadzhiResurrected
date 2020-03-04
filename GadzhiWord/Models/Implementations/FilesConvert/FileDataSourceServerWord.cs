using ConvertingModels.Models.Implementations.FilesConvert;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiWord.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Отконвертированный файл серверной части для приложения
    /// </summary>
    public class FileDataSourceServerWord: FileDataSourceServer, IFileDataSourceServerWord
    {
        public FileDataSourceServerWord(string filePath, FileExtention fileExtensionType)
         : this(filePath, fileExtensionType, "-", "-")
        {

        }

        public FileDataSourceServerWord(string filePath, FileExtention fileExtensionType, string paperSize, string printerName)
            :base (filePath, fileExtensionType, paperSize, printerName)
        {
           
        }
    }
}
