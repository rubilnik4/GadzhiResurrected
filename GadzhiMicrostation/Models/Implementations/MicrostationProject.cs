using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Interfaces;
using GadzhiMicrostation.Models.StampCollections;
using System;
using System.Collections.Generic;
using System.IO;

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
        private readonly List<ErrorMicrostation> _errorsMicrostation;

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        public IProjectMicrostationSettings ProjectMicrostationSettings { get; }

        /// <summary>
        /// Класс для хранения информации о конвертируемом файле типа DGN
        /// </summary>
        public FileDataMicrostation FileDataMicrostation { get; private set; }

        /// <summary>
        /// Список используемых принтеров
        /// </summary>
        public PrintersInformation PrintersInformation { get; private set; }

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
            PutResourcesToDataFolder();
        }

        /// <summary>
        /// Записать исходные данные для конвертации
        /// </summary>      
        public void SetInitialFileData(FileDataMicrostation fileDataMicrostation, PrintersInformation printersInformation)
        {
            if (fileDataMicrostation != null && printersInformation != null)
            {
                FileDataMicrostation = fileDataMicrostation;
                PrintersInformation = printersInformation;

                CreateSaveFolders();
            }
            else if (fileDataMicrostation == null)
            {
                throw new ArgumentNullException(nameof(fileDataMicrostation));
            }
            else if (printersInformation == null)
            {
                throw new ArgumentNullException(nameof(printersInformation));
            }
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
        /// Создать путь для сохранения отконвертированных файлов
        /// </summary>        
        public string CreateFileSavePath(string fileName, FileExtentionType fileExtentionType)
        {
            string fileFolderSave = FileExtensionToSaveFolder(fileExtentionType);

            if (!String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(fileFolderSave))
            {
                return _fileSystemOperationsMicrostation.CombineFilePath(fileFolderSave,
                                                                         fileName,
                                                                         fileExtentionType.ToString());
            }
            else
            {
                if (String.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName));
                }
                if (String.IsNullOrEmpty(fileFolderSave))
                {
                    throw new ArgumentException(nameof(fileFolderSave));
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Папка для сохранения по типу фала
        /// </summary>      
        private string FileExtensionToSaveFolder(FileExtentionType fileExtentionType)
        {
            string fileFolderSave = String.Empty;

            switch (fileExtentionType)
            {
                case FileExtentionType.dgn:
                    fileFolderSave = DgnFilesFolder;
                    break;  
                case FileExtentionType.pdf:
                    fileFolderSave = PdfFilesFolder;
                    break;
                case FileExtentionType.dwg:
                    fileFolderSave = DwgFilesFolder;
                    break;
            }

            return fileFolderSave;
        }

        /// <summary>
        /// Папка для сохранения файлов DGN
        /// </summary>
        private string DgnFilesFolder => Path.GetDirectoryName(FileDataMicrostation?.FilePathServer) + "\\DGN";
       
        /// <summary>
        /// Папка для сохранения файлов PDF
        /// </summary>
        private string PdfFilesFolder => Path.GetDirectoryName(FileDataMicrostation?.FilePathServer) + "\\PDF";

        /// <summary>
        /// Папка для сохранения файлов DWG
        /// </summary>
        private string DwgFilesFolder => Path.GetDirectoryName(FileDataMicrostation?.FilePathServer) + "\\DWG";       

        /// <summary>
        /// Создать пути для сохранения файлов
        /// </summary>      
        private void CreateSaveFolders()
        {
            _fileSystemOperationsMicrostation.CreateFolderByName(DgnFilesFolder);
            _fileSystemOperationsMicrostation.CreateFolderByName(PdfFilesFolder);
            _fileSystemOperationsMicrostation.CreateFolderByName(DwgFilesFolder);
        }

        /// <summary>
        /// Скопировать ресурсы
        /// </summary>        
        private void PutResourcesToDataFolder()
        {
            _fileSystemOperationsMicrostation.CreateFolderByName(StampAdditionalParameters.MicrostationDataFolder);
            if (_fileSystemOperationsMicrostation.IsDirectoryExist(StampAdditionalParameters.MicrostationDataFolder))
            {
                _fileSystemOperationsMicrostation.SaveFileFromByte(StampAdditionalParameters.SignatureLibraryPath,
                                                                   Properties.Resources.Signature);
                _fileSystemOperationsMicrostation.SaveFileFromByte(StampAdditionalParameters.StampLibraryPath,
                                                                 Properties.Resources.Stamp);

            }
        }
    }
}
