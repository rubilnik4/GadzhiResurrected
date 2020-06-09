using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка согласования для опросных листов и тех требований
    /// </summary>
    public interface IStampApprovalPerformers : IStampSignature
    {
        /// <summary>
        /// Отдел согласования
        /// </summary>
        IStampTextField Department { get; }

        /// <summary>
        /// Дата
        /// </summary>
        IStampTextField DateSignature { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        IStampTextField ResponsiblePerson { get; }
    }
}