using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка с согласованием
    /// </summary>
    public interface IStampApproval<out TField> : IStampSignature<TField>
        where TField : IStampField
    {
        /// <summary>
        /// Отдел согласования
        /// </summary>
        TField DepartmentApproval { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        TField ResponsiblePerson { get; }        

        /// <summary>
        /// Дата
        /// </summary>
        TField DateSignature { get; }      
    }
}
