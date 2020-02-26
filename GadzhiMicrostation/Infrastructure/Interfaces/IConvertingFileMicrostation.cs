using GadzhiMicrostation.Models.Implementations;
using System;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{

    /// <summary>
    /// Обработка и конвертирование файла DGN
    /// </summary>
    public interface IConvertingFileMicrostation: IDisposable
    {
        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary> 
        FileDataMicrostation ConvertingFile(FileDataMicrostation fileDataMicrostation, PrintersInformationMicrostation printersInformation);
    }
}
