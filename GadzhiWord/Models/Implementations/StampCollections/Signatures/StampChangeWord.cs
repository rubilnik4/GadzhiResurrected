using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Interfaces.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с изменениями Word
    /// </summary>
    public class StampChangeWord : StampSignatureWord, IStampChange
    {
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int FIELDS_COUNT = 6;

        public StampChangeWord(ISignatureLibraryApp signatureLibrary, IStampFieldWord signature, IStampTextField numberChange,
                               IStampTextField numberPlots, IStampTextField typeOfChange, IStampTextField documentChange,
                               IStampTextField dateChange)
            : base(signatureLibrary, signature)
        {
            NumberChange = numberChange;
            NumberPlots = numberPlots;
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
    }
}
