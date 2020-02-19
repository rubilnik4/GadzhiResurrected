﻿using GadzhiMicrostation.Infrastructure.Implementations;
using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Implementations;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using Microsoft.Practices.Unity;

namespace GadzhiMicrostation.DependencyInjection.BootStrapMicrostation
{
    /// <summary>
    /// Класс для регистрации зависимостей
    /// </summary>
    public static class BootStrapUnityMicrostation
    {
        public static void Start(IUnityContainer container)
        {       
            //microstation
            container.RegisterType<IMicrostationProject, MicrostationProject>(new HierarchicalLifetimeManager());
            container.RegisterType<IErrorMessagingMicrostation, ErrorMessagingMicrostation>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectMicrostationSettings, ProjectMicrostationSettings>(new HierarchicalLifetimeManager());
            container.RegisterType<ILoggerMicrostation, LoggerMicrostation>(new HierarchicalLifetimeManager());
            container.RegisterType<IExecuteAndCatchErrorsMicrostation, ExecuteAndCatchErrorsMicrostation>();
            container.RegisterType<IFileSystemOperationsMicrostation, FileSystemOperationsMicrostation>();

            container.RegisterType<IApplicationMicrostation, ApplicationMicrostation>(new HierarchicalLifetimeManager());
            container.RegisterType<IDesignFileMicrostation, DesignFileMicrostation>(new HierarchicalLifetimeManager());
        }
    }
}
