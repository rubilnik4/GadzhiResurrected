﻿using System;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections.Fields;

namespace GadzhiWord.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с ответственным лицом и подписью
    /// </summary>
    public class PersonSignatureWord : SignatureWord, IStampPerson
    {
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int FIELDS_COUNT = 4;

        public PersonSignatureWord(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier, IStampFieldWord signature, 
                                   IStampTextField actionType,
                                   IStampTextField responsiblePerson, IStampTextField dateSignature)
            : base(signatureLibrary, stampIdentifier, signature)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            ActionType = actionType;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public IStampTextField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampTextField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public IStampTextField DateSignature { get; }
    }
}
