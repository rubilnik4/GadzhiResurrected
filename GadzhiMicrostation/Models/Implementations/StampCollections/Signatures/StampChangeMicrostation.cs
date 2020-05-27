using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с изменениями Microstation
    /// </summary>
    public class StampChangeMicrostation : StampSignatureMicrostation, IStampChangeMicrostation
    {
        public StampChangeMicrostation(IStampFieldMicrostation numberChange, IStampFieldMicrostation numberOfPlots,
                                       IStampFieldMicrostation typeOfChange, IStampFieldMicrostation documentChange,
                                       IStampFieldMicrostation dateChange, ISignatureLibraryApp signatureLibrary,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            : this(numberChange, numberOfPlots, typeOfChange, documentChange, dateChange, signatureLibrary, insertSignatureFunc,
                   GetNotInitializedSignature(signatureLibrary?.PersonInformation.FullName))
        { }

        public StampChangeMicrostation(IStampFieldMicrostation numberChange, IStampFieldMicrostation numberOfPlots,
                                       IStampFieldMicrostation typeOfChange, IStampFieldMicrostation documentChange,
                                       IStampFieldMicrostation dateChange, ISignatureLibraryApp signatureLibrary,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                       IResultAppValue<IStampFieldMicrostation> signature)
            : base(insertSignatureFunc, signature)
        {
            PersonId = signatureLibrary?.PersonId ?? throw new ArgumentNullException(nameof(signatureLibrary));
            PersonInformation = signatureLibrary.PersonInformation;
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
        public override PersonInformationApp PersonInformation { get; }

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        public override bool IsPersonFieldValid() => !String.IsNullOrEmpty(PersonId) && 
                                                     !NumberChangeElement.Text.IsNullOrWhiteSpace();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> InsertSignature(ISignatureFileApp signatureFile) =>
            new StampChangeMicrostation(NumberChange, NumberOfPlots, TypeOfChange, DocumentChange, DateChange, signatureFile,
                                        InsertSignatureFunc, InsertSignatureFunc.Invoke(signatureFile));

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> DeleteSignature() =>
            new StampChangeMicrostation(NumberChange, NumberOfPlots, TypeOfChange, DocumentChange, DateChange,
                                        new SignatureLibraryApp(PersonId, PersonInformation), InsertSignatureFunc);
    }
}
