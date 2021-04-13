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
            PutSignatureTemplateToDataFolder();
        }

        /// <summary>
        /// Подписи Microstation
        /// </summary>
        public const string SIGNATURE_MICROSTATION_FILE = "SignatureMicrostation.cel";

        /// <summary>
        /// Подписи Microstation
        /// </summary>
        public const string STAMP_MICROSTATION_FILE = "StampMicrostation.cel";

        /// <summary>
        /// Подписи Microstation
        /// </summary>
        public const string SIGNATURE_TEMPLATE_FILE = "SignatureTemplate.dgn";

        /// <summary>
        /// Подписи для Microstation
        /// </summary>
        public string SignatureMicrostationFileName => DataResourcesFolder + SIGNATURE_MICROSTATION_FILE;

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        public string StampMicrostationFileName => DataResourcesFolder + STAMP_MICROSTATION_FILE;

        /// <summary>
        /// Путь к файлу шаблону для преобразования подписей
        /// </summary>
        public string SignatureTemplateFilePath => DataResourcesFolder + SIGNATURE_TEMPLATE_FILE;

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
        /// Размер изображения подписи в пикселях
        /// </summary>
        public static (int Width, int Height) JpegPixelSize => (500, 250);

        /// <summary>
        /// Скопировать ресурсы и вернуть пути их расположения
        /// </summary>        
        private void PutSignatureTemplateToDataFolder()
        {
            _fileSystemOperations.CreateFolderByName(SignaturesSaveFolder);
            _fileSystemOperations.CreateFolderByName(DataResourcesFolder);

            string templateFileName = Path.Combine(DataResourcesFolder, SIGNATURE_TEMPLATE_FILE);
            _fileSystemOperations.SaveFileFromByte(templateFileName, Properties.Resources.SignatureTemplate);

            string signatureFileName = Path.Combine(DataResourcesFolder, SIGNATURE_MICROSTATION_FILE);
            _fileSystemOperations.SaveFileFromByte(signatureFileName, Properties.Resources.SignatureMicrostation);

            string stampFileName = Path.Combine(DataResourcesFolder, STAMP_MICROSTATION_FILE);
            _fileSystemOperations.SaveFileFromByte(stampFileName, Properties.Resources.StampMicrostation);
        }
    }
}