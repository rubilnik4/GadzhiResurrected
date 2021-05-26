using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.FilesConvert;

// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.FilesConvert.Components
{
    /// <summary>
    /// Параметры конвертации. Компонент в базе данных
    /// </summary>
    public class ConvertingSettingsComponent: IConvertingPackageSettings
    {
        public ConvertingSettingsComponent()
        { }

        public ConvertingSettingsComponent(string personId, PdfNamingType pdfNamingType,
                                           IList<ConvertingModeType> convertingModeTypesList, bool useDefaultSignature)
        {
            PersonId = personId;
            PdfNamingType = pdfNamingType;
            ConvertingModeTypesList = convertingModeTypesList;
            UseDefaultSignature = useDefaultSignature;
        }

        /// <summary>
        /// Личная подпись
        /// </summary>
        public virtual string PersonId { get; protected set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public virtual PdfNamingType PdfNamingType { get; protected set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public virtual IList<ConvertingModeType> ConvertingModeTypesList { get; protected set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public IReadOnlyCollection<ConvertingModeType> ConvertingModeTypes =>
            ConvertingModeTypesList.ToList();

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        public virtual bool UseDefaultSignature { get; protected set; }
    }
}