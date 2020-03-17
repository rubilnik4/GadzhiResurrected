using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с согласованием
    /// </summary>
    public interface IStampApprovalSignatures<out TField> where TField : IStampField
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
        /// Подпись
        /// </summary>
        TField Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        TField DateSignature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        string AttributePersonId { get; }
    }
}
