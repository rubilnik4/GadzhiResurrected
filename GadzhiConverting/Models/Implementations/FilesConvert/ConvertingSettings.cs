using System;
using System.Linq;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings(string personId, PdfNamingType pdfNamingType, 
                                  ConvertingModeType convertingModeType, bool useDefaultSignature,
                                  IResultValue<IPrintersInformation> printersInformation)
        {
            PersonId = personId ?? String.Empty;
            PdfNamingType = pdfNamingType;
            ConvertingModeType = convertingModeType;
            UseDefaultSignature = useDefaultSignature;
            PrintersInformation = printersInformation;
        }

        /// <summary>
        /// Идентификатор личной подпись
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Принцип именование PDF
        /// </summary>
        public PdfNamingType PdfNamingType { get; }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        public ConvertingModeType ConvertingModeType { get; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        public bool UseDefaultSignature { get; }

        /// <summary>
        /// Информация о принтерах
        /// </summary>
        public IResultValue<IPrintersInformation> PrintersInformation { get; }
    }
}