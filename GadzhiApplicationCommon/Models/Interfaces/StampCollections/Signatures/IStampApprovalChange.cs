using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка согласования для извещения изменений
    /// </summary>
    public interface IStampApprovalChange : IStampSignature
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