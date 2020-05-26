using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с изменениями Microstation
    /// </summary>
    public interface IStampChangeMicrostation : IStampChange<IStampFieldMicrostation>, IStampSignatureMicrostation
    {
        /// <summary>
        /// Номер изменения. Элемент
        /// </summary>
        ITextElementMicrostation NumberChangeElement { get; }

        /// <summary>
        /// Количество участков. Элемент
        /// </summary>
        ITextElementMicrostation NumberOfPlotsElement { get; }

        /// <summary>
        /// Тип изменения. Элемент
        /// </summary>
        ITextElementMicrostation TypeOfChangeElement { get; }

        /// <summary>
        /// Номер документа. Элемент
        /// </summary>
        ITextElementMicrostation DocumentChangeElement { get; }

        /// <summary>
        /// Подпись. Элемент
        /// </summary>
        IResultAppValue<ICellElementMicrostation> SignatureElement { get; }

        /// <summary>
        /// Дата изменения. Элемент
        /// </summary>
        ITextElementMicrostation DateChangeElement { get; }
    }
}
