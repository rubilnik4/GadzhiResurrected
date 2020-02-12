using GadzhiDAL.DependencyInjection;
using GadzhiDTO.Contracts.FilesConvert;
using GadzhiWcfHost.Helpers;
using GadzhiWcfHost.Infrastructure.Implementations;
using GadzhiWcfHost.Infrastructure.Interfaces;
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
                .RegisterType<IFileConvertingService, FileConvertingService>()
                .RegisterType<IApplicationUploadAndGetConverting, ApplicationUploadAndGetConverting>(new HierarchicalLifetimeManager());
              
            GadzhiDALDependencyInjection.ConfigureContainer(container, 
                                                            HostSystemInformation.DataBasePath,
                                                            false);
        }

    }
}