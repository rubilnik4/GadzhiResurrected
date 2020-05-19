using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.Resources;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;
using GadzhiWord.Word.Implementations.ApplicationWordPartial;
using GadzhiWord.Word.Interfaces;
using Unity;
using static GadzhiConverting.Infrastructure.Implementations.Converters.SignaturesFunctionSync;

namespace GadzhiConverting.DependencyInjection
{
    /// <summary>
    /// Класс для регистрации зависимостей
    /// </summary>
    public static class BootStrapUnity
    {
        /// <summary>
        /// Зарегистрировать зависимости
        /// </summary>
        public static async Task ConfigureContainer(IUnityContainer container)
        {
            var clientEndpoints = new ClientEndpoints();
            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingServerService));

            container.RegisterSingleton<IProjectSettings, ProjectSettings>();
            container.RegisterSingleton<IConvertingFileData, ConvertingFileData>();
            container.RegisterSingleton<IConvertingService, ConvertingService>();
            container.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>((unity) =>
                      ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint), FactoryLifetime.Singleton);

            container.RegisterType<IMessagingService, MessagingService>();
            container.RegisterType<ILoggerService, LoggerService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterType<IConverterServerPackageDataFromDto, ConverterServerPackageDataFromDto>();
            container.RegisterType<IConverterServerPackageDataToDto, ConverterServerPackageDataToDto>();
            container.RegisterType<IConverterDataFileFromDto, ConverterDataFileFromDto>();
            container.RegisterType<IPdfCreatorService, PdfCreatorService>();

            await RegisterConvertingApplications(container);

            container.RegisterFactory<IApplicationConverting>(unity =>
                new ApplicationConverting(unity.Resolve<IApplicationLibrary<IDocumentMicrostation>>(nameof(ApplicationMicrostation)),
                                          unity.Resolve<IApplicationLibrary<IDocumentWord>>(nameof(ApplicationWord)),
                                          unity.Resolve<IFileSystemOperations>(),
                                          unity.Resolve<IPdfCreatorService>()));
        }

        /// <summary>
        /// Регистрация приложений Microstation и Word
        /// </summary>
        private static async Task RegisterConvertingApplications(IUnityContainer container)
        {
            var converterDataFileFromDto = container.Resolve<IConverterDataFileFromDto>();
            var projectSettings = container.Resolve<IProjectSettings>();
            var fileConvertingServerService = container.Resolve<IServiceConsumer<IFileConvertingServerService>>();

            var convertingResources = await projectSettings.ConvertingResources;
            var signaturesLibrarySearching = new SignaturesLibrarySearching(convertingResources.SignatureNames,
                                                                            GetSignaturesSync(fileConvertingServerService, 
                                                                                              converterDataFileFromDto, 
                                                                                              ProjectSettings.DataSignaturesFolder));

            container.RegisterFactory<IApplicationLibrary<IDocumentMicrostation>>(nameof(ApplicationMicrostation), unity =>
                new ApplicationMicrostation(new MicrostationResources(signaturesLibrarySearching, 
                                                                      convertingResources.SignaturesMicrostation.Value, 
                                                                      convertingResources.StampMicrostation.Value)));

            container.RegisterFactory<IApplicationLibrary<IDocumentWord>>(nameof(ApplicationWord), unity =>
                new ApplicationWord(new ResourcesWord(signaturesLibrarySearching)));
        }
    }
}
