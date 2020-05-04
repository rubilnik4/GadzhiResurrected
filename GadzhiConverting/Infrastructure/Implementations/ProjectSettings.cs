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
    /// Параметры приложения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ProjectSettings(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;

            ConvertingResources = PutResourcesToDataFolder();
        }

        /// <summary>
        /// Пути ресурсов модулей конвертации
        /// </summary>
        public ConvertingResources ConvertingResources { get; }

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
        public int IntervalHoursToDeleteUnusedPackages => 12;

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        public string NetworkName => Environment.UserDomainName + "\\" + Environment.MachineName;

        /// <summary>
        /// Скопировать ресурсы и вернуть пути их расположения
        /// </summary>        
        private ConvertingResources PutResourcesToDataFolder()
        {
            _fileSystemOperations.CreateFolderByName(DataResourcesFolder);

            string signatureWordFileName = Path.Combine(DataResourcesFolder, "signatureWord.jpg");
            if (!_fileSystemOperations.IsFileExist(signatureWordFileName))
            {
                Properties.Resources.SignatureWord.Save(signatureWordFileName);
            }

            string signatureMicrostationFileName = Path.Combine(DataResourcesFolder, "signatureMicrostation.cel");
            if (!_fileSystemOperations.IsFileExist(signatureMicrostationFileName))
            {
                _fileSystemOperations.SaveFileFromByte(signatureMicrostationFileName, Properties.Resources.SignatureMicrostation);
            }

            string stampMicrostationFileName = Path.Combine(DataResourcesFolder, "stampMicrostation.cel");
            if (!_fileSystemOperations.IsFileExist(stampMicrostationFileName))
            {
                _fileSystemOperations.SaveFileFromByte(stampMicrostationFileName, Properties.Resources.StampMicrostation);
            }

            return new ConvertingResources(signatureWordFileName, signatureMicrostationFileName, stampMicrostationFileName);
        }
    }
}
