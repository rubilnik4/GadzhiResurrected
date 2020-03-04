using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Поля штампа с согласованием
    /// </summary>
    public static class StampApprovals
    {  
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
        public static HashSet<StampBaseField> GetStampControlNamesApprovals()
        {
            var stampControlNamesApprovals = new HashSet<StampBaseField>();

            stampControlNamesApprovals.UnionWith(StampApprovalsFirst.StampControlNamesApproval);
            stampControlNamesApprovals.UnionWith(StampApprovalsSecond.StampControlNamesApproval);
            stampControlNamesApprovals.UnionWith(StampApprovalsThird.StampControlNamesApproval);

            return stampControlNamesApprovals;
        }

        /// <summary>
        /// Список строк с согласующим лицом и подписью
        /// </summary>
        public static HashSet<StampApproval> GetStampRowApprovalSignatures()
        {
            return new HashSet<StampApproval>()
                {
                    StampApprovalsFirst,
                    StampApprovalsSecond,
                    StampApprovalsThird,                   
                };
        }
    }
}
