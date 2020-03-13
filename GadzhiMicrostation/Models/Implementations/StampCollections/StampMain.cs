using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public class StampMain : Stamp, IStampMain
    {
        public StampMain(CellElement stampCellElement, IOwnerContainerMicrostation ownerModelMicrostation)
            : base(stampCellElement, ownerModelMicrostation)
        {

        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        public IEnumerable<IStampPersonSignature> StampPersonSignatures =>
                StampFieldPersonSignatures.GetStampRowPersonSignatures().
                                           Select(signatureRow => signatureRow.StampPersonSignatureFields.
                                                                               Select(field => field.Name)).
                                           Select(signatureRow => new StampPersonSignature(FindElementsInStampFields(signatureRow))).
                                           Cast<IStampPersonSignature>();
    }
}
