using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Implementations.FilesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертация файла Microstation в модуль серверной части
    /// </summary>
    public static class ConverterFileDataServerFromMicrostation
    {
        /// <summary>
        /// Обновить информацию о конвертируемых пакетах в серверной части из модуля Microstation
        /// </summary>
        public static FileDataServer UpdateFileDataServerFromMicrostation(FileDataServer fileDataServer, FileDataMicrostation fileDataMicrostation)
        {
            if (fileDataServer != null && fileDataMicrostation != null)
            {
                fileDataServer.FileDatasSourceServer = fileDataMicrostation.FileDataSourceMicrostation.
                                                         Select(fileData => ConvertingFileDataSourceFromMicrostation(fileData));

                fileDataServer.AddRangeFileConvertErrorType(fileDataMicrostation.FileConvertErrorTypes.
                                                            Select(error => ConvertingFileErrorTypes(error)));
            }
            return fileDataServer;
        }

        /// <summary>
        /// Преобразовать информацию о отконвертированных чертежах из модуля Microstation в серверную часть
        /// </summary>        
        private static FileDataSourceServer ConvertingFileDataSourceFromMicrostation(FileDataSourceMicrostation fileDataSourceMicrostation) =>
            new FileDataSourceServer(fileDataSourceMicrostation.FilePath,
                                     ConvertingFileExtension(fileDataSourceMicrostation.FileExtentionMicrostation),
                                     fileDataSourceMicrostation.PaperSize,
                                     fileDataSourceMicrostation.PrinterName);

        /// <summary>
        /// Преобразование расширений типов файлов  
        /// </summary>       
        private static FileExtention ConvertingFileExtension(FileExtentionMicrostation fileExtentionMicrostation) =>
            Enum.TryParse(fileExtentionMicrostation.ToString(), out FileExtention fileExtension) ?
            fileExtension :
            throw new FormatException(nameof(fileExtentionMicrostation));

        /// <summary>
        /// Преобразование типов ошибок конвертации  
        /// </summary>       
        private static FileConvertErrorType ConvertingFileErrorTypes(ErrorMicrostationType errorMicrostationType) =>
            Enum.TryParse(errorMicrostationType.ToString(), out FileConvertErrorType fileConvertErrorType) ?
            fileConvertErrorType :
            throw new FormatException(nameof(errorMicrostationType));

    }
}
