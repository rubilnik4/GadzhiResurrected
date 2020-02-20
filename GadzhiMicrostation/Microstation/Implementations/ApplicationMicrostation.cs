using GadzhiMicrostation.Factory;
using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using GadzhiMicrostation.Models.StampCollections;
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

        #region DesingFile
        /// <summary>
        /// Текущий файл Microstation
        /// </summary>
        public IDesignFileMicrostation ActiveDesignFile =>
            new DesignFileMicrostation(_application.ActiveDesignFile, this);


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
        #endregion

        #region Commands

        /// <summary>
        /// Создать ячейку на освнове шаблона в библиотеке
        /// </summary>       
        public void CreateCellElementFromLibrary(string cellName,
                                                  PointMicrostation origin)
        {
            _application.CreateCellElement2(cellName,
                                            _application.Point3dFromXY(origin.X, origin.Y),
                                            _application.Point3dFromXY(1, 1),
                                            false,
                                            _application.Matrix3dIdentity());
        }

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public void CreateSignatureFromLibrary(string cellName,
                                               PointMicrostation origin)
        {
            AttachLibrary(StampAdditionalParameters.SignatureLibraryPath);
            CreateCellElementFromLibrary(cellName, origin);
            DetachLibrary();
        }

        /// <summary>
        /// Подключить библиотеку
        /// </summary>      
        private void AttachLibrary(string libraryPath)
        {
            if (_fileSystemOperationsMicrostation.IsFileExist(libraryPath))
            {
                _application.CadInputQueue.SendCommand("ATTACH LIBRARY " + libraryPath);
            }
            else
            {
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.FileNotFound,
                                                                        $"Файл библиотеки {libraryPath} не найден"));
            }
        }

        /// <summary>
        /// Отключить библиотеку
        /// </summary>      
        private void DetachLibrary()
        {
            _application.CadInputQueue.SendCommand("DETACH LIBRARY ");
        }
        #endregion
    }
}
