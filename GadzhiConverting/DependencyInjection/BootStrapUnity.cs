using ChannelAdam.ServiceModel;
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
using GadzhiMicrostation.Microstation.Implementations;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiWord.Word.Implementations;
using GadzhiWord.Word.Implementations.ApplicationWordPartial;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace GadzhiConverting.DependencyInjection.GadzhiConverting
{
    /// <summary>
    /// Класс для регистрации зависимостей
    /// </summary>
    public static class BootStrapUnity
    {
        /// <summary>
        /// Зарегистрировать зависимости
        /// </summary>
        public static void ConfigureContainer(IUnityContainer container)
        {
            var clientEndpoints = new ClientEndpoints();
            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingServerService));

            container.RegisterSingleton<IProjectSettings, ProjectSettings>();
            container.RegisterSingleton<IConvertingFileData, ConvertingFileData>();
            container.RegisterSingleton<IConvertingService, ConvertingService>();
            container.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>((unity) =>
                      ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint), new ContainerControlledLifetimeManager());

            container.RegisterType<IMessagingService, MessagingService>();
            container.RegisterType<ILoggerService, LoggerService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterType<IExecuteAndCatchErrors, ExecuteAndCatchErrors>();
            container.RegisterType<IConverterServerFilesDataFromDTO, ConverterServerFilesDataFromDTO>();
            container.RegisterType<IConverterServerFilesDataToDTO, ConverterServerFilesDataToDTO>();
            container.RegisterType<IPdfCreatorService, PdfCreatorService>();

            var projectSettings = container.Resolve<IProjectSettings>();
            container.RegisterFactory<IApplicationLibrary>(nameof(ApplicationMicrostation), unity =>
                      new ApplicationMicrostation(new MicrostationResources(projectSettings.ConvertingResources.SignatureMicrostationFileName,
                                                                            projectSettings.ConvertingResources.StampMicrostationFileName)));
            container.RegisterFactory<IApplicationLibrary>(nameof(ApplicationWord), unity =>
                      new ApplicationWord(new WordResources(projectSettings.ConvertingResources.SignatureWordFileName)));

            container.RegisterFactory<IApplicationConverting>(unity =>
                new ApplicationConverting(unity.Resolve<IApplicationLibrary>(nameof(ApplicationMicrostation)),
                                          unity.Resolve<IApplicationLibrary>(nameof(ApplicationWord)),
                                          unity.Resolve<IExecuteAndCatchErrors>(),
                                          unity.Resolve<IFileSystemOperations>(),
                                          unity.Resolve<IPdfCreatorService>()));

        }
    }
}
