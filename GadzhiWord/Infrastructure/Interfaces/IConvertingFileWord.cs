using ConvertingModels.Models.Interfaces.FilesConvert;
using ConvertingModels.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Infrastructure.Interfaces
{
    /// <summary>
    /// Обработка и конвертирование файла DOC
    /// </summary>
    public interface IConvertingFileWord
    {
        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        IFileDataServer ConvertingFile(IFileDataServer fileDataServer, IPrintersInformation printersInformation);
    }
}
