using GadzhiConverting.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    public interface IConvertingFileData
    {
        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        Task<FileDataServer> Converting(FileDataServer fileDataServer);       
    }
}
