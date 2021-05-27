using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Коллекция путей файлов
    /// </summary>
    public class FilePathCollection
    {
        public FilePathCollection(IFilePath filePathMain, IFilePath filePathPdf, IFilePath filePathPrint)
        {
            FilePathMain = filePathMain;
            FilePathPdf = filePathPdf;
            FilePathPrint = filePathPrint;
        }

        /// <summary>
        /// Путь основного файла
        /// </summary>
        public IFilePath FilePathMain { get; }

        /// <summary>
        /// Путь PDF файла
        /// </summary>
        public IFilePath FilePathPdf { get; }

        /// <summary>
        /// Путь печати файла
        /// </summary>
        public IFilePath FilePathPrint { get; }
    }
}