using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Поля штампа с согласованием
    /// </summary>
    public static class StampFieldApprovals
    {
        /// <summary>
        /// Согласование 1
        /// </summary>
        public static StampFieldApproval StampApprovalsFirst =>
            new StampFieldApproval("V_C_DEP1_1", "V_C_NAME1_1", "V_E_DATE_35");

        /// <summary>
        /// Согласование 2
        /// </summary>
        public static StampFieldApproval StampApprovalsSecond =>
            new StampFieldApproval("V_C_DEP1_2", "V_C_NAME1_2", "V_E_DATE_25");

        /// <summary>
        /// Согласование 3
        /// </summary>
        public static StampFieldApproval StampApprovalsThird =>
            new StampFieldApproval("V_C_DEP1_3", "V_C_NAME1_3", "V_E_DATE_15");

        /// <summary>
        /// Список строк с согласующим лицом и подписью
        /// </summary>
        public static HashSet<StampFieldApproval> GetStampRowApprovalSignatures() =>
            new HashSet<StampFieldApproval>()
                {
                    StampApprovalsFirst,
                    StampApprovalsSecond,
                    StampApprovalsThird,
                };

        /// <summary>
        /// Список всех полей с изменениями
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsApprovalSignatures() =>
            new HashSet<StampFieldBase>(GetStampRowApprovalSignatures().
                                        SelectMany(rowApproval => rowApproval.StampApprovalSignatureFields));

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsDepartmentApproval() =>
            new HashSet<StampFieldBase>(GetStampRowApprovalSignatures().
                                        Select(rowApproval => rowApproval.DepartmentApproval));

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsResponsiblePerson() =>
            new HashSet<StampFieldBase>(GetStampRowApprovalSignatures().
                                        Select(rowApproval => rowApproval.ResponsiblePerson));

        /// <summary>
        /// Дата
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsDateSignature() =>
            new HashSet<StampFieldBase>(GetStampRowApprovalSignatures().
                                        Select(rowApproval => rowApproval.DateSignature));
    }
}
