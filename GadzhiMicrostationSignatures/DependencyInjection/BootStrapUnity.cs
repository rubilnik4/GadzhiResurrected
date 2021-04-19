using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.Resources;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostationSignatures.Infrastructure.Implementations;
using GadzhiMicrostationSignatures.Infrastructure.Interfaces;
using Unity;
using GadzhiMicrostationSignatures.Models.Implementations;
using GadzhiMicrostationSignatures.Models.Interfaces;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiConvertingLibrary.Infrastructure.Implementations;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Converters;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Converters;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOBase.Infrastructure.Interfaces.Converters;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Services;

namespace GadzhiMicrostationSignatures.DependencyInjection
{
    /// <summary>
    /// Класс для регистрации зависимостей
    /// </summary>
    public static class BootStrapUnity
    {
        /// <summary>
        /// Зарегистрировать зависимости
        /// </summary>
        public static void ConfigureContainer(IUnityContainer container, string signatureFolder)
        {
            container.RegisterSingleton<IProjectSignatureSettings, ProjectSignatureSettings>();
            container.RegisterType<ISignaturesToJpeg, SignaturesUpload>();
            container.RegisterType<IConverterDataFileFromDto, ConverterDataFileFromDto>();
            container.RegisterType<ISignatureConverter, SignatureConverter>();

            container.RegisterType<IMessagingService, MessagingService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();

            GadzhiConvertingLibrary.DependencyInjection.BootStrapUnity.RegisterServices(container);

            var wcfServerServicesFactory = container.Resolve<IWcfServerServicesFactory>();
            var signatureConverter = container.Resolve<ISignatureConverter>();
            var signaturesLibrarySearching = new SignaturesSearching(Enumerable.Empty<ISignatureLibraryApp>(),
                                                                     SignaturesFunctionSync.GetSignaturesSync(wcfServerServicesFactory.SignatureServerServiceFactory, 
                                                                                                              signatureConverter, signatureFolder));
            container.RegisterFactory<IApplicationMicrostation>(unity =>
                {
                    var projectSignatureSettings = unity.Resolve<IProjectSignatureSettings>();
                    return new ApplicationMicrostation(new MicrostationResources(signaturesLibrarySearching,
                                                                projectSignatureSettings.SignatureMicrostationFileName,
                                                                projectSignatureSettings.StampMicrostationFileName));
                });
        }
    }
}
