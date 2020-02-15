using GadzhiMicrostation.Factory;
using GadzhiMicrostation.Microstation.Interfaces;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public class ApplicationMicrostation : IApplicationMicrostation
    {
        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private readonly Application _applicationMicrostation;

        public ApplicationMicrostation()
        {
            try
            {
                _applicationMicrostation = MicrostationInstance.Instance();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => _applicationMicrostation != null;

        /// <summary>
        /// Текущий файл Microstation
        /// </summary>
        public IDesignFileMicrostation ActiveDesignFile => 
               new DesignFileMicrostation(_applicationMicrostation.ActiveDesignFile);

        /// <summary>
        /// Открыть файл
        /// </summary>
        public void OpenDesignFile(string filePath) => _applicationMicrostation.OpenDesignFile(filePath, false);


    }
}
