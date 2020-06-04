using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampContainer
{
    public static class StampDocument
    {
        /// <summary>
        /// Определить тип документа по типу шифра в штампе
        /// </summary>
        public static StampDocumentType GetDocumentTypeByFullCode(string fullCode, StampApplicationType stampApplicationType) =>
            stampApplicationType switch
            {
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(2), ".с") => StampDocumentType.Specification,
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(3), ".вр") => StampDocumentType.BillOfQuantities,
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(3), ".кж") => StampDocumentType.CableMagazine,
                StampApplicationType.Microstation => StampDocumentType.Drawing,
                _ => StampDocumentType.Unknown,
            };

        /// <summary>
        /// Сравнить подстроку
        /// </summary>
        private static bool CompareSubstring(string substring, string equalMarker) =>
            substring.Equals(equalMarker, StringComparison.CurrentCultureIgnoreCase);
    }
}