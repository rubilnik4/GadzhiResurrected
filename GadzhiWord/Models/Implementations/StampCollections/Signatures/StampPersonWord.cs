using System;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiWord.Models.Interfaces.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с ответственным лицом и подписью
    /// </summary>
    public class StampPersonWord : StampSignatureWord, IStampPersonWord
    {
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int FIELDS_COUNT = 4;
       
        public StampPersonWord(IStampFieldWord actionType, IStampFieldWord responsiblePerson, IStampFieldWord signature, 
                               IStampFieldWord dateSignature, ISignatureLibraryApp signatureLibrary)
            : base(signature, signatureLibrary)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            ActionType = actionType;
            DateSignature = dateSignature;      
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public IStampFieldWord ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampFieldWord ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public IStampFieldWord DateSignature { get; }
    }
}
