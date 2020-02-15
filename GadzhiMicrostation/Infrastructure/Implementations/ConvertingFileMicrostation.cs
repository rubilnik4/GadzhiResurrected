using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Microstation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Обработка и конвертирование файла DGN
    /// </summary>
    public class ConvertingFileMicrostation : IConvertingFileMicrostation
    {
        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        private readonly IApplicationMicrostation _applicationMicrostation;

        public ConvertingFileMicrostation(IApplicationMicrostation applicationMicrostation)
        {
            _applicationMicrostation = applicationMicrostation;
        }

        /// <summary>
        /// Конвертировать файл
        /// </summary>      
        public void ConvertingFile(string filePath)
        {
            if (_applicationMicrostation.IsApplicationValid)
            {
                _applicationMicrostation.OpenDesignFile(filePath);
            }
            else
            {

            }
          

         
        }
    }
}
