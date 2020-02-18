using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Строка с согласованием
    /// </summary>
    public class StampApproval
    {
        public StampApproval(string departmentApproval,
                             string responsiblePerson,
                             string date)
        {
            DepartmentApproval = new StampBaseField(departmentApproval, isVertical: true);
            ResponsiblePerson = new StampBaseField(responsiblePerson, isVertical: true);
            Date = new StampBaseField(date, isVertical: true);
        }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public StampBaseField DepartmentApproval { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public StampBaseField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public StampBaseField Date { get; }

        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<StampBaseField> StampControlNamesApproval => new HashSet<StampBaseField>()
        {
            DepartmentApproval,
            ResponsiblePerson,
            Date,
        };
    }
}
