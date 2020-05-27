using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка с согласованием
    /// </summary>
    public interface IStampApproval : IStampSignature
    {
        /// <summary>
        /// Отдел согласования
        /// </summary>
        IStampTextField DepartmentApproval { get; }

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
