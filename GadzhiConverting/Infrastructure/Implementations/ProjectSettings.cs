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
using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;
using GadzhiDTOBase.Infrastructure.Interfaces.Converters;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        public ProjectSettings(SignatureServerServiceFactory signatureServerServiceFactory,
                               IFileSystemOperations fileSystemOperations, IConverterDataFileFromDto converterDataFileFromDto,
                               IMessagingService messagingService)
        {
            _signatureServerServiceFactory = signatureServerServiceFactory ?? throw new ArgumentNullException(nameof(signatureServerServiceFactory));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _converterDataFileFromDto = converterDataFileFromDto;
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
        }

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису подписей для сервера
        /// </summary>    
        private readonly SignatureServerServiceFactory _signatureServerServiceFactory;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Преобразование подписи в трансферную модель
        /// </summary>
        private readonly IConverterDataFileFromDto _converterDataFileFromDto;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        public static string ConvertingDirectory => 
            ConfigurationManager.AppSettings.Get("ConvertingDirectory") 
            ?? throw new ArgumentNullException(nameof(ConvertingDirectory));

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
        /// Время (часы) через которое осуществляется проверка подписей
        /// </summary>
        public static int IntervalSignatureUpdate => 12;

        /// <summary>
        /// Время (дни) через которое осуществляется удаление ненужных пакетов с ошибками на сервере
        /// </summary>
        public static int IntervalHoursToDeleteUnusedErrorPackages => 14;

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
        public ConvertingResources ConvertingResources =>
            _convertingResources ??= GetConvertingResourcesLoaded();

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
                                           _signatureServerServiceFactory, _converterDataFileFromDto, _fileSystemOperations);
        }
    }
}
