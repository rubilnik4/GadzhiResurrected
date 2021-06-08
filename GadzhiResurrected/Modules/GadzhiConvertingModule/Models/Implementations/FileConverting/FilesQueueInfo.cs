using System;
using System.Globalization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting
{
    /// <summary>
    /// Информация о количестве файлов в очереди на сервере
    /// </summary>
    public readonly struct FilesQueueInfo : IEquatable<FilesQueueInfo>, IFormattable
    {
        public FilesQueueInfo(int filesInQueueCount, int packagesInQueueCount, int firstFilesCountInQueue)
        {
            FilesInQueueCount = filesInQueueCount;
            PackagesInQueueCount = packagesInQueueCount;
            FirstFilesCountInQueue = firstFilesCountInQueue;
        }

        /// <summary>
        /// Количество файлов в очереди
        /// </summary>     
        public int FilesInQueueCount { get; }

        /// <summary>
        /// Количество пакетов в очереди
        /// </summary>      
        public int PackagesInQueueCount { get; }

        /// <summary>
        /// Первоначальное количество файлов в очереди. Для расчета процентов выполнения
        /// </summary>
        public int FirstFilesCountInQueue { get; }

        /// <summary>
        /// Получить информацию о количестве файлов из промежуточного запроса
        /// </summary>        
        public static FilesQueueInfo GetQueueInfoByStatus(QueueStatus queueStatus, StatusProcessingProject statusProcessingProject) =>
            statusProcessingProject switch
            {
                StatusProcessingProject.InQueue => new FilesQueueInfo(queueStatus.FilesInQueueCount,
                                                                      queueStatus.PackagesInQueueCount,
                                                                      queueStatus.FilesInQueueCount),
                _ => new FilesQueueInfo(),
            };

        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => $"Files in queue:{FilesInQueueCount}";
        #endregion

        #region IEquatable
        public bool Equals(FilesQueueInfo other) =>
            FilesInQueueCount == other.FilesInQueueCount &&
            PackagesInQueueCount == other.PackagesInQueueCount &&
            FirstFilesCountInQueue == other.FirstFilesCountInQueue;

        public override bool Equals(object obj) => obj is FilesQueueInfo other && Equals(other);

        public override int GetHashCode()
        {
            int hashCode = FilesInQueueCount;
            hashCode = (hashCode * 397) ^ PackagesInQueueCount;
            hashCode = (hashCode * 397) ^ FirstFilesCountInQueue;
            return hashCode;
        }

        public static bool operator ==(FilesQueueInfo left, FilesQueueInfo right) => left.Equals(right);

        public static bool operator !=(FilesQueueInfo left, FilesQueueInfo right) => !(left == right);

        #endregion
    }
}
