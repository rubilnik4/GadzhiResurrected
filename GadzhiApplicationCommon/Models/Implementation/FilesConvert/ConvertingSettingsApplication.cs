using System;

namespace GadzhiApplicationCommon.Models.Implementation.FilesConvert
{
    public class ConvertingSettingsApplication
    {
        public ConvertingSettingsApplication(string department)
        {
            Department = department ?? String.Empty;
        }

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; }
    }
}