using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
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
            StampPersonSignaturesMicrostation = GetStampPersonRowsWithoutSignatures();
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответсвенным лицом и подписью Microstation
        /// </summary>
        private IEnumerable<IStampPersonSignatureMicrostation> StampPersonSignaturesMicrostation { get; set; }

        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        public IEnumerable<IStampPersonSignature<IStampFieldMicrostation>> StampPersonSignatures =>
                StampPersonSignaturesMicrostation.Cast<IStampPersonSignature<IStampFieldMicrostation>>();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override void DeleteSignatures()
        {
            foreach (var personSignature in StampPersonSignatures)
            {
                personSignature?.Signature.ElementStamp.Remove();
            }
        }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        protected override IEnumerable<ICellElementMicrostation> InsertSignaturesFromLibrary()
        {
            StampPersonSignaturesMicrostation = StampPersonSignaturesMicrostation.Select(person => GetStampPersonRowWithSignatures(person));
            return StampPersonSignaturesMicrostation.Select(personSignature => personSignature.SignatureElement);
        }

        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        private IEnumerable<IStampPersonSignatureMicrostation> GetStampPersonRowsWithoutSignatures() =>
            StampFieldPersonSignatures.GetStampRowPersonSignatures().
                                       Select(signatureRow => signatureRow.StampPersonSignatureFields.
                                                                           Select(field => field.Name)).
                                       Select(signatureRow => new StampPersonSignatureMicrostation(FindElementsInStampFields(signatureRow, 
                                                                                                                             ElementMicrostationType.TextElement).
                                                                                                   Cast<ITextElementMicrostation>())).
                                       Cast<IStampPersonSignatureMicrostation>();

        /// <summary>
        /// Получить строку с ответственным лицом с подписью
        /// </summary>
        private IStampPersonSignatureMicrostation GetStampPersonRowWithSignatures(IStampPersonSignatureMicrostation person) =>
            new StampPersonSignatureMicrostation(person.ActionType,
                                                 person.ResponsiblePerson,
                                                 new StampFieldMicrostation(InsertSignatureFromLibrary(person),
                                                                            StampFieldType.PersonSignature),
                                                 person.DateSignature)
            as IStampPersonSignatureMicrostation;

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        private ICellElementMicrostation InsertSignatureFromLibrary(IStampPersonSignatureMicrostation personSignature) =>
           InsertSignature(personSignature.AttributePersonId,
                           personSignature.ResponsiblePersonElement.Text,
                           personSignature.ResponsiblePersonElement,
                           personSignature.DateSignatureElement);
    }
}
