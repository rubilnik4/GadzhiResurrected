using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Строка с согласованием
    /// </summary>
    public abstract class StampApprovalSignature<TField>: IStampApprovalSignatures<TField> 
                                                          where TField : IStampField
    {
        public StampApprovalSignature() { }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public abstract TField DepartmentApproval { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public abstract TField ResponsiblePerson { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        public abstract TField Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public abstract TField DateSignature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public abstract string AttributePersonId { get; }
    }
}
