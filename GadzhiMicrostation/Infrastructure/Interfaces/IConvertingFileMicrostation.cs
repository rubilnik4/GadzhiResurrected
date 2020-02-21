using GadzhiMicrostation.Models.Implementations;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{

    /// <summary>
    /// Обработка и конвертирование файла DGN
    /// </summary>
    public interface IConvertingFileMicrostation
    {
        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary> 
        void ConvertingFile(FileDataMicrostation fileDataMicrostation);
    }
}
