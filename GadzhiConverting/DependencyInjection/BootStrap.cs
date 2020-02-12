using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DependencyInjection.GadzhiConverting
{
    /// <summary>
    /// Класс для регистрации зависимостей
    /// </summary>
    public static class BootStrapUnity
    {
        public static void Start(UnityContainer container)
        {
            container.RegisterSingleton<IApplicationConverting, ApplicationConverting>();           
            container.RegisterSingleton<IProjectSettings, ProjectSettings>();
            container.RegisterSingleton<IConvertingProject, ConvertingProject>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterType<IMessageAndLoggingService, MessageAndLoggingService>();
        }
    }
}
