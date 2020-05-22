using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с изменениями Word
    /// </summary>
    public class StampChangeWord : StampSignatureWord, IStampChangeWord
    {       
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int FIELDS_COUNT = 6;

        public StampChangeWord(IStampFieldWord numberChange, IStampFieldWord numberOfPlots, IStampFieldWord typeOfChange,
                               IStampFieldWord documentChange, IStampFieldWord signature, IStampFieldWord dateChange,
                               ISignatureLibrary signatureLibrary)
            : base(signature, signatureLibrary)
        {
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
        /// Номер документа
        /// </summary>
        public IStampFieldWord DocumentChange { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public IStampFieldWord DateChange { get; }
    }
}
