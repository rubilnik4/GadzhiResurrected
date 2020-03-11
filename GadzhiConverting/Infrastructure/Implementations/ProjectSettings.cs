using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommonServer.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.Printers;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приолжения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ProjectSettings(IFileSystemOperations fileSystemOperations)
        {
            PutResourcesToDataFolder();
           // CreateSaveFolders();
        }

        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        public string ConvertingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                             "//Converting.gitignore";

        /// <summary>
        /// Папка с ресурсами и библиотеками
        /// </summary>
        public string DataResourcesFolder => AppDomain.CurrentDomain.BaseDirectory + "DataResources\\";

        /// <summary>
        /// Информация о установленных в системе принтерах
        /// </summary>
        public IPrintersInformation PrintersInformation => ConverterPrintingConfiguration.ToPrintersInformation();

        /// <summary>
        /// Время через которое осуществляется проверка пакетов на сервере
        /// </summary>
        public int IntervalSecondsToServer => 5;

        /// <summary>
        /// Время через которое осуществляется удаление ненужных пакетов на сервере
        /// </summary>
        public int IntervalHouresToDeleteUnusedPackages => 12;

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        public string NetworkName => Environment.UserDomainName + "\\" + Environment.MachineName;


        ///// <summary>
        ///// Создать путь для сохранения отконвертированных файлов
        ///// </summary>        
        //public string CreateFileSavePath(string fileName, FileExtention fileExtentionType)
        //{
        //    string fileFolderSave = FileExtensionToSaveFolder(fileExtentionType);

        //    if (!String.IsNullOrWhiteSpace(fileName) && !String.IsNullOrWhiteSpace(fileFolderSave))
        //    {
        //        return _fileSystemOperations.CombineFilePath(fileFolderSave, fileName,
        //                                                     fileExtentionType.ToString());
        //    }
        //    else
        //    {
        //        if (String.IsNullOrEmpty(fileName))
        //        {
        //            throw new ArgumentNullException(nameof(fileName));
        //        }
        //        if (String.IsNullOrEmpty(fileFolderSave))
        //        {
        //            throw new ArgumentException(nameof(fileFolderSave));
        //        }

        //        return String.Empty;
        //    }
        //}

        /// <summary>
        /// Скопировать ресурсы
        /// </summary>        
        private void PutResourcesToDataFolder()
        {
            _fileSystemOperations.CreateFolderByName(DataResourcesFolder);
            if (_fileSystemOperations.IsDirectoryExist(DataResourcesFolder))
            {
                Properties.Resources.signature.Save(Path.Combine(DataResourcesFolder, "signature.jpg"));
            }
        }

        /// <summary>
        /// Папка для сохранения файлов DOC
        /// </summary>
        public string DocFilesFolder(string FilePathServer) => Path.GetDirectoryName(FilePathServer) + "\\DOC";

        /// <summary>
        /// Папка для сохранения файлов DGN
        /// </summary>
        public string DgnFilesFolder(string FilePathServer) => Path.GetDirectoryName(FilePathServer) + "\\DGN";

        /// <summary>
        /// Папка для сохранения файлов PDF
        /// </summary>
        public string PdfFilesFolder(string FilePathServer) => Path.GetDirectoryName(FilePathServer) + "\\PDF";

        /// <summary>
        /// Папка для сохранения файлов XLS
        /// </summary>
        public string DwgFilesFolder(string FilePathServer) => Path.GetDirectoryName(FilePathServer) + "\\XLS";

        ///// <summary>
        ///// Создать пути для сохранения файлов
        ///// </summary>      
        //private void CreateSaveFolders()
        //{
        //    _fileSystemOperations.CreateFolderByName(DocFilesFolder);
        //    _fileSystemOperations.CreateFolderByName(PdfFilesFolder);
        //    _fileSystemOperations.CreateFolderByName(DwgFilesFolder);
        //}

        ///// <summary>
        ///// Папка для сохранения по типу фала
        ///// </summary>      
        //private string FileExtensionToSaveFolder(FileExtention fileExtentionType)
        //{
        //    string fileFolderSave = String.Empty;

        //    switch (fileExtentionType)
        //    {
        //        case FileExtention.docx:
        //            fileFolderSave = DocFilesFolder;
        //            break;
        //        case FileExtention.pdf:
        //            fileFolderSave = PdfFilesFolder;
        //            break;
        //        case FileExtention.xlsx:
        //            fileFolderSave = DwgFilesFolder;
        //            break;
        //    }

        //    return fileFolderSave;
        //}
    }
}
