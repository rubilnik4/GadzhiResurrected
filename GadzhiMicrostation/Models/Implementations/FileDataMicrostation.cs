using GadzhiMicrostation.Models.Enum;
using System.IO;


namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле типа DGN
    /// </summary>
    public class FileDataMicrostation
    {
        public FileDataMicrostation(string filePathServer,
                                    string filePathClient,
                                    ColorPrint colorPrint)
        {
            string fileType = Path.GetExtension(filePathServer).Trim('.');
            string fileName = Path.GetFileNameWithoutExtension(filePathServer);

            FileExtension = fileType;
            FileName = fileName;
            FilePathServer = filePathServer;
            FilePathClient = filePathClient;
            ColorPrint = colorPrint;
        }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileExtension { get; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Путь файла на сервере
        /// </summary>
        public string FilePathServer { get; }

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        public string FileNameWithExtensionClient => Path.GetFileName(FilePathClient);

        /// <summary>
        /// Путь файла на клиенте
        /// </summary>
        public string FilePathClient { get; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        public ColorPrint ColorPrint { get; }
    }
}
