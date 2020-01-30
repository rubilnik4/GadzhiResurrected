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
        /// Поместить файлы в очередь для конвертации
        /// </summary>
        void QueueFilesData(FilesDataServer filesDataServer);
    }
}
