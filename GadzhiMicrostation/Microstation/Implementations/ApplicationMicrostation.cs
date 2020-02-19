using GadzhiMicrostation.Factory;
using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using Microsoft.Practices.Unity;
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
        /// Контейнер для инверсии зависимости
        /// </summary>
        private readonly IUnityContainer _container;

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

        public ApplicationMicrostation(IUnityContainer container,
                                       IExecuteAndCatchErrorsMicrostation executeAndCatchErrorsMicrostation,
                                       IFileSystemOperationsMicrostation fileSystemOperationsMicrostation,
                                       IErrorMessagingMicrostation errorMessagingMicrostation,
                                       IMicrostationProject microstationProject)
        {
            _container = container;
            _executeAndCatchErrorsMicrostation = executeAndCatchErrorsMicrostation;
            _fileSystemOperationsMicrostation = fileSystemOperationsMicrostation;
            _errorMessagingMicrostation = errorMessagingMicrostation;
            _microstationProject = microstationProject;
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
                    _executeAndCatchErrorsMicrostation.ExecuteAndHandleError(() => _application = MicrostationInstance.Instance(),
                                                          errorMicrostation: new ErrorMicrostation(ErrorMicrostationType.ApplicationLoad,
                                                                                                   "Ошибка загрузки приложения Microstation"));
                }
                return _application;
            }
        }

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => Application != null;

        /// <summary>
        /// Текущий файл Microstation
        /// </summary>
        public IDesignFileMicrostation ActiveDesignFile =>
            _container.Resolve<IDesignFileMicrostation>(new ParameterOverride(typeof(DesignFile), _application.ActiveDesignFile));

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            _application.Quit();
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        public IDesignFileMicrostation OpenDesignFile(string filePath)
        {
            if (IsDesingFileValidAndSetErrors(filePath))
            {
                _executeAndCatchErrorsMicrostation.ExecuteAndHandleError(() => Application.OpenDesignFile(filePath, false),
                                                      errorMicrostation: new ErrorMicrostation(ErrorMicrostationType.DesingFileOpen,
                                                                                               $"Ошибка открытия файла {filePath}"));
            };
            return ActiveDesignFile;
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void SaveDesignFile(string filePath)
        {
            _executeAndCatchErrorsMicrostation.ExecuteAndHandleError(() => ActiveDesignFile.SaveAs(filePath),
                                                  errorMicrostation: new ErrorMicrostation(ErrorMicrostationType.DesingFileOpen,
                                                                                           $"Ошибка сохранения файла {filePath}"),
                                                  ApplicationCatchMethod: () => ActiveDesignFile.Close());
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public void CloseDesignFile()
        {
            _executeAndCatchErrorsMicrostation.ExecuteAndHandleError(() => ActiveDesignFile.CloseWithSaving(),
                                                  errorMicrostation: new ErrorMicrostation(ErrorMicrostationType.DesingFileOpen,
                                                                                           $"Ошибка закрытия файла {ActiveDesignFile.FullName}"),
                                                  ApplicationCatchMethod: () => ActiveDesignFile.Close());
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
