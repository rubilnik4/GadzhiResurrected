using GadzhiMicrostation.DependencyInjection.BootStrapMicrostation;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Implementations.FilesData;
using GadzhiMicrostation.Models.Implementations.Printers;
using GadzhiMicrostation.Models.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Обработка и конвертирование файла DGN
    /// </summary>
    public class ConvertingFileMicrostation : IConvertingFileMicrostation
    {
        /// <summary>
        /// Контейнер для инверсии зависимости
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        private readonly IApplicationMicrostation _applicationMicrostation;      

        /// <summary>
        /// Отображение системных сообщений
        /// </summary>
        private readonly ILoggerMicrostation _loggerMicrostation;

        /// <summary>
        /// Модель хранения данных конвертации Microstation
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        public ConvertingFileMicrostation()
        {
            _container = new UnityContainer();
            BootStrapUnityMicrostation.ConfigureContainer(_container);

            _applicationMicrostation = _container.Resolve<IApplicationMicrostation>();         
            _loggerMicrostation = _container.Resolve<ILoggerMicrostation>();
            _microstationProject = _container.Resolve<IMicrostationProject>();
        }

        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        public FileDataMicrostation ConvertingFile(FileDataMicrostation fileDataMicrostation, PrintersInformationMicrostation printersInformation)
        {
            _microstationProject.SetInitialFileData(fileDataMicrostation, printersInformation);

            if (_applicationMicrostation.IsApplicationValid)
            {
                _loggerMicrostation.ShowMessage("Загрузка файла Microstation");
                _applicationMicrostation.OpenDesignFile(_microstationProject.FileDataMicrostation.FilePathServer);
                _applicationMicrostation.SaveDesignFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                                                                                                FileExtentionMicrostation.dgn));

                _loggerMicrostation.ShowMessage("Создание файлов PDF");
                _applicationMicrostation.CreatePdfFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                                                                                            FileExtentionMicrostation.pdf));

                _loggerMicrostation.ShowMessage("Создание файла DWG");
                _applicationMicrostation.CreateDwgFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                                                                                                FileExtentionMicrostation.dwg));

                _applicationMicrostation.CloseDesignFile();
            }

            return _microstationProject.FileDataMicrostation;
        }

        /// <summary>
        /// Освободить элементы
        /// </summary>      
        public void Dispose()
        {
            _container?.Dispose();
            _applicationMicrostation?.Dispose();
        }

    }
}
