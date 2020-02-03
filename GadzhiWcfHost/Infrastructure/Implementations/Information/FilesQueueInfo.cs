﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Infrastructure.Implementations.Information
{
    /// <summary>
    /// Информация о количестве файлов в очереди 
    /// </summary>
    public class FilesQueueInfo
    {
        public FilesQueueInfo(int filesInQueueCount, int packagesInQueueCount)
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
