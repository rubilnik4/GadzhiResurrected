using System;
using System.Collections.Generic;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ConvertingSettings
{
    /// <summary>
    /// Параметры конвертации, отображение
    /// </summary>
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings()
            : this(String.Empty, null)
        { }

        public ConvertingSettings(string department, ISignatureLibrary personSignature)
        {
            Department = department ?? String.Empty;
            PersonSignature = personSignature;
        }

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public ISignatureLibrary PersonSignature { get; set; }
    }
}