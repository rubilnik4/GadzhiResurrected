using ChannelAdam.ServiceModel;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiMicrostation.Microstation.Implementations;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using MicrostationSignatures.Infrastructure.Implementations;
using MicrostationSignatures.Infrastructure.Interfaces;
using Unity;
using MicrostationSignatures.Models.Implementations;
using MicrostationSignatures.Models.Interfaces;

namespace MicrostationSignatures.DependencyInjection
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

            container.RegisterType<IProjectSignatureSettings, ProjectSignatureSettings>();
            container.RegisterType<ISignaturesToJpeg, SignaturesToJpeg>();

            container.RegisterType<IMessagingService, MessagingService>();
            container.RegisterType<ILoggerService, LoggerService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>((unity) => 
                    ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint));

            container.RegisterFactory<IApplicationMicrostation>(unity =>
                new ApplicationMicrostation(new MicrostationResources(ProjectSignatureSettings.SignatureMicrostationFileName,
                                                                      ProjectSignatureSettings.StampMicrostationFileName)));
        }
    }
}
