using System;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings(string department)
        {
            Department = department ?? String.Empty;
        }
        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; }
    }
}