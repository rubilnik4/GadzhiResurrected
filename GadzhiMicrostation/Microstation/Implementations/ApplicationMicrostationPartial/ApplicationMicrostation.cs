using GadzhiMicrostation.Factory;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using MicroStationDGN;
using System;
using System.IO;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationMicrostation
    {  
        public ApplicationMicrostation()
        {
            
        }

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application _application;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application Application
        {
            get
            {
                if (_application == null)
                {
                    _application = MicrostationInstance.Instance();
                }
                return _application;
            }
        }

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => Application != null;
       
        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            _application.Quit();
        }
    }
}
