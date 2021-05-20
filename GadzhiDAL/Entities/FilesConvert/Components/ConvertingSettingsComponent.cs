using System;
using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDAL.Entities.FilesConvert.Components
{
    /// <summary>
    /// Параметры конвертации. Компонент в базе данных
    /// </summary>
    public class ConvertingSettingsComponent
    {
        /// <summary>
        /// Личная подпись
        /// </summary>
        public string PersonId { get; set; } = String.Empty;

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public PdfNamingType PdfNamingType { get; set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public IList<ConvertingModeType> ConvertingModeTypes { get; set; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        public bool UseDefaultSignature { get; set; }
    }
}