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
        /// Класс для работы с приложением Microstation
        /// </summary>
        private readonly IApplicationMicrostation _applicationMicrostation;        

        /// <summary>
        /// Модель хранения данных конвертации Microstation
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingMicrostationService _messagingMicrostation;

        public ConvertingFileMicrostation(IApplicationMicrostation applicationMicrostation,
                                          IMicrostationProject microstationProject,
                                          IMessagingMicrostationService messagingMicrostation)
        {            
            _applicationMicrostation = applicationMicrostation;            
            _microstationProject = microstationProject;
            _messagingMicrostation = messagingMicrostation;
        }

        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        public FileDataMicrostation ConvertingFile(FileDataMicrostation fileDataMicrostation, PrintersInformationMicrostation printersInformation)
        {
            _microstationProject.SetInitialFileData(fileDataMicrostation, printersInformation);

            if (_applicationMicrostation.IsApplicationValid)
            {
                _messagingMicrostation.ShowAndLogMessage("Загрузка файла Microstation");
                _applicationMicrostation.OpenDesignFile(_microstationProject.FileDataMicrostation.FilePathServer);
                _applicationMicrostation.SaveDesignFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                                                                                                FileExtentionMicrostation.dgn));

                _messagingMicrostation.ShowAndLogMessage("Создание файлов PDF");
                _applicationMicrostation.CreatePdfFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                                                                                            FileExtentionMicrostation.pdf));

                _messagingMicrostation.ShowAndLogMessage("Создание файла DWG");
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
            _applicationMicrostation?.Dispose();
        }

    }
}
