using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Поля штампа с согласованием
    /// </summary>
    public class StampApprovals
    {
        public StampApprovals()
        {
        }

        /// <summary>
        /// Согласование 1
        /// </summary>
        public static StampApproval StampApprovalsFirst =>
            new StampApproval("V_C_DEP1_1",
                              "V_C_NAME1_1",
                              "V_E_DATE_35");

        /// <summary>
        /// Согласование 2
        /// </summary>
        public static StampApproval StampApprovalsSecond =>
            new StampApproval("V_C_DEP1_2",
                              "V_C_NAME1_2",
                              "V_E_DATE_25");

        /// <summary>
        /// Согласование 3
        /// </summary>
        public static StampApproval StampApprovalsThird =>
            new StampApproval("V_C_DEP1_3",
                              "V_C_NAME1_3",
                              "V_E_DATE_15");


        /// <summary>
        /// Список всех полей с изменениями
        /// </summary>
        public HashSet<string> StampControlNamesApprovals
        {
            get
            {
                var stampControlNamesApprovals = new HashSet<string>();

                stampControlNamesApprovals.UnionWith(StampApprovalsFirst.StampControlNamesApproval);
                stampControlNamesApprovals.UnionWith(StampApprovalsSecond.StampControlNamesApproval);
                stampControlNamesApprovals.UnionWith(StampApprovalsThird.StampControlNamesApproval);               

                return stampControlNamesApprovals;
            }
        }
    }
}
