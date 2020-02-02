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
        /// Получить первый в очереди пакет
        /// </summary>      
        public FilesDataServer GetFirstInQueuePackage() => _filesDataToConverting?.Peek();

        /// <summary>
        /// Поместить файлы в очередь для конвертации
        /// </summary>
        public void QueueFilesData(FilesDataServer filesDataServer)
        {
            filesDataServer.StatusProcessingProject = StatusProcessingProject.InQueue;
            filesDataServer.SetStatusToAllFiles(StatusProcessing.InQueue);

            _filesDataToConverting.Enqueue(filesDataServer);
        }

        /// <summary>
        /// Запустить конвертацию пакета
        /// </summary>
        public async Task ConvertingFilesDataPackage(FilesDataServer filesDataServer)
        {
            filesDataServer.StatusProcessingProject = StatusProcessingProject.Converting;

            foreach (var fileData in filesDataServer.FilesDataInfo)
            {
                fileData.StatusProcessing = StatusProcessing.InProcess;
                await Task.Delay(2000);
                fileData.StatusProcessing = StatusProcessing.Completed;
            }

            filesDataServer.StatusProcessingProject = StatusProcessingProject.Receiving;
            filesDataServer.IsCompleted = true;
        }
    }
}