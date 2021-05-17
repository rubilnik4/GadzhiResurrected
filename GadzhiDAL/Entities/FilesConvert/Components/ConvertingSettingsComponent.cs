using System;
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
        public PdfNamingType PdfNamingType { get; set; } = PdfNamingType.ByFile;

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public ConvertingModeType ConvertingModeType { get; set; } = ConvertingModeType.All;

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        public bool UseDefaultSignature { get; set; }
    }
}