using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiMicrostation.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с ответственным лицом и подписью Microstation
    /// </summary>
    public class StampPersonMicrostation : StampSignatureMicrostation, IStampPersonMicrostation
    {
        public StampPersonMicrostation(IStampFieldMicrostation actionType, IStampFieldMicrostation responsiblePerson,
                                       IStampFieldMicrostation dateSignature,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            : this(actionType, responsiblePerson, dateSignature, insertSignatureFunc, 
                   GetNotInitializedSignature(responsiblePerson.ElementStamp.AsTextElementMicrostation.Text))
        { }

        public StampPersonMicrostation(IStampFieldMicrostation actionType, IStampFieldMicrostation responsiblePerson,
                                       IStampFieldMicrostation dateSignature,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                       IResultAppValue<IStampFieldMicrostation> signature)
            : base(insertSignatureFunc, signature)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            ActionType = actionType;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public IStampFieldMicrostation ActionType { get; }

        /// <summary>
        /// Тип действия. Элемент
        /// </summary>
        public ITextElementMicrostation ActionTypeElement => ActionType.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampFieldMicrostation ResponsiblePerson { get; }

        /// <summary>
        /// Ответственное лицо. Элемент
        /// </summary>
        public ITextElementMicrostation ResponsiblePersonElement => ResponsiblePerson.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Дата
        /// </summary>
        public IStampFieldMicrostation DateSignature { get; }

        /// <summary>
        /// Дата. Элемент
        /// </summary>
        public ITextElementMicrostation DateSignatureElement => DateSignature.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// идентификатор личности
        /// </summary>    
        public override string PersonId => ResponsiblePerson.ElementStamp.AttributePersonId;

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override PersonInformationApp PersonInformation => new PersonInformationApp(ResponsiblePersonElement.Text, String.Empty,
                                                                                     String.Empty, String.Empty);

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> InsertSignature(ISignatureFileApp signatureFile) =>
            new StampPersonMicrostation(ActionType, ResponsiblePerson, DateSignature, InsertSignatureFunc,
                                        InsertSignatureFunc.Invoke(signatureFile));

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> DeleteSignature() =>
            new StampPersonMicrostation(ActionType, ResponsiblePerson, DateSignature, InsertSignatureFunc);
    }
}
