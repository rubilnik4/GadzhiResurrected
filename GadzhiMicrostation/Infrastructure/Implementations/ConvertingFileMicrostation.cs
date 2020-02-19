using GadzhiMicrostation.DependencyInjection.BootStrapMicrostation;
using GadzhiMicrostation.Infrastructure.Implementations.Converting;
using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using Microsoft.Practices.Unity;
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
        /// Контейнер для инверсии зависимости
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        private readonly IApplicationMicrostation _applicationMicrostation;

        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        private readonly IErrorMessagingMicrostation _errorMessagingMicrostation;

        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        public ConvertingFileMicrostation()
        {
            _container = new UnityContainer();
            BootStrapUnityMicrostation.Start(_container);

            _applicationMicrostation = _container.Resolve<IApplicationMicrostation>();
            _errorMessagingMicrostation = _container.Resolve<IErrorMessagingMicrostation>();
            _microstationProject = _container.Resolve<IMicrostationProject>();
        }

        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        public void ConvertingFile(FileDataMicrostation fileDataMicrostation)
        {
            _microstationProject.SetInitialFileData(fileDataMicrostation);

            if (_applicationMicrostation.IsApplicationValid)
            {
                _applicationMicrostation.OpenDesignFile(_microstationProject.FileDataMicrostation.FilePathServer);
                _applicationMicrostation.SaveDesignFile(_microstationProject.CreateDngSavePath());

                var desingFile = _applicationMicrostation.ActiveDesignFile;
                if (desingFile.IsDesingFileValid)
                {
                    FindStampsInDesingFile(desingFile);
                }

                _applicationMicrostation.CloseDesignFile();
            }
        }

        /// <summary>
        /// Найти все доступные штампы во всех моделях и листах. Начать обработку каждого из них
        /// </summary>       
        private void FindStampsInDesingFile(IDesignFileMicrostation desingFile)
        {
            var stamps = desingFile.Stamps;
            if (stamps.Any())
            {
                foreach (var stamp in stamps)
                {
                    StampProcessing.ConvertingStamp(stamp);
                }
            }
            else
            {
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.StampNotFound,
                                                     $"Штапы в файле {_microstationProject.FileDataMicrostation.FileName} не найдены"));
            }
        }


    }
}
