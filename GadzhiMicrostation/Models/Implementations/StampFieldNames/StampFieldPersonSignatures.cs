using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Поля штампа с ответственным лицом и подписью
    /// </summary>
    public static class StampFieldPersonSignatures
    {
        /// <summary>
        /// Разработчик
        /// </summary>
        public static StampFieldPersonSignature DeveloperPerson =>
            new StampFieldPersonSignature("G_C_ROW1_1", "G_C_NAME1_1", "G_E_DATE_1");

        /// <summary>
        /// Начальник группы
        /// </summary>
        public static StampFieldPersonSignature HeadLeaderPerson =>
            new StampFieldPersonSignature("G_C_ROW2_1", "G_C_NAME2_1", "G_E_DATE_2");

        /// <summary>
        /// Начальник сектора
        /// </summary>
        public static StampFieldPersonSignature SectorLeaderPerson =>
            new StampFieldPersonSignature("G_C_ROW3_1", "G_C_NAME2_2", "G_E_DATE_3");

        /// <summary>
        /// Начальник отдела 
        /// </summary>
        public static StampFieldPersonSignature DepartmentLeaderPerson =>
            new StampFieldPersonSignature("G_C_ROW4_1", "G_C_NAME2_3", "G_E_DATE_4");

        /// <summary>
        /// Нормоконтроль
        /// </summary>
        public static StampFieldPersonSignature NormControlPerson =>
            new StampFieldPersonSignature("G_C_ROW5_1", "G_C_NAME2_4", "G_E_DATE_5");

        /// <summary>
        /// ГИП
        /// </summary>
        public static StampFieldPersonSignature GipPerson =>
            new StampFieldPersonSignature("G_C_ROW5_2", "G_C_NAME2_5", "G_E_DATE_6");


        /// <summary>
        /// Список строк с ответственным лицом и подписью
        /// </summary>
        public static HashSet<StampFieldPersonSignature> GetStampRowPersonSignatures() =>
            new HashSet<StampFieldPersonSignature>()
                {
                    DeveloperPerson,
                    HeadLeaderPerson,
                    SectorLeaderPerson,
                    DepartmentLeaderPerson,
                    NormControlPerson,
                    GipPerson
                };

        /// <summary>
        /// Список всех полей с ответственным лицом и подписью
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsPersonSignatures() =>
            new HashSet<StampFieldBase>(GetStampRowPersonSignatures().
                                        SelectMany(rowPerson => rowPerson.StampPersonSignatureFields));

        /// <summary>
        /// Список полей с типом действия
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsActionType() =>
            new HashSet<StampFieldBase>(GetStampRowPersonSignatures().
                                        Select(rowPerson => rowPerson.ActionType));

        /// <summary>
        /// Список полей с ответственным лицом
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsResponsiblePerson() =>
            new HashSet<StampFieldBase>(GetStampRowPersonSignatures().
                                        Select(rowPerson => rowPerson.ResponsiblePerson));

        /// <summary>
        /// Список полей с датой
        /// </summary>
        public static HashSet<StampFieldBase> GetFieldsDateSignature() =>
            new HashSet<StampFieldBase>(GetStampRowPersonSignatures().
                                        Select(rowPerson => rowPerson.DateSignature));
    }
}
