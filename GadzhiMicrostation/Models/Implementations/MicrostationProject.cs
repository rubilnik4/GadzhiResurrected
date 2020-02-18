using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Модель хранения данных конвертации
    /// </summary>
    public class MicrostationProject : IMicrostationProject
    {
        /// <summary>
        /// Ошибки конвертации
        /// </summary>
        private List<ErrorMicrostation> _errorsMicrostation;

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        public IProjectMicrostationSettings ProjectMicrostationSettings { get; }

        /// <summary>
        /// Класс для хранения информации о конвертируемом файле типа DGN
        /// </summary>
        public FileDataMicrostation FileDataMicrostation { get; private set; }

        /// <summary>
        /// Проверка состояния папок и файлов, архивация, сохранение
        /// </summary>
        private readonly IFileSystemOperationsMicrostation _fileSystemOperationsMicrostation;

        public MicrostationProject(IProjectMicrostationSettings projectMicrostationSettings,
                                   IFileSystemOperationsMicrostation fileSystemOperationsMicrostation)
        {
            _errorsMicrostation = new List<ErrorMicrostation>();
            _fileSystemOperationsMicrostation = fileSystemOperationsMicrostation;

            ProjectMicrostationSettings = projectMicrostationSettings;
        }

        /// <summary>
        /// Создать путь для сохранения отконвертированного файла
        /// </summary>        
        public string CreateDngSavePath()
        {
            string fileFolderServer = Path.GetDirectoryName(FileDataMicrostation?.FilePathServer);
            string fileFolderSave = _fileSystemOperationsMicrostation.CreateFolderByName(fileFolderServer, "DGN");
            return _fileSystemOperationsMicrostation.CombineFilePath(fileFolderSave, 
                                                                     FileDataMicrostation?.FileName, 
                                                                     FileDataMicrostation?.FileExtension);
                      
        }

        /// <summary>
        /// Ошибки конвертации
        /// </summary>
        public IEnumerable<ErrorMicrostation> ErrorsMicrostation => _errorsMicrostation;

        /// <summary>
        /// Добавить ошибку
        /// </summary>   
        public void AddError(ErrorMicrostation errorMicrostation)
        {
            _errorsMicrostation.Add(errorMicrostation);
        }

        /// <summary>
        /// Записать исходные данные для конвертации
        /// </summary>      
        public void SetInitialFileData(FileDataMicrostation fileDataMicrostation)
        {
            FileDataMicrostation = fileDataMicrostation;
        }
    }
}
