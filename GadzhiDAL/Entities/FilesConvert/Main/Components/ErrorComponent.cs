using System;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDAL.Entities.FilesConvert.Main.Components
{
    /// <summary>
    /// Информация об ошибке
    /// </summary>
    public class ErrorComponent
    {
        /// <summary>
        /// Тип ошибки при конвертации файлов
        /// </summary>
        public virtual FileConvertErrorType FileConvertErrorType { get; set; } = FileConvertErrorType.NoError;

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public virtual string ErrorDescription { get; set; } = String.Empty;
    }
}