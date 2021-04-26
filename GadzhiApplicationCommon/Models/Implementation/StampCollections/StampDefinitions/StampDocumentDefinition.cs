using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampDefinitions
{
    public static class StampDocumentDefinition
    {
        /// <summary>
        /// Определить тип документа по типу шифра в штампе
        /// </summary>
        public static StampDocumentType GetDocumentTypeByFullCode(string fullCode, StampApplicationType stampApplicationType) =>
            stampApplicationType switch
            {
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(2), ".с") => StampDocumentType.Specification,
                StampApplicationType.Word when fullCode.ContainsIgnoreCase("-со-") => StampDocumentType.Specification,
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(3), ".вр") => StampDocumentType.BillOfQuantities,
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(3), ".кж") => StampDocumentType.CableMagazine,
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(3), ".тт") => StampDocumentType.TechnicalRequirements,
                StampApplicationType.Word when fullCode.ContainsIgnoreCase("-тт-") => StampDocumentType.TechnicalRequirements,
                StampApplicationType.Word when CompareSubstring(fullCode.SubstringEnd(3), ".ол") => StampDocumentType.Questionnaire,
                StampApplicationType.Word when fullCode.ContainsIgnoreCase("-ол-") => StampDocumentType.Questionnaire,
                StampApplicationType.Microstation => StampDocumentType.Drawing,
                _ => StampDocumentType.Unknown,
            };

        /// <summary>
        /// Является ли тип документа тех требованиями или опросным листом
        /// </summary>
        public static bool IsDocumentQuestionnaire(StampDocumentType stampDocumentType) =>
            stampDocumentType == StampDocumentType.Questionnaire || stampDocumentType == StampDocumentType.TechnicalRequirements;

        /// <summary>
        /// Сравнить подстроку
        /// </summary>
        private static bool CompareSubstring(string substring, string equalMarker) =>
            substring.Equals(equalMarker, StringComparison.CurrentCultureIgnoreCase);
    }
}