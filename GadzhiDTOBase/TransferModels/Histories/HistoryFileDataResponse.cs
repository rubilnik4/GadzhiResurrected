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
                                       IList<FileExtensionType> fileExtensionTypes, int errorCount, IList<string> paperSizes)
        {
            FilePath = filePath;
            StatusProcessing = statusProcessing;
            FileExtensionTypesList = fileExtensionTypes;
            ErrorCount = errorCount;
            PaperSizesList = paperSizes;
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

        /// <summary>
        /// Форматы
        /// </summary>
        [DataMember]
        public IList<string> PaperSizesList { get; private set; }

        /// <summary>
        /// Форматы
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyCollection<string> PaperSizes =>
            PaperSizesList.ToList();
    }
}