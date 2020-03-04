using ChannelAdam.ServiceModel;
using ConvertingModels.Infrastructure.Implementations;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiMicrostation.Infrastructure.Implementations;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Implementations;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using GadzhiWord.Infrastructure.Implementations;
using GadzhiWord.Infrastructure.Interfaces;
using GadzhiWord.Models.Implementations;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Word.Implementations.ApplicationWordPartial;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
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
            container.RegisterSingleton<IApplicationConverting, ApplicationConverting>();
            container.RegisterSingleton<IConvertingService, ConvertingService>();             
            container.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>((unity) =>
                      ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint), new ContainerControlledLifetimeManager());

            container.RegisterType<IMessagingService, MessagingService>();
            container.RegisterType<ILoggerService, LoggerService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterType<IExecuteAndCatchErrors, ExecuteAndCatchErrors>();
            container.RegisterType<IConverterServerFilesDataFromDTO, ConverterServerFilesDataFromDTO>();
            container.RegisterType<IConverterServerFilesDataToDTO, ConverterServerFilesDataToDTO>();

            //microstation
            container.RegisterType<IConvertingFileMicrostation, ConvertingFileMicrostation>();
            container.RegisterSingleton<IMicrostationProject, MicrostationProject>();
            container.RegisterSingleton<IApplicationMicrostation, ApplicationMicrostation>();
            container.RegisterSingleton<IDesignFileMicrostation, DesignFileMicrostation>();
            container.RegisterType<IMessagingMicrostationService, MessagingMicrostationService>();
            container.RegisterType<ILoggerMicrostationService, LoggerMicrostationService>();
            container.RegisterType<IExecuteAndCatchErrorsMicrostation, ExecuteAndCatchErrorsMicrostation>();
            container.RegisterType<IFileSystemOperationsMicrostation, FileSystemOperationsMicrostation>();
            container.RegisterType<IPdfCreatorService, PdfCreatorService>();

            //word
            var wordContainer = container?.CreateChildContainer();
            wordContainer.RegisterSingleton<IWordProject, WordProject>();
            wordContainer.RegisterSingleton<IApplicationWord, ApplicationWord>();
            wordContainer.RegisterType<IMessagingService, MessagingWordService>();
            container.RegisterType<IConvertingFileWord, ConvertingFileWord>(
                new InjectionConstructor(wordContainer.Resolve<IApplicationWord>(),
                                         wordContainer.Resolve<IWordProject>(),
                                         wordContainer.Resolve<IMessagingService>()));
        }
    }
}
