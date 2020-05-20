using System;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information
{
    /// <summary>
    /// Информация о количестве файлов в очереди на сервере
    /// </summary>
    public readonly struct QueueStatus : IEquatable<QueueStatus>
    {
        public QueueStatus(int filesInQueueCount, int packagesInQueueCount)
        {
            FilesInQueueCount = filesInQueueCount;
            PackagesInQueueCount = packagesInQueueCount;
        }

        /// <summary>
        /// Количество файлов в очереди
        /// </summary>     
        public int FilesInQueueCount { get; }

        /// <summary>
        /// Количество пакетов в очереди
        /// </summary>      
        public int PackagesInQueueCount { get; }

        #region IEquatable
        public override bool Equals(object obj) => obj is QueueStatus other && Equals(other);

        public bool Equals(QueueStatus other) =>
            FilesInQueueCount == other.FilesInQueueCount && PackagesInQueueCount == other.PackagesInQueueCount;

        public override int GetHashCode() => (FilesInQueueCount * 397) ^ PackagesInQueueCount;

        public static bool operator ==(QueueStatus left, QueueStatus right) => left.Equals(right);

        public static bool operator !=(QueueStatus left, QueueStatus right) => !(left == right);
        #endregion
    }
}
