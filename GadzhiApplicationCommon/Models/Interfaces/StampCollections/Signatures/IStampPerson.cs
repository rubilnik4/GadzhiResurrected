using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка с ответственным лицом и подписью
    /// </summary>
    public interface IStampPerson: IStampSignature
    {
        /// <summary>
        /// Тип действия
        /// </summary>
        IStampTextField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        IStampTextField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        IStampTextField DateSignature { get; } 
    }
}
