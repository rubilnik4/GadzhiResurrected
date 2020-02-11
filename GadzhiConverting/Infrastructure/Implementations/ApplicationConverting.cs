using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Инфраструктура для конвертирования файлов
    /// </summary>
    public class ApplicationConverting : IApplicationConverting
    {
        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Основная модель состояния процесса конвертирования
        /// </summary>
        private readonly IConvertingProject _convertingProject;

        public ApplicationConverting(IProjectSettings projectSettings,
                                     IFileSystemOperations fileSystemOperations,
                                     IConvertingProject convertingProject)
        {
            _projectSettings = projectSettings;
            _fileSystemOperations = fileSystemOperations;
            _convertingProject = convertingProject;
        }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        public void StartConverting()
        {
            bool isValidStartUpdaParameters = ValidateStartupParameters();
            if (isValidStartUpdaParameters)
            {

            }          
        }

        /// <summary>
        /// Проверить параметры запуска, добавить ошибки в модель
        /// </summary>
        private bool ValidateStartupParameters()
        {
            bool isDataBaseExist = _fileSystemOperations.IsFileExist(_projectSettings.SQLiteDataBasePath);
            if (!isDataBaseExist)
            {
                var errorTypeConverting = new ErrorTypeConverting(FileConvertErrorType.FileNotFound,
                                                                  $"Файл базы данных {_projectSettings.SQLiteDataBasePath} не найден");
                _convertingProject.AddError(errorTypeConverting);
            }

            return isDataBaseExist;
        }
    }
}
