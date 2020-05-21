using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ConvertingSettings
{
    /// <summary>
    /// Параметры конвертации, отображение
    /// </summary>
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings()
            : this(Departments[0])
        { }

        public ConvertingSettings(string department)
        {
            Department = department ?? String.Empty;
        }

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Отделы
        /// </summary>
        public static IReadOnlyList<string> Departments => new List<string>
        {
            "ЭЛТО",
            "АСО",
        };
    }
}