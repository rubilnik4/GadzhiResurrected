using System;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Converters;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings
{
    /// <summary>
    /// Тип конвертации
    /// </summary>
    public class ConvertingMode : IFormattable
    {
        public ConvertingMode(ConvertingModeType convertingModeType, bool isUsed)
        {
            ConvertingModeType = convertingModeType;
            IsUsed = isUsed;
        }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        public ConvertingModeType ConvertingModeType { get; }

        /// <summary>
        /// Использование
        /// </summary>
        public bool IsUsed { get; set; }

        #region IFormattable
        public string ToString(string format, IFormatProvider formatProvider) =>
            ConvertingModeTypeConverter.ConvertingModeToString(ConvertingModeType);
        #endregion
    }
}