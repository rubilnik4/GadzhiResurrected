using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiMicrostation.Extensions.StringAdditional;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с изменениями Microstation
    /// </summary>
    public class StampChangeMicrostation : StampSignatureMicrostation, IStampChangeMicrostation
    {
        public StampChangeMicrostation(IStampFieldMicrostation numberChange, IStampFieldMicrostation numberOfPlots,
                                       IStampFieldMicrostation typeOfChange, IStampFieldMicrostation documentChange,
                                       IStampFieldMicrostation dateChange, string personId, string personName,
                                       Func<string, string, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            : this(numberChange, numberOfPlots, typeOfChange, documentChange, dateChange, personId, personName, insertSignatureFunc,
                   GetNotInitializedSignature(personName))
        { }

        public StampChangeMicrostation(IStampFieldMicrostation numberChange, IStampFieldMicrostation numberOfPlots,
                                       IStampFieldMicrostation typeOfChange, IStampFieldMicrostation documentChange,
                                       IStampFieldMicrostation dateChange, string personId, string personName,
                                       Func<string, string, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                       IResultAppValue<IStampFieldMicrostation> signature)
            : base(insertSignatureFunc, signature)
        {
            if (String.IsNullOrEmpty(personId)) throw new ArgumentNullException(nameof(personId));

            PersonId = personId;
            PersonName = personName;
            NumberChange = numberChange;
            NumberOfPlots = numberOfPlots;
            TypeOfChange = typeOfChange;
            DocumentChange = documentChange;
            DateChange = dateChange;
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public IStampFieldMicrostation NumberChange { get; }

        /// <summary>
        /// Номер изменения. Элемент
        /// </summary>
        public ITextElementMicrostation NumberChangeElement => NumberChange.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Количество участков
        /// </summary>
        public IStampFieldMicrostation NumberOfPlots { get; }

        /// <summary>
        ///Количество участков. Элемент
        /// </summary>
        public ITextElementMicrostation NumberOfPlotsElement => NumberOfPlots.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Тип изменения
        /// </summary>
        public IStampFieldMicrostation TypeOfChange { get; }

        /// <summary>
        /// Тип изменения. Элемент
        /// </summary>
        public ITextElementMicrostation TypeOfChangeElement => TypeOfChange.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Номер документа
        /// </summary>
        public IStampFieldMicrostation DocumentChange { get; }

        /// <summary>
        /// Номер документа. Элемент
        /// </summary>
        public ITextElementMicrostation DocumentChangeElement => DocumentChange.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Дата изменения
        /// </summary>
        public IStampFieldMicrostation DateChange { get; }

        /// <summary>
        ///Дата изменения. Элемент
        /// </summary>
        public ITextElementMicrostation DateChangeElement => DateChange.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// идентификатор личности
        /// </summary>    
        public override string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName { get; }

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        public override bool IsPersonFieldValid() => !String.IsNullOrEmpty(PersonId) && 
                                                     !NumberChangeElement.Text.IsNullOrWhiteSpace();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> InsertSignature() =>
            new StampChangeMicrostation(NumberChange, NumberOfPlots, TypeOfChange, DocumentChange, DateChange, PersonId, PersonName,
                                        InsertSignatureFunc, InsertSignatureFunc.Invoke(PersonId, PersonName));

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> DeleteSignature() =>
            new StampChangeMicrostation(NumberChange, NumberOfPlots, TypeOfChange, DocumentChange, DateChange, PersonId, PersonName,
                                        InsertSignatureFunc);
    }
}
