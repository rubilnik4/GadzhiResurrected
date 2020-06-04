using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с изменениями Microstation
    /// </summary>
    public class ChangeSignatureMicrostation : SignatureMicrostation, IStampChange
    {
        public ChangeSignatureMicrostation(ISignatureLibraryApp signatureLibrary,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                       IStampTextField numberChange, IStampTextField numberOfPlots, IStampTextField typeOfChange,
                                       IStampTextField documentChange, IStampTextField dateChange)
            : this(signatureLibrary, GetNotInitializedSignature(signatureLibrary?.PersonInformation.Surname),
                   insertSignatureFunc, numberChange, numberOfPlots, typeOfChange, documentChange, dateChange)
        { }

        public ChangeSignatureMicrostation(ISignatureLibraryApp signatureLibrary, IResultAppValue<IStampField> signature,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                       IStampTextField numberChange, IStampTextField numberOfPlots, IStampTextField typeOfChange,
                                       IStampTextField documentChange, IStampTextField dateChange)
            : base(signatureLibrary, signature, insertSignatureFunc)
        {
            NumberChange = numberChange;
            NumberPlots = numberOfPlots;
            TypeOfChange = typeOfChange;
            DocumentChange = documentChange;
            DateChange = dateChange;
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public IStampTextField NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        public IStampTextField NumberPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public IStampTextField TypeOfChange { get; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public IStampTextField DocumentChange { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public IStampTextField DateChange { get; }

        /// <summary>
        /// Необходимо ли вставлять подпись в поле
        /// </summary>
        public override bool NeedToInsert() => IsPersonFieldValid() && !NumberChange.Text.IsNullOrWhiteSpace();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature InsertSignature(ISignatureFileApp signatureFile) =>
            new ChangeSignatureMicrostation(SignatureLibrary, InsertSignatureFunc.Invoke(signatureFile), InsertSignatureFunc,
                                        NumberChange, NumberPlots, TypeOfChange, DocumentChange, DateChange);

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature DeleteSignature() =>
            new ChangeSignatureMicrostation(SignatureLibrary, InsertSignatureFunc, NumberChange, NumberPlots, 
                                        TypeOfChange, DocumentChange, DateChange);
    }
}
