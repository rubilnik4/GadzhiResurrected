using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiDTOServer.Contracts.FilesConvert;
using System.Linq;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiConverting.Infrastructure.Implementations.Services;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        /// <summary>
        /// Фабрика для создания подключения к WCF сервису подписей для сервера
        /// </summary>    
        private readonly SignatureServerServiceFactory _signatureServerServiceFactory;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        public ProjectSettings(SignatureServerServiceFactory signatureServerServiceFactory,
                               IFileSystemOperations fileSystemOperations, IMessagingService messagingService)
        {
            _signatureServerServiceFactory = signatureServerServiceFactory ?? throw new ArgumentNullException(nameof(signatureServerServiceFactory));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
        }

        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        public static string ConvertingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                                    "//Converting.gitignore";

        /// <summary>
        /// Папка с ресурсами и библиотеками
        /// </summary>
        public static string DataResourcesFolder => AppDomain.CurrentDomain.BaseDirectory + "DataResources";

        /// <summary>
        /// Папка для временного хранения подписей
        /// </summary>
        public static string DataSignaturesFolder => DataResourcesFolder + Path.DirectorySeparatorChar +
                                                     "Signatures" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Информация о установленных в системе принтерах
        /// </summary>
        private static IPrintersInformation _printersInformation;

        /// <summary>
        /// Информация о установленных в системе принтерах
        /// </summary>
        public static IPrintersInformation PrintersInformation => 
            _printersInformation ??= ConverterPrintingConfiguration.ToPrintersInformation();

        /// <summary>
        /// Время через которое осуществляется проверка пакетов на сервере
        /// </summary>
        public static int IntervalSecondsToServer => 5;

        /// <summary>
        /// Время (часы) через которое осуществляется удаление ненужных пакетов на сервере
        /// </summary>
        public static int IntervalHoursToDeleteUnusedPackages => 12;

        /// <summary>
        /// Время (дни) через которое осуществляется удаление ненужных пакетов с ошибками на сервере
        /// </summary>
        public static int IntervalHoursToDeleteUnusedErrorPackages => 14;

        /// <summary>
        /// Максимальное время ожидания между операциями
        /// </summary>
        public static int TimeOutMinutesOperation => 3;

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        public static string NetworkName => Environment.UserDomainName + Path.DirectorySeparatorChar +
                                            Environment.MachineName;

        /// <summary>
        /// Пути ресурсов модулей конвертации
        /// </summary>
        private ConvertingResources _convertingResources;

        /// <summary>
        /// Пути ресурсов модулей конвертации
        /// </summary>
        public ConvertingResources ConvertingResources => _convertingResources ??= GetConvertingResourcesLoaded();

        /// <summary>
        /// Загрузить стартовый набор ресурсов для начала конвертирования
        /// </summary>
        [Logger]
        public ConvertingResources GetConvertingResourcesLoaded()
        {
            _messagingService.ShowMessage("Обработка предварительных данных...");
            var convertingResources = GetConvertingResourcesLazy();

            var errors = convertingResources.SignaturesMicrostation.Errors.
                         Concat(convertingResources.StampMicrostation.Errors).ToList();
            if (errors.Count > 0)
            {
                _messagingService.ShowMessage("Обнаружены ошибки...");
                _messagingService.ShowAndLogErrors(errors);
            }

            _messagingService.ShowMessage("Обработка предварительных данных завершена...");
            return convertingResources;
        }

        /// <summary>
        /// Скопировать ресурсы и вернуть пути их расположения
        /// </summary>        
        private ConvertingResources GetConvertingResourcesLazy()
        {
            _fileSystemOperations.CreateFolderByName(DataResourcesFolder);
            _fileSystemOperations.CreateFolderByName(DataSignaturesFolder);

            string signatureMicrostationFileName = Path.Combine(DataResourcesFolder, "signatureMicrostation.cel");
            string stampMicrostationFileName = Path.Combine(DataResourcesFolder, "stampMicrostation.cel");

            return new ConvertingResources(signatureMicrostationFileName, stampMicrostationFileName,
                                           _signatureServerServiceFactory, _fileSystemOperations);
        }
    }
}
