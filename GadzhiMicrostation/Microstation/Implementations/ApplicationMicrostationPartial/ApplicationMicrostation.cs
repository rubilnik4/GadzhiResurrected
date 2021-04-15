using GadzhiMicrostation.Factory;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using MicroStationDGN;
using System;
using System.IO;
using System.Runtime.InteropServices;
using GadzhiApplicationCommon.Models.Implementation.Resources;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationMicrostation
    {
        /// <summary>
        /// Ресурсы, используемые модулем Microstation
        /// </summary>
        public MicrostationResources MicrostationResources { get; }

        public ApplicationMicrostation(MicrostationResources microstationResources)
        {
            MicrostationResources = microstationResources;
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
                _application ??= MicrostationInstance.Instance();

                try
                {
                    var _ =_application.Version;
                }
                catch
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
            _application?.Quit();
            _application = null;
        }
    }
}
