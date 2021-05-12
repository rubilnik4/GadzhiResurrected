using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.Histories
{
    /// <summary>
    /// Данные истории конвертации файла на клиенте
    /// </summary>
    public class HistoryFileDataClient: IHistoryFileData
    {
        public HistoryFileDataClient(string filePath, StatusProcessing statusProcessing, 
                                     IEnumerable<FileExtensionType> convertingFileTypes, int errorCount)
        {
            FilePath = filePath;
            StatusProcessing = statusProcessing;
            FileExtensionTypes = convertingFileTypes.ToList();
            ErrorCount = errorCount;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Статус
        /// </summary>
        public StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Типы обработанных файлов
        /// </summary>
        public IReadOnlyCollection<FileExtensionType> FileExtensionTypes { get; }

        /// <summary>
        /// Количество ошибок
        /// </summary>
        public int ErrorCount { get; }
    }
}