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
        public static StampDocumentType GetDocumentTypeByFullCode(string fullCode) =>
            fullCode switch
            {
                _ when fullCode.SubstringEnd(2).Equals(".с", StringComparison.CurrentCultureIgnoreCase) => StampDocumentType.Specification,
                _ when fullCode.SubstringEnd(2).Equals(".вр", StringComparison.CurrentCultureIgnoreCase) => StampDocumentType.BillOfQuantities,
                _ when fullCode.SubstringEnd(2).Equals(".кж", StringComparison.CurrentCultureIgnoreCase) => StampDocumentType.CableMagazine,
                _ => StampDocumentType.Drawing,
            };
    }
}