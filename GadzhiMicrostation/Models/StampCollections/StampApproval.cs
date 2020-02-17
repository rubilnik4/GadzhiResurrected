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
            departmentApproval = DepartmentApproval;
            responsiblePerson = ResponsiblePerson;
            date = Date;
        }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public string DepartmentApproval { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public string ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<string> StampControlNamesApproval => new HashSet<string>()
        {
            DepartmentApproval,
            ResponsiblePerson,
            Date,            
        };
    }
}
