﻿using System.Collections;
using System.Linq;
using ChannelAdam.ServiceModel;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.Resources;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using MicrostationSignatures.Infrastructure.Implementations;
using MicrostationSignatures.Infrastructure.Interfaces;
using Unity;
using MicrostationSignatures.Models.Implementations;
using MicrostationSignatures.Models.Interfaces;
using static GadzhiConverting.Infrastructure.Implementations.Converters.SignaturesFunctionSync;

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
            container.RegisterType<ISignaturesToJpeg, SignaturesUpload>();
            container.RegisterType<IConverterDataFileFromDto, ConverterDataFileFromDto>();

            container.RegisterType<IMessagingService, MessagingService>();
            container.RegisterType<ILoggerService, LoggerService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>((unity) => 
                    ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint));

            var fileConvertingServerService = container.Resolve<IServiceConsumer<IFileConvertingServerService>>();
            var converterDataFileFromDto = container.Resolve<IConverterDataFileFromDto>();
            var signaturesLibrarySearching = new SignaturesLibrarySearching(Enumerable.Empty<ISignatureLibrary>(),
                                                                            GetSignaturesSync(fileConvertingServerService,
                                                                                              converterDataFileFromDto,
                                                                                              ProjectSettings.DataSignaturesFolder));
            container.RegisterFactory<IApplicationMicrostation>(unity =>
                new ApplicationMicrostation(new MicrostationResources(signaturesLibrarySearching,
                                                                      ProjectSignatureSettings.SignatureMicrostationFileName,
                                                                      ProjectSignatureSettings.StampMicrostationFileName)));
        }
    }
}
