using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для функций конвертирования файлов
    /// </summary>
    public class ApplicationConverting: IApplicationConverting
    {
        /// <summary>
        /// Класс пользовательских пакетов на конвертирование
        /// </summary>
        public IFilesDataPackages FileDataPackage { get; }

        public ApplicationConverting(IFilesDataPackages fileDataPackage)
        {
            FileDataPackage = fileDataPackage;
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        public void QueueFilesData(FilesDataServer filesDataServer)
        {
            FileDataPackage.QueueFilesData(filesDataServer);
        }
    }
}