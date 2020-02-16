using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
