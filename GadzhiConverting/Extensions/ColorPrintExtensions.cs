using GadzhiApplicationCommon.Models.Interfaces;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Models.Converters;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Extensions
{
    /// <summary>
    /// Методы расширения для цвета печати
    /// </summary>
    public static class ColorPrintExtensions
    {
        /// <summary>
        /// Конвертировать тип цвета печати в версию для приложения
        /// </summary>
        public static ColorPrintApplication ToApplication(this ColorPrint colorPrint) => 
            ColorPrintToApplicationConverter.ConvertingToApplication(colorPrint);      
    }
}
