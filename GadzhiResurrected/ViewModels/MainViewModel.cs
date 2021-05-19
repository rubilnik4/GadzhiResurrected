using System.Deployment.Application;
using System.Reflection;
using GadzhiModules.Modules;
using GadzhiResurrected.Infrastructure.Implementations.Reflections;
using Prism.Mvvm;

namespace GadzhiResurrected.ViewModels
{
    public class MainViewModel : BindableBase
    {
        /// <summary>
        /// Название и версия
        /// </summary>
        private string _title = $"Gadzhi ({VersionReflection.GetClickOnceVersion()})";

        /// <summary>
        /// Название и версия
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Название модуля для конвертации файлов
        /// </summary>
        public string GadzhiConvertingRegionName => RegionNames.GadzhiConvertingModule;
    }
}
