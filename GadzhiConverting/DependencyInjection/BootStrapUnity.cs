using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
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
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConvertingLibrary.Extensions;
using GadzhiConvertingLibrary.Infrastructure.Implementations;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Converters;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Converters;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Services;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOBase.Infrastructure.Interfaces.Converters;
using GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;
using GadzhiPdfPrinting.Infrastructure.Implementations;
using GadzhiPdfPrinting.Infrastructure.Interfaces;
using GadzhiWord.Word.Interfaces.Word;
using Unity;
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


            GadzhiConvertingLibrary.DependencyInjection.BootStrapUnity.RegisterServices(container);
            RegisterConvertingApplications(container);

            container.RegisterFactory<IApplicationConverting>(unity =>
                new ApplicationConverting(unity.Resolve<IApplicationLibrary<IDocumentMicrostation>>(nameof(ApplicationMicrostation)),
                                          unity.Resolve<IApplicationLibrary<IDocumentWord>>(nameof(ApplicationOffice)),
                                          unity.Resolve<IFileSystemOperations>(),
                                          unity.Resolve<IPdfCreatorService>(),
                                          unity.Resolve<IMessagingService>()));
        }

        /// <summary>
        /// Регистрация приложений Microstation и Word
        /// </summary>
        [Logger]
        private static void RegisterConvertingApplications(IUnityContainer container)
        {
            var signatureConverter = container.Resolve<ISignatureConverter>();
            var projectSettings = container.Resolve<IProjectSettings>();

            var convertingResources = projectSettings.ConvertingResources;
            var signaturesLibrarySearching = new SignaturesSearching(convertingResources.SignatureNames.Value.ToApplication(),
                                                                     SignaturesFunctionSync.GetSignaturesSync(convertingResources.SignatureNames,
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
