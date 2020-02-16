using GadzhiMicrostation.Factory;
using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// Сервис работы с ошибками
        /// </summary>
        private readonly IExecuteAndCatchErrorsMicrostation _executeAndCatchErrorsMicrostation;

        /// <summary>
        /// Проверка состояния папок и файлов, архивация, сохранение
        /// </summary>
        private readonly IFileSystemOperationsMicrostation _fileSystemOperationsMicrostation;

        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        private readonly IErrorMessagingMicrostation _errorMessagingMicrostation;

        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        public ApplicationMicrostation(IExecuteAndCatchErrorsMicrostation executeAndCatchErrorsMicrostation,
                                       IFileSystemOperationsMicrostation fileSystemOperationsMicrostation,
                                       IErrorMessagingMicrostation errorMessagingMicrostation,
                                       IMicrostationProject microstationProject)
        {
            _executeAndCatchErrorsMicrostation = executeAndCatchErrorsMicrostation;
            _fileSystemOperationsMicrostation = fileSystemOperationsMicrostation;
            _errorMessagingMicrostation = errorMessagingMicrostation;
            _microstationProject = microstationProject;
        }

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application _applicationMicrostation;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application ApplicationMicrostationProperty
        {
            get
            {
                if (_applicationMicrostation == null)
                {
                    _executeAndCatchErrorsMicrostation.ExecuteAndHandleError(() => _applicationMicrostation = MicrostationInstance.Instance(),
                                                          errorMicrostation: new ErrorMicrostation(ErrorMicrostationType.ApplicationLoad,
                                                                                                   "Ошибка загрузки приложения Microstation"));
                }
                return _applicationMicrostation;
            }
        }

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => ApplicationMicrostationProperty != null;

        /// <summary>
        /// Текущий файл Microstation
        /// </summary>
        public IDesignFileMicrostation ActiveDesignFile =>
               new DesignFileMicrostation(ApplicationMicrostationProperty.ActiveDesignFile);

        /// <summary>
        /// Открыть файл
        /// </summary>
        public void OpenDesignFile(string filePath)
        {
            if (IsDesingFileValidAndSetErrors(filePath))
            {
                _executeAndCatchErrorsMicrostation.ExecuteAndHandleError(() => ApplicationMicrostationProperty.OpenDesignFile(filePath, false),
                                                      errorMicrostation: new ErrorMicrostation(ErrorMicrostationType.DesingFileOpen,
                                                                                               $"Ошибка открытия файла {filePath}"));
            }
          ;
        }

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private bool IsDesingFileValidAndSetErrors(string filePath)
        {
            bool isValid = false;

            if (_fileSystemOperationsMicrostation.IsFileExist(filePath))
            {
                string fileExtension = Path.GetExtension(filePath).Trim('.').ToLower();
                if (_microstationProject.ProjectMicrostationSettings.
                                         AllowedFileTypes?.Contains(fileExtension) == true)
                {
                    isValid = true;
                }
                else
                {
                    _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.IncorrectExtension,
                                                                          $"Расширение файла {filePath} не соответствует типу .dgn"));
                }
            }
            else
            {
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.FileNotFound,
                                                                           $"Файл {filePath} не найден"));
            }

            return isValid;
        }
    }
}
