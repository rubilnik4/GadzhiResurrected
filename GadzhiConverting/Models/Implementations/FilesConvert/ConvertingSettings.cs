using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    public class ConvertingSettings: ConvertingPackageSettings, IConvertingSettings
    {
        public ConvertingSettings(IConvertingPackageSettings convertingPackageSettings,
                                  IResultValue<IPrintersInformation> printersInformation)
          : this(convertingPackageSettings.PersonId, convertingPackageSettings.PdfNamingType,
                 convertingPackageSettings.ConvertingModeTypes, convertingPackageSettings.UseDefaultSignature, 
                 printersInformation)
        { }


        public ConvertingSettings(string personId, PdfNamingType pdfNamingType,
                                  IEnumerable<ConvertingModeType> convertingModeTypes, bool useDefaultSignature,
                                  IResultValue<IPrintersInformation> printersInformation)
            :base(personId, pdfNamingType, convertingModeTypes, useDefaultSignature)
        {
            PrintersInformation = printersInformation;
        }

        /// <summary>
        /// Информация о принтерах
        /// </summary>
        public IResultValue<IPrintersInformation> PrintersInformation { get; }
    }
}