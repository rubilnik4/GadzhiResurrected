using System;

namespace GadzhiDAL.Entities.FilesConvert.Main
{
    /// <summary>
    /// Параметры конвертации. Компонент в базе данных
    /// </summary>
    public class ConvertingSettingsComponent
    {
        /// <summary>
        /// Отдел
        /// </summary>    
        public virtual string Department { get; set; } = String.Empty;
    }
}