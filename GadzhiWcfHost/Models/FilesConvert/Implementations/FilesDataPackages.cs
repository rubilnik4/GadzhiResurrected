using GadzhiCommon.Enums.FilesConvert;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private Queue<FilesDataServer> _filesDataPackagesToConverting;

        public FilesDataPackages()
        {
            _filesDataPackagesToConverting = new Queue<FilesDataServer>();
        }

        /// <summary>
        /// Очередь неконвертированных пакетов
        /// </summary>
        public IEnumerable<FilesDataServer> FilesDataPackagesToConverting => _filesDataPackagesToConverting;

        /// <summary>
        /// Получить пакет конвертируемых файлов по идентефикатору
        /// </summary>      
        public FilesDataServer GetFilesDataServerByID(Guid FilesDataServerID) =>
            _filesDataPackagesToConverting?.FirstOrDefault(file => file.ID == FilesDataServerID);

        /// <summary>
        /// Получить первый невыполненный в очереди пакет
        /// </summary>      
        public FilesDataServer GetFirstUncompliteInQueuePackage() => _filesDataPackagesToConverting?.
                                                                     FirstOrDefault(package => !package.IsCompleted);

        /// <summary>
        /// Поместить файлы в очередь для конвертации
        /// </summary>
        public void QueueFilesData(FilesDataServer filesDataServer)
        {
            filesDataServer.StatusProcessingProject = StatusProcessingProject.InQueue;
            filesDataServer.SetStatusToAllFiles(StatusProcessing.InQueue);

            _filesDataPackagesToConverting.Enqueue(filesDataServer);
        }

        /// <summary>
        /// Запустить конвертацию пакета
        /// </summary>
        public async Task ConvertingFilesDataPackage(FilesDataServer filesDataServer)
        {
            if (filesDataServer != null)
            {
                filesDataServer.StatusProcessingProject = StatusProcessingProject.Converting;

                foreach (var fileData in filesDataServer.FilesDataInfo)
                {
                    fileData.StatusProcessing = StatusProcessing.InProcess;
                    await Task.Delay(2000);
                    fileData.IsCompleted = true;
                    fileData.StatusProcessing = StatusProcessing.Completed;
                }

                filesDataServer.StatusProcessingProject = StatusProcessingProject.Receiving;
                filesDataServer.IsCompleted = true;
            }
        }
    }
}