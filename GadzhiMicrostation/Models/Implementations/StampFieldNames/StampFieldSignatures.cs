using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Models.Interfaces.StampFieldNames;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Поля штампа с подписью
    /// </summary>
    public static class StampFieldSignatures
    {
        /// <summary>
        /// Получить строки штампа с подписью по их типу
        /// </summary>
        public static IEnumerable<IStampFieldSignature> GetRowFieldsBySignatureType(StampFieldType stampFieldType) =>
            stampFieldType switch
            {
                StampFieldType.ApprovalSignature => StampFieldApprovals.GetStampRowApprovalSignatures().
                                                                        Cast<IStampFieldSignature>(),
                StampFieldType.ChangeSignature => StampFieldChanges.GetStampRowChangesSignatures().
                                                                    Cast<IStampFieldSignature>(),
                StampFieldType.PersonSignature => StampFieldPersons.GetStampRowPersonSignatures().
                                                                    Cast<IStampFieldSignature>(),
                _ => throw new InvalidEnumArgumentException(nameof(stampFieldType), (int)stampFieldType, typeof(StampFieldType)),
            };

        /// <summary>
        /// Получить поля штампа с подписью по их типу
        /// </summary>
        public static IEnumerable<IEnumerable<string>> GetFieldsBySignatureType(StampFieldType stampFieldType) =>
            GetRowFieldsBySignatureType(stampFieldType).
            Select(signatureRow => signatureRow.StampSignatureFields.Select(field => field.Name));
    }
}
