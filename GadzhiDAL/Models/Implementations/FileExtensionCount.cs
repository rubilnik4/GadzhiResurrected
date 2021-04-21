using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDAL.Models.Implementations
{
    /// <summary>
    /// Подсчет количества типов
    /// </summary>
    public class FileExtensionCount
    {
        public FileExtensionCount(FileExtensionType fileExtensionType, int count)
        {
            FileExtensionType = fileExtensionType;
            Count = count;
        }

        /// <summary>
        /// Типы расширения
        /// </summary>   
        public FileExtensionType FileExtensionType { get; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; }
    } 
}