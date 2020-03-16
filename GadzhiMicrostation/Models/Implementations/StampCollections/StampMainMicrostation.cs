using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IStampPersonSignatureMicrostation =
    GadzhiApplicationCommon.Models.Interfaces.StampCollections.IStampPersonSignature
            <GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections.IStampTextFieldMicrostation,
             GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections.IStampCellFieldMicrostation>;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Основные поля штампа Microstation
    /// </summary>
    public class StampMainMicrostation : StampMicrostation, IStampMain<IStampTextFieldMicrostation, IStampCellFieldMicrostation>
    {
        public StampMainMicrostation(ICellElementMicrostation stampCellElement)
            : base(stampCellElement)
        {
            StampPersonSignatures = GetStampPersonRowsWithoutSignatures();
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        public IEnumerable<IStampPersonSignatureMicrostation> StampPersonSignatures { get; private set; }

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
            StampPersonSignatures = StampPersonSignatures.Select(person => GetStampPersonRowWithSignatures(person));
            return StampPersonSignatures.Select(personSignature => personSignature.Signature.ElementStamp);
        }  

        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        private IEnumerable<IStampPersonSignatureMicrostation> GetStampPersonRowsWithoutSignatures()=>
            StampFieldPersonSignatures.GetStampRowPersonSignatures().
                                           Select(signatureRow => signatureRow.StampPersonSignatureFields.
                                                                               Select(field => field.Name)).
                                           Select(signatureRow => new StampPersonSignaturesMicrostation<ITextElementMicrostation, 
                                                                                                    ICellElementMicrostation>(FindElementsInStampFields(signatureRow)));

        /// <summary>
        /// Получить строку с ответственным лицом с подписью
        /// </summary>
        private IStampPersonSignatureMicrostation GetStampPersonRowWithSignatures(IStampPersonSignatureMicrostation person) =>
            new StampPersonSignaturesMicrostation<ITextElementMicrostation, 
                                                  ICellElementMicrostation>(person.ActionType,
                                                                            person.ResponsiblePerson,
                                                                            new StampFieldMicrostation<ICellElementMicrostation>(InsertSignatureFromLibrary(person),
                                                                                                                                 StampFieldType.PersonSignature),
                                                                            person.DateSignature) 
            as IStampPersonSignatureMicrostation;      

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        private ICellElementMicrostation InsertSignatureFromLibrary(IStampPersonSignatureMicrostation personSignature) =>
           InsertSignature(personSignature.AttributePersonId,
                           personSignature.ResponsiblePerson.ElementStamp.Text,
                           personSignature.ResponsiblePerson.ElementStamp,
                           personSignature.DateSignature.ElementStamp);
    }
}
