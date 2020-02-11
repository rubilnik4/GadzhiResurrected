using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDAL.DependencyInjection;
using GadzhiDTO.Contracts.FilesConvert;
using GadzhiWcfHost.Helpers;
using GadzhiWcfHost.Infrastructure.Implementations;
using GadzhiWcfHost.Infrastructure.Implementations.Converters;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Infrastructure.Interfaces.Converters;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using GadzhiWcfHost.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.Lifetime;
using Unity.Wcf;

namespace GadzhiWcfHost.DependencyInjection
{
    /// <summary>
    /// Класс для подключения инверсии зависимостей
    /// </summary>
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            // Регистрируем зависимости
            container
                //.RegisterType<IFileSystemOperations, FileSystemOperations>()
                .RegisterType<IFileConvertingService, FileConvertingService>()
                //.RegisterType<IQueueInformation, QueueInformation>()
                //.RegisterType<IConverterServerFilesDataFromDTO, ConverterServerFilesDataFromDTO>()
                //.RegisterType<IConverterServerFilesDataToDTO, ConverterServerFilesDataToDTO>()              
                //.RegisterType<IFilesDataPackages, FilesDataPackages>(new HierarchicalLifetimeManager())
                .RegisterType<IApplicationUploadAndGetConverting, ApplicationUploadAndGetConverting>(new HierarchicalLifetimeManager());

            GadzhiDALDependencyInjection.ConfigureContainer(container, HostSystemInformation.ApplicationPath);
        }

    }
}