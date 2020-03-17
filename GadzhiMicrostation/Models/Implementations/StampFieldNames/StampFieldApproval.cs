using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Строка с согласованием
    /// </summary>
    public class StampFieldApproval
    {
        public StampFieldApproval(string departmentApproval, string responsiblePerson, string date)
        {
            DepartmentApproval = new StampFieldBase(departmentApproval, isVertical: true);
            ResponsiblePerson = new StampFieldBase(responsiblePerson, isVertical: true);
            DateSignature = new StampFieldBase(date, isVertical: true);
        }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public StampFieldBase DepartmentApproval { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public StampFieldBase ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public StampFieldBase DateSignature { get; }

        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<StampFieldBase> StampApprovalSignatureFields => new HashSet<StampFieldBase>()
        {
            DepartmentApproval,
            ResponsiblePerson,
            DateSignature,
        };
    }
}
