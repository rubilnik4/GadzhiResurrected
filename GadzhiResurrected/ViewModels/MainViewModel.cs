using GadzhiModules.Modules;
using Prism.Mvvm;

namespace GadzhiResurrected.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {

        }

        /// <summary>
        /// Название и версия
        /// </summary>
        private string _title = "Gadzhi";

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
