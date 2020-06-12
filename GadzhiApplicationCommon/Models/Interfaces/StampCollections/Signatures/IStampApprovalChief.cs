using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка согласования для опросных листов и тех требований с директорами
    /// </summary>
    public interface IStampApprovalChief : IStampSignature
    {
        /// <summary>
        /// Отдел согласования
        /// </summary>
        IStampTextField Department { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        IStampTextField ResponsiblePerson { get; }
    }
}