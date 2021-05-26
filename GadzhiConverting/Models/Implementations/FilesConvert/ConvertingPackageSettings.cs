using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.FilesConvert;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    public class ConvertingPackageSettings : IConvertingPackageSettings
    {
        public ConvertingPackageSettings(string personId, PdfNamingType pdfNamingType,
                                         IEnumerable<ConvertingModeType> convertingModeTypes, bool useDefaultSignature)
        {
            PersonId = personId ?? String.Empty;
            PdfNamingType = pdfNamingType;
            ConvertingModeTypes = convertingModeTypes.ToList();
            UseDefaultSignature = useDefaultSignature;
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
        public IReadOnlyCollection<ConvertingModeType> ConvertingModeTypes { get; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        public bool UseDefaultSignature { get; }
    }
}