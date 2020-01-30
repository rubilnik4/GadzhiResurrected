using GadzhiCommon.Enums.FilesConvert;
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
        /// Очередь неконвертированных пакетов
        /// </summary>
        public IEnumerable<FilesDataServer> FilesDataToConverting => _filesDataToConverting;

        /// <summary>
        /// Получить пакет конвертируемых файлов по идентефикатору
        /// </summary>      
        public FilesDataServer GetFilesDataServerByID(Guid FilesDataServerID) =>
            _filesDataToConverting?.FirstOrDefault(file => file.ID == FilesDataServerID);

        /// <summary>
        /// Поместить файлы в очередь для конвертации
        /// </summary>
        public void QueueFilesData(FilesDataServer filesDataServer)
        {
            filesDataServer.SetStatusToAllFiles(StatusProcessing.InQueue);

            _filesDataToConverting.Enqueue(filesDataServer);
        }
    }
}