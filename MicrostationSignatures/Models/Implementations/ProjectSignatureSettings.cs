using System;
using System.IO;
using GadzhiCommon.Infrastructure.Interfaces;
using MicrostationSignatures.Models.Interfaces;

namespace MicrostationSignatures.Models.Implementations
{
    /// <summary>
    /// Параметры и установки
    /// </summary>
    public class ProjectSignatureSettings: IProjectSignatureSettings
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ProjectSignatureSettings(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }
        /// <summary>
        /// Папка с файловыми данными для конвертации
        /// </summary>
        public static string ConvertingResourcesFolder => new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).
                                                          Parent?.Parent?.Parent?.FullName + Path.DirectorySeparatorChar +
                                                          "GadzhiConverting" + Path.DirectorySeparatorChar +
                                                          "Resources" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Подписи для Microstation
        /// </summary>
        public static string SignatureMicrostationFileName => ConvertingResourcesFolder + "SignatureMicrostation.cel";

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        public static string StampMicrostationFileName => ConvertingResourcesFolder + "StampMicrostation.cel";

        /// <summary>
        /// Папка для сохранения подписей
        /// </summary>
        public static string SignaturesSaveFolder => AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar +
                                                     "Signatures";

        /// <summary>
        /// Папка с ресурсами и библиотеками
        /// </summary>
        public string DataResourcesFolder => AppDomain.CurrentDomain.BaseDirectory + "DataResources" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Путь к файлу шаблону для преобразования подписей
        /// </summary>
        private string _signatureTemplateFilePath;

        /// <summary>
        /// Путь к файлу шаблону для преобразования подписей
        /// </summary>
        public string SignatureTemplateFilePath => _signatureTemplateFilePath ??= PutSignatureTemplateToDataFolder();

        /// <summary>
        /// Размер изображения подписи в пикселях
        /// </summary>
        public static (int Width, int Height) JpegPixelSize => (640, 400);

        /// <summary>
        /// Скопировать ресурсы и вернуть пути их расположения
        /// </summary>        
        private string PutSignatureTemplateToDataFolder()
        {
            _fileSystemOperations.CreateFolderByName(SignaturesSaveFolder);

            _fileSystemOperations.CreateFolderByName(DataResourcesFolder);

            string templateFileName = Path.Combine(DataResourcesFolder, "SignatureTemplate.dgn");
            if (!_fileSystemOperations.IsFileExist(templateFileName))
            {
                _fileSystemOperations.SaveFileFromByte(templateFileName, Properties.Resources.SignatureTemplate);
            }

            return templateFileName;
        }
    }
}