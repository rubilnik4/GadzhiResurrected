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
    public class FilesDataServer
    {
        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        private List<FileDataServer> _filesDataToConverting;

        public FilesDataServer(IEnumerable<FileDataServer> filesDataServer)
        {
            _filesDataToConverting = new List<FileDataServer>();
            if (filesDataServer != null)
            {
                _filesDataToConverting.AddRange(filesDataServer);
            }
        }

       
    }
}
