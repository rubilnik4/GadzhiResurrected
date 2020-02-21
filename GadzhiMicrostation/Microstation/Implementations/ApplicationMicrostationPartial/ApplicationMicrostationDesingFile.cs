using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    public partial class ApplicationMicrostation: IApplicationMicrostationCommands
    {

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
                string fileExtension = Path.GetExtension(filePath).Trim('.').ToLower(CultureInfo.CurrentCulture);
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
