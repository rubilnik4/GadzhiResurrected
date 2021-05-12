using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;

namespace GadzhiDTOBase.TransferModels.Histories
{
    /// <summary>
    /// Данные истории конвертации файла. Трансферная модель
    /// </summary>
    [DataContract]
    public class HistoryFileDataResponse: IHistoryFileData
    {
        public HistoryFileDataResponse(string filePath, StatusProcessing statusProcessing,
                                       IList<FileExtensionType> fileExtensionTypes, int errorCount)
        {
            FilePath = filePath;
            StatusProcessing = statusProcessing;
            FileExtensionTypesList = fileExtensionTypes;
            ErrorCount = errorCount;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FilePath { get; private set; }

        /// <summary>
        /// Статус
        /// </summary>
        [DataMember]
        public StatusProcessing StatusProcessing { get; private set; }

        /// <summary>
        /// Типы обработанных файлов
        /// </summary>
        [DataMember]
        public IList<FileExtensionType> FileExtensionTypesList { get; private set; }

        /// <summary>
        /// Типы обработанных файлов
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyCollection<FileExtensionType> FileExtensionTypes =>
            FileExtensionTypesList.ToList();

        /// <summary>
        /// Количество ошибок
        /// </summary>
        [DataMember]
        public int ErrorCount { get; private set; }
    }
}