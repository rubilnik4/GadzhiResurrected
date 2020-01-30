using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Models.FilesConvert.Implementations
{
    /// <summary>
    /// Класс пользовательских пакетов на конвертирование
    /// </summary>
    public class FilesDataPackages : IFilesDataPackages
    {
        /// <summary>
        /// Очередь неконвертированных пакетов
        /// </summary>
        private Queue<FilesDataServer> _filesDataToConverting;

        public FilesDataPackages()
        {
            _filesDataToConverting = new Queue<FilesDataServer>();
        }

        /// <summary>
        /// Поместить файлы в очередь для конвертации
        /// </summary>
        public void QueueFilesData(FilesDataServer filesDataServer)
        {
            _filesDataToConverting.Enqueue(filesDataServer);
        }
    }
}