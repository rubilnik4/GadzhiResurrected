using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка с изменениями
    /// </summary>
    public interface IStampChange: IStampSignature
    {
        /// <summary>
        /// Номер изменения
        /// </summary>
        IStampTextField NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        IStampTextField NumberPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        IStampTextField TypeOfChange { get; }

        /// <summary>
        /// Номер документа
        /// </summary>
        IStampTextField DocumentChange { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        IStampTextField DateChange { get; }
    }
}
