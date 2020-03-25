using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с изменениями Word
    /// </summary>
    public class StampChangeSignatureWord : StampSignatureWord, IStampChangeSignature<IStampFieldWord>
    {       
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public static int FieldsCount => 6;

        public StampChangeSignatureWord(IStampFieldWord numberChange, IStampFieldWord numberOfPlots,
                                        IStampFieldWord typeOfChange, IStampFieldWord documentChange,
                                        IStampFieldWord signature, IStampFieldWord dateChange,
                                        SignatureInformation signatureInformation)
            : base(signature, signatureInformation?.SignaturePath)
        {
            PersonId = signatureInformation?.PersonId ?? throw new ArgumentNullException(nameof(PersonId));
            PersonName = signatureInformation?.PersonName;

            NumberChange = numberChange;
            NumberOfPlots = numberOfPlots;
            TypeOfChange = typeOfChange;
            DocumentChange = documentChange;
            DateChange = dateChange;
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public IStampFieldWord NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        public IStampFieldWord NumberOfPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public IStampFieldWord TypeOfChange { get; }

        /// <summary>
        /// Номер докумета
        /// </summary>
        public IStampFieldWord DocumentChange { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public IStampFieldWord DateChange { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName { get; }
    }
}
