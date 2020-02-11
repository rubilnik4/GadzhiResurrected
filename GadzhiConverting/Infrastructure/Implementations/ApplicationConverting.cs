using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
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

        public ApplicationConverting(IProjectSettings projectSettings,
                                     IFileSystemOperations fileSystemOperations)
        {
            _projectSettings = projectSettings;
            _fileSystemOperations = fileSystemOperations;
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
            else
            {

            }
        }

        /// <summary>
        /// Проверить параметры запуска
        /// </summary>
        private bool ValidateStartupParameters()
        {
            bool isDataBaseExist = _fileSystemOperations.IsFileExist(_projectSettings.SQLiteDataBasePath);

            return isDataBaseExist;
        }
    }
}
