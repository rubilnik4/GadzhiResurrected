using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Models.FilesConvert.Implementations
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class FilesDataServer : IFilesDataServer
    {
        public FilesDataServer()
        {

        }

        /// <summary>
        /// Очередь неконвертированных файлов
        /// </summary>
        public Queue<FileDataServer> FileDataToConverting { get; private set; }
    }
}
