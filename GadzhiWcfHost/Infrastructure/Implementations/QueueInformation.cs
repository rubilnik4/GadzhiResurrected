using GadzhiWcfHost.Infrastructure.Implementations.Information;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Информация о статусе конвертируемых файлов
    /// </summary>
    public class QueueInformation : IQueueInformation
    {
        /// <summary>
        /// Класс пользовательских пакетов на конвертирование
        /// </summary>
        public readonly IFilesDataPackages _filesDataPackages;

        public QueueInformation(IFilesDataPackages filesDataPackages)
        {
            _filesDataPackages = filesDataPackages;
        }

        /// <summary>
        /// Получить информацию о количестве конвертируемых файлов в очереди
        /// </summary>        
        public FilesQueueInfo GetQueueInfo()
        {
            return GetQueueInfoUpToIdPackageOrAll();
        }

        /// <summary>
        /// Получить информацию о количестве конвертируемых файлов в очереди до опеределенного пакета
        /// </summary>        
        public FilesQueueInfo GetQueueInfoUpToIdPackage(Guid id)
        {
            return GetQueueInfoUpToIdPackageOrAll(id);
        }

        /// <summary>
        /// Получить информацию о количестве конвертируемых файлов в очереди до опеределенного пакета или все пакеты
        /// </summary>        
        private FilesQueueInfo GetQueueInfoUpToIdPackageOrAll(Guid? id = null)
        {
            int packagesInQueue = _filesDataPackages?.
                                  FilesDataPackagesToConverting.
                                  Where (package => !package.IsCompleted).
                                  TakeWhile(package => id == null || package.ID != id).
                                  Count() ?? 0;

            int filesInQueue = _filesDataPackages?.
                               FilesDataPackagesToConverting.
                               Where(package => !package.IsCompleted).
                               TakeWhile(package => id == null || package.ID != id).
                               Sum(package => package?.
                                              FilesDataInfo?.
                                              Count(file => !file.IsCompleted)) ?? 0;

            return new FilesQueueInfo(filesInQueue,
                                      packagesInQueue);
        }
    }
}