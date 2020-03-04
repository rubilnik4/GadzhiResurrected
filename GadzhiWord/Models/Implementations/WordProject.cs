using ConvertingModels.Models.Implementations.FilesConvert;
using ConvertingModels.Models.Interfaces.FilesConvert;
using ConvertingModels.Models.Interfaces.Printers;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiWord.Models.Implementations.FilesConvert;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations
{
    /// <summary>
    /// Модель хранения данных конвертации Word
    /// </summary>
    public class WordProject : IWordProject
    {
        /// <summary>
        /// Класс для хранения информации о конвертируемом файле типа DGN
        /// </summary>
        public IFileDataServerWord FileDataServerWord { get; private set; }

        /// <summary>
        /// Список используемых принтеров
        /// </summary>
        public IPrintersInformation PrintersInformation { get; private set; }

        /// <summary>
        /// Проверка состояния папок и файлов, архивация, сохранение
        /// </summary>
        private readonly IFileSystemOperations _fileSystemOperations;

        public WordProject(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Записать исходные данные для конвертации
        /// </summary>      
        public void SetInitialFileData(IFileDataServer fileDataServer, IPrintersInformation printersInformation)
        {
            if (fileDataServer != null && printersInformation != null)
            {
                FileDataServerWord = new FileDataServerWord(fileDataServer.FilePathServer, fileDataServer.FilePathClient, 
                                                            fileDataServer.ColorPrint);
                PrintersInformation = printersInformation;

                CreateSaveFolders();
            }
            else if (fileDataServer == null)
            {
                throw new ArgumentNullException(nameof(fileDataServer));
            }
            else if (printersInformation == null)
            {
                throw new ArgumentNullException(nameof(printersInformation));
            }
        }

        /// <summary>
        /// Создать путь для сохранения отконвертированных файлов
        /// </summary>        
        public string CreateFileSavePath(string fileName, FileExtention fileExtentionType)
        {
            string fileFolderSave = FileExtensionToSaveFolder(fileExtentionType);

            if (!String.IsNullOrWhiteSpace(fileName) && !String.IsNullOrWhiteSpace(fileFolderSave))
            {
                return _fileSystemOperations.CombineFilePath(fileFolderSave, fileName,
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
        private string FileExtensionToSaveFolder(FileExtention fileExtentionType)
        {
            string fileFolderSave = String.Empty;

            switch (fileExtentionType)
            {
                case FileExtention.docx:
                    fileFolderSave = DocFilesFolder;
                    break;
                case FileExtention.pdf:
                    fileFolderSave = PdfFilesFolder;
                    break;
                case FileExtention.xlsx:
                    fileFolderSave = DwgFilesFolder;
                    break;
            }

            return fileFolderSave;
        }

        /// <summary>
        /// Папка для сохранения файлов DGN
        /// </summary>
        private string DocFilesFolder => Path.GetDirectoryName(FileDataServerWord?.FilePathServer) + "\\DOC";

        /// <summary>
        /// Папка для сохранения файлов PDF
        /// </summary>
        private string PdfFilesFolder => Path.GetDirectoryName(FileDataServerWord?.FilePathServer) + "\\PDF";

        /// <summary>
        /// Папка для сохранения файлов DWG
        /// </summary>
        private string DwgFilesFolder => Path.GetDirectoryName(FileDataServerWord?.FilePathServer) + "\\XLS";

        /// <summary>
        /// Создать пути для сохранения файлов
        /// </summary>      
        private void CreateSaveFolders()
        {
            _fileSystemOperations.CreateFolderByName(DocFilesFolder);
            _fileSystemOperations.CreateFolderByName(PdfFilesFolder);
            _fileSystemOperations.CreateFolderByName(DwgFilesFolder);
        }
    }
}
