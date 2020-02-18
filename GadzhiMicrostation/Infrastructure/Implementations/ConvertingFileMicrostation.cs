using GadzhiMicrostation.Infrastructure.Implementations.Converting;
using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
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

        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        private readonly IErrorMessagingMicrostation _errorMessagingMicrostation;

        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        public ConvertingFileMicrostation(IApplicationMicrostation applicationMicrostation,
                                          IErrorMessagingMicrostation errorMessagingMicrostation,
                                          IMicrostationProject microstationProject)
        {
            _applicationMicrostation = applicationMicrostation;
            _errorMessagingMicrostation = errorMessagingMicrostation;
            _microstationProject = microstationProject;
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
