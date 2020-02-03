using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Models.FilesConvert.Interfaces
{
    /// <summary>
    /// Класс пользовательских пакетов на конвертирование
    /// </summary>
    public interface IFilesDataPackages
    {
        /// <summary>
        /// Очередь неконвертированных пакетов
        /// </summary>
        IEnumerable<FilesDataServer> FilesDataPackagesToConverting { get; }

        /// <summary>
        /// Получить пакет конвертируемых файлов по идентефикатору
        /// </summary>      
        FilesDataServer GetFilesDataServerByID(Guid FilesDataServerID);

        /// <summary>
        /// Получить первый в очереди пакет
        /// </summary>      
        FilesDataServer GetFirstUncompliteInQueuePackage();

        /// <summary>
        /// Поместить файлы в очередь для конвертации
        /// </summary>
        void QueueFilesData(FilesDataServer filesDataServer);

        /// <summary>
        /// Запустить конвертацию пакета
        /// </summary>
        Task ConvertingFilesDataPackage(FilesDataServer filesDataServer);     
    }
}
