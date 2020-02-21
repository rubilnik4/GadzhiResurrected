using GadzhiMicrostation.Models.Implementations;

namespace GadzhiMicrostation.Infrastructure.Interface
{

    /// <summary>
    /// Обработка и конвертирование файла DGN
    /// </summary>
    public interface IConvertingFileMicrostation
    {

        void ConvertingFile(FileDataMicrostation fileDataMicrostation);
    }
}
