using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces;
using GadzhiDAL.DependencyInjection;
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
            container.RegisterType<IConvertingService, ConvertingService>();
            container.RegisterType<IFileSystemOperations, FileSystemOperations>();
            container.RegisterType<IMessageAndLoggingService, MessageAndLoggingService>();
            container.RegisterType<IExecuteAndCatchErrors, ExecuteAndCatchErrors>();
            container.RegisterType<IConverterServerFilesDataFromDTO, ConverterServerFilesDataFromDTO>();
            container.RegisterType<IConverterServerFilesDataToDTO, ConverterServerFilesDataToDTO>();

            var projectSettings = container.Resolve<IProjectSettings>();
            GadzhiDALDependencyInjection.ConfigureContainer(container, 
                                                            projectSettings.SQLiteDataBasePath,
                                                            true);
        }
    }
}
