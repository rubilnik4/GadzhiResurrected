using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiWord.Models.Interfaces.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections.Signatures
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
                               ISignatureLibraryApp signatureLibrary)
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
