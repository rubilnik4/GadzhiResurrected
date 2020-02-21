namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information
{
    /// <summary>
    /// Информация о количестве файлов в очереди на сервере
    /// </summary>
    public class FilesQueueStatus
    {
        public FilesQueueStatus(int filesInQueueCount, int packagesInQueueCount)
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
    }
}
