using System.Collections.Generic;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    public class ConvertingSettingsViewModel : BindableBase
    {
        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Отделы
        /// </summary>
        public IReadOnlyList<string> Departments => new List<string>
        {
            "АСО",
            "ЭЛТО"
        };
    }
}