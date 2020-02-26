using GadzhiMicrostation.DependencyInjection.BootStrapMicrostation;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Обработка и конвертирование файла DGN
    /// </summary>
    public sealed class ConvertingFileMicrostation : IConvertingFileMicrostation
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
        public void ConvertingFile(FileDataMicrostation fileDataMicrostation, PrintersInformationMicrostation printersInformation)
        {
            _microstationProject.SetInitialFileData(fileDataMicrostation, printersInformation);

            if (_applicationMicrostation.IsApplicationValid)
            {
                _applicationMicrostation.OpenDesignFile(_microstationProject.FileDataMicrostation.FilePathServer);
                _applicationMicrostation.SaveDesignFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
                                                                                                FileExtentionType.dgn));

                var desingFile = _applicationMicrostation.ActiveDesignFile;
                if (desingFile.IsDesingFileValid)
                {
                    CreatePdfInDesingFIle(desingFile);
                }

                desingFile.CreateDWG();

                _applicationMicrostation.CloseDesignFile();
            }
        }

        /// <summary>
        /// Найти все доступные штампы во всех моделях и листах. Начать обработку каждого из них
        /// </summary>       
        private void CreatePdfInDesingFIle(IDesignFileMicrostation desingFile)
        {
            var stamps = desingFile.Stamps;
            if (stamps.Any())
            {
                foreach (var stamp in stamps)
                {
                    stamp.CompressFieldsRanges();

                    stamp.DeleteSignaturesPrevious();
                    stamp.InsertSignatures();

                    desingFile.CreatePdfByStamp(stamp);

                    stamp.DeleteSignaturesInserted();
                }
            }
            else
            {
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.StampNotFound,
                                                     $"Штапы в файле {_microstationProject.FileDataMicrostation.FileName} не найдены"));
            }
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
