﻿using ChannelAdam.ServiceModel;
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
using Unity;
using Unity.Lifetime;

namespace GadzhiConverting.DependencyInjection.GadzhiConverting
{
    /// <summary>
    /// Класс для регистрации зависимостей
    /// </summary>
    public static class BootStrapUnity
    {
        public static void Start(IUnityContainer container)
        {
            var clientEndpoints = new ClientEndpoints();
            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingServerService));
           
            container.RegisterSingleton<IProjectSettings, ProjectSettings>();
            container.RegisterSingleton<IConvertingFileData, ConvertingFileData>();
            container.RegisterType<IApplicationConverting, ApplicationConverting>(new ContainerControlledLifetimeManager());
            container.RegisterType<IConvertingService, ConvertingService>(new ContainerControlledLifetimeManager());             
            container.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>((unity) =>
                      ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint), new ContainerControlledLifetimeManager());

            container.RegisterType<IMessageAndLoggingService, MessageAndLoggingService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterType<IExecuteAndCatchErrors, ExecuteAndCatchErrors>();
            container.RegisterType<IConverterServerFilesDataFromDTO, ConverterServerFilesDataFromDTO>();
            container.RegisterType<IConverterServerFilesDataToDTO, ConverterServerFilesDataToDTO>();

            //microstation
            container.RegisterType<IConvertingFileMicrostation, ConvertingFileMicrostation>(new HierarchicalLifetimeManager());            
        }
    }
}