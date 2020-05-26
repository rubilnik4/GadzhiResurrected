using System;
using GadzhiCommon.Enums.ConvertingSettings;

namespace GadzhiDAL.Entities.FilesConvert.Main
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
        /// Отдел
        /// </summary>    
        public virtual string Department { get; set; } = String.Empty;

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public PdfNamingType PdfNamingType { get; set; }
    }
}