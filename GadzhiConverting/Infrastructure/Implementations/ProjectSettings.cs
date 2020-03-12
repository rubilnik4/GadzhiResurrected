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
            _fileSystemOperations = fileSystemOperations;

            PutResourcesToDataFolder();
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
    }
}
