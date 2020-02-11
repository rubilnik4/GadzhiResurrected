using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Характеристика ошибок
    /// </summary>
    public class ErrorTypeConverting
    {
        public ErrorTypeConverting(FileConvertErrorType fileConvertErrorType,
                                   string fileConvertErrorDescription)
        {
            FileConvertErrorType = fileConvertErrorType;
            FileConvertErrorDescription = fileConvertErrorDescription;
        }

        /// <summary>
        /// Тип ошибки
        /// </summary>
        public FileConvertErrorType FileConvertErrorType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string FileConvertErrorDescription { get; }
    }
}
