using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Основные поля штампа Microstation
    /// </summary>
    public class StampMainMicrostation : StampMicrostation, IStampMain<IStampFieldMicrostation>
    {
        public StampMainMicrostation(ICellElementMicrostation stampCellElement)
            : base(stampCellElement)
        {

        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        public IEnumerable<IStampPersonSignature<IStampFieldMicrostation>> StampPersonSignatures =>
                StampFieldPersonSignatures.GetStampRowPersonSignatures().
                                           Select(signatureRow => signatureRow.StampPersonSignatureFields.
                                                                               Select(field => field.Name)).
                                           Select(signatureRow => new StampPersonSignaturesMicrostation(FindElementsInStampFields(signatureRow)));


        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        protected override IEnumerable<ICellElementMicrostation> InsertSignatureFromLibrary() =>
            StampPersonSignatures?.Select(personSignature => InsertSignature(personSignature.AttributePersonId,
                                                                             personSignature.ResponsiblePerson.Text,
                                                                             personSignature.ResponsiblePerson.TextElementStamp,
                                                                             personSignature.DateSignature.TextElementStamp)).
                                   Where(insertedSignature => insertedSignature != null);
           

        private IEnumerable<IStampPersonSignature<IStampFieldMicrostation>> GetStampPersonSignatures()=>
    }
}
