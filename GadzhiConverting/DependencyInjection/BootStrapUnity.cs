using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiApplicationCommon.Infrastructure.Implementations;
using GadzhiApplicationCommon.Infrastructure.Interfaces;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.Resources;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Implementations.Services;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Infrastructure.Interfaces.Services;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOBase.Infrastructure.Interfaces.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;
using GadzhiWord.Word.Interfaces.Word;
using Unity;
using Unity.Lifetime;
using static GadzhiConverting.Infrastructure.Implementations.Converters.SignaturesFunctionSync;
using ApplicationOffice = GadzhiWord.Word.Implementations.ApplicationOfficePartial.ApplicationOffice;

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
        [Logger]
        public static void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterSingleton<IProjectSettings, ProjectSettings>();
            container.RegisterSingleton<IConvertingFileData, ConvertingFileData>();
            container.RegisterSingleton<IConvertingService, ConvertingService>();

            container.RegisterType<IMessagingService, MessagingService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterType<ISignatureConverter, SignatureConverter>();
            container.RegisterType<IConverterServerPackageDataFromDto, ConverterServerPackageDataFromDto>();
            container.RegisterType<IConverterServerPackageDataToDto, ConverterServerPackageDataToDto>();
            container.RegisterType<IConverterDataFileFromDto, ConverterDataFileFromDto>();
            container.RegisterType<IPdfCreatorService, PdfCreatorService>();

            RegisterServices(container);
            RegisterConvertingApplications(container);

            container.RegisterFactory<IApplicationConverting>(unity =>
                new ApplicationConverting(unity.Resolve<IApplicationLibrary<IDocumentMicrostation>>(nameof(ApplicationMicrostation)),
                                          unity.Resolve<IApplicationLibrary<IDocumentWord>>(nameof(ApplicationOffice)),
                                          unity.Resolve<IFileSystemOperations>(),
                                          unity.Resolve<IPdfCreatorService>()));
        }

        /// <summary>
        /// Регистрация WCF сервисов
        /// </summary>
        public static void RegisterServices(IUnityContainer unityContainer)
        {

            var clientEndpoints = new ClientEndpoints();

            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingServerService));
            unityContainer.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>(unity =>
                ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint));

            string signatureEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(ISignatureServerService));
            unityContainer.RegisterFactory<IServiceConsumer<ISignatureServerService>>(unity =>
                ServiceConsumerFactory.Create<ISignatureServerService>(signatureEndpoint));

            unityContainer.RegisterFactory<IWcfServerServicesFactory>(unity =>
                new WcfServerServicesFactory(() => unity.Resolve<IServiceConsumer<IFileConvertingServerService>>(),
                                             () => unity.Resolve<IServiceConsumer<ISignatureServerService>>()), 
                                                                      new SingletonLifetimeManager());
        }

        /// <summary>
        /// Регистрация приложений Microstation и Word
        /// </summary>
        [Logger]
        private static void RegisterConvertingApplications(IUnityContainer container)
        {
            var signatureConverter = container.Resolve<ISignatureConverter>();
            var projectSettings = container.Resolve<IProjectSettings>();
            var wcfServerServicesFactory = container.Resolve<IWcfServerServicesFactory>();

            var convertingResources = projectSettings.ConvertingResources;
            var signaturesLibrarySearching = new SignaturesSearching(convertingResources.SignatureNames.Value.ToApplication(),
                                                                     GetSignaturesSync(wcfServerServicesFactory.SignatureServerServiceFactory, 
                                                                                       signatureConverter, ProjectSettings.DataSignaturesFolder));

            container.RegisterFactory<IApplicationLibrary<IDocumentMicrostation>>(nameof(ApplicationMicrostation), unity =>
                new ApplicationMicrostation(new MicrostationResources(signaturesLibrarySearching, 
                                                                      convertingResources.SignaturesMicrostation.Value, 
                                                                      convertingResources.StampMicrostation.Value)));

            container.RegisterFactory<IApplicationLibrary<IDocumentWord>>(nameof(ApplicationOffice), unity =>
                new ApplicationOffice(new ResourcesWord(signaturesLibrarySearching)));
        }
    }
}
