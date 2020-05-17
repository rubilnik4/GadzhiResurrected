﻿using GadzhiCommon.Infrastructure.Interfaces;
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

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части, обработки подписей
        /// </summary>     
        private readonly IServiceConsumer<IFileConvertingServerService> _fileConvertingServerService;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        public ProjectSettings(IServiceConsumer<IFileConvertingServerService> fileConvertingServerService,
                               IFileSystemOperations fileSystemOperations, IMessagingService messagingService)
        {
            _fileConvertingServerService = fileConvertingServerService ?? throw new ArgumentNullException(nameof(fileConvertingServerService));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
        }

        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        public string ConvertingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                             "//Converting.gitignore";

        /// <summary>
        /// Папка с ресурсами и библиотеками
        /// </summary>
        public string DataResourcesFolder => AppDomain.CurrentDomain.BaseDirectory + "DataResources" + Path.DirectorySeparatorChar;

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
        /// Пути ресурсов модулей конвертации
        /// </summary>
        private Task<ConvertingResources> _convertingResources;

        /// <summary>
        /// Пути ресурсов модулей конвертации
        /// </summary>
        public Task<ConvertingResources> ConvertingResources => _convertingResources ??= GetConvertingResourcesLoaded();

        /// <summary>
        /// Загрузить стартовый набор ресурсов для начала конвертирования
        /// </summary>
        public async Task<ConvertingResources> GetConvertingResourcesLoaded()
        {
            var convertingResources = GetConvertingResourcesLazy();

            _messagingService.ShowAndLogMessage("Обработка предварительных данных...");
            await convertingResources.LoadData();
            _messagingService.ShowAndLogMessage("Загрузка подписей и штампов завершена...");

            var errors = convertingResources.SignaturesMicrostation.Errors.
                                             Concat(convertingResources.StampMicrostation.Errors).ToList();
            if (errors.Count > 0)
            {
                _messagingService.ShowAndLogMessage("Обнаружены ошибки...");
                _messagingService.ShowAndLogErrors(errors);
            }

            _messagingService.ShowAndLogMessage("Обработка предварительных данных завершена...");
            return convertingResources;
        }

        /// <summary>
        /// Скопировать ресурсы и вернуть пути их расположения
        /// </summary>        
        private ConvertingResources GetConvertingResourcesLazy()
        {
            _fileSystemOperations.CreateFolderByName(DataResourcesFolder);
            string signatureMicrostationFileName = Path.Combine(DataResourcesFolder, "signatureMicrostation.cel");
            string stampMicrostationFileName = Path.Combine(DataResourcesFolder, "stampMicrostation.cel");

            return new ConvertingResources(signatureMicrostationFileName, stampMicrostationFileName,
                                           _fileConvertingServerService, _fileSystemOperations);
        }
    }
}
