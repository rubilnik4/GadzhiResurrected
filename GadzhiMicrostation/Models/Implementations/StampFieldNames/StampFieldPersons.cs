using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Поля штампа с ответственным лицом и подписью
    /// </summary>
    public static class StampFieldPersons
    {
        /// <summary>
        /// Разработчик
        /// </summary>
        public static StampFieldPerson DeveloperPerson =>
            new StampFieldPerson("G_C_ROW1_1", "G_C_NAME1_1", "G_E_DATE_1");

        /// <summary>
        /// Начальник группы
        /// </summary>
        public static StampFieldPerson HeadLeaderPerson =>
            new StampFieldPerson("G_C_ROW2_1", "G_C_NAME2_1", "G_E_DATE_2");

        /// <summary>
        /// Начальник сектора
        /// </summary>
        public static StampFieldPerson SectorLeaderPerson =>
            new StampFieldPerson("G_C_ROW3_1", "G_C_NAME2_2", "G_E_DATE_3");

        /// <summary>
        /// Начальник отдела 
        /// </summary>
        public static StampFieldPerson DepartmentLeaderPerson =>
            new StampFieldPerson("G_C_ROW4_1", "G_C_NAME2_3", "G_E_DATE_4");

        /// <summary>
        /// Нормоконтроль
        /// </summary>
        public static StampFieldPerson NormControlPerson =>
            new StampFieldPerson("G_C_ROW5_1", "G_C_NAME2_4", "G_E_DATE_5");

        /// <summary>
        /// ГИП
        /// </summary>
        public static StampFieldPerson GipPerson =>
            new StampFieldPerson("G_C_ROW5_2", "G_C_NAME2_5", "G_E_DATE_6");


        /// <summary>
        /// Список строк с ответственным лицом и подписью
        /// </summary>
        public static HashSet<StampFieldPerson> GetStampRowPersonSignatures() =>
            new HashSet<StampFieldPerson>()
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
                                        SelectMany(rowPerson => rowPerson.StampSignatureFields));

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
