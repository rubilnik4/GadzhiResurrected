using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Collections
{
    /// <summary>
    /// Поля составляющие штамп
    /// </summary>
    public static class StampElement
    {
        /// <summary>
        /// Основной шифр
        /// </summary>
        public static string FullCode => "G_E_FULLCODE_1";

        /// <summary>
        /// Наименование проекта
        /// </summary>
        public static string ProjectName => "G_E_PRNAME_1";

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public static string ObjectName => "G_E_OBJNAME_1";

        /// <summary>
        /// Наименование листа
        /// </summary>
        public static string SheetName => "G_E_LISTNAME_1";

        /// <summary>
        /// Разработчик
        /// </summary>
        public static StampPersonSignature DeveloperPerson => new StampPersonSignature("G_C_ROW1_1",
                                                                                       "G_C_NAME1_1",
                                                                                       "UNKNOWN",
                                                                                       "G_E_DATE_1");

        /// <summary>
        /// Начальник группы
        /// </summary>
        public static StampPersonSignature HeadLeaderPerson => new StampPersonSignature("G_C_ROW2_1",
                                                                                        "G_C_NAME2_1",
                                                                                        "UNKNOWN",
                                                                                        "G_E_DATE_2");

        /// <summary>
        /// Начальник сектора
        /// </summary>
        public static StampPersonSignature SectorLeaderPerson => new StampPersonSignature("G_C_ROW3_1",
                                                                                          "G_C_NAME2_2",
                                                                                          "UNKNOWN",
                                                                                          "G_E_DATE_3");

        /// <summary>
        /// Начальник отдела
        /// </summary>
        public static StampPersonSignature DepartmentLeaderPerson => new StampPersonSignature("G_C_ROW4_1",
                                                                                              "G_C_NAME2_3",
                                                                                              "UNKNOWN",
                                                                                              "G_E_DATE_4");

        /// <summary>
        /// Нормоконтроль
        /// </summary>
        public static StampPersonSignature NormControlPerson => new StampPersonSignature("G_C_ROW5_1",
                                                                                         "G_C_NAME2_4",
                                                                                         "UNKNOWN",
                                                                                         "G_E_DATE_5");

        /// <summary>
        /// ГИП
        /// </summary>
        public static StampPersonSignature GipPerson => new StampPersonSignature("G_C_ROW5_2",
                                                                                 "G_C_NAME2_5",
                                                                                 "UNKNOWN",
                                                                                 "G_E_DATE_6");

        /// <summary>
        /// Изменение 1
        /// </summary>
        public static StampChanges StampChangesFirst => new StampChanges("G_E_S_41",
                                                                         "G_E_S_42",
                                                                         "G_E_S_43",
                                                                         "G_E_S_44",
                                                                         "UNKNOWN",
                                                                         "G_E_S_46");

        /// <summary>
        /// Изменение 2
        /// </summary>
        public static StampChanges StampChangesSecond => new StampChanges("G_E_S_31",
                                                                         "G_E_S_32",
                                                                         "G_E_S_33",
                                                                         "G_E_S_34",
                                                                         "UNKNOWN",
                                                                         "G_E_S_36");

        /// <summary>
        /// Изменение 3
        /// </summary>
        public static StampChanges StampChangesThird => new StampChanges("G_E_S_21",
                                                                         "G_E_S_22",
                                                                         "G_E_S_23",
                                                                         "G_E_S_24",
                                                                         "UNKNOWN",
                                                                         "G_E_S_26");

        /// <summary>
        /// Изменение 4
        /// </summary>
        public static StampChanges StampChangesForth => new StampChanges("G_E_S_11",
                                                                         "G_E_S_12",
                                                                         "G_E_S_13",
                                                                         "G_E_S_14",
                                                                         "UNKNOWN",
                                                                         "G_E_S_16");

        /// <summary>
        /// Список всех полей
        /// </summary>
        private static HashSet<string> _stampFields
        {
            get
            {
                var stampFields = new HashSet<string>()
                {
                    FullCode,
                    ProjectName,
                    ObjectName,
                    SheetName,
                };
                stampFields.UnionWith(DeveloperPerson.StampPersonSignatureFields);
                stampFields.UnionWith(HeadLeaderPerson.StampPersonSignatureFields);
                stampFields.UnionWith(SectorLeaderPerson.StampPersonSignatureFields);
                stampFields.UnionWith(DepartmentLeaderPerson.StampPersonSignatureFields);
                stampFields.UnionWith(NormControlPerson.StampPersonSignatureFields);
                stampFields.UnionWith(GipPerson.StampPersonSignatureFields);

                stampFields.UnionWith(StampChangesFirst.StampChangesFields);
                stampFields.UnionWith(StampChangesSecond.StampChangesFields);
                stampFields.UnionWith(StampChangesThird.StampChangesFields);
                stampFields.UnionWith(StampChangesForth.StampChangesFields);

                return stampFields;
            }
        }


        /// <summary>
        /// Содержится ли поле в списке Штампа
        /// </summary>       
        public static bool ContainField(string findField)
        {
            findField = GetNameInCorrectCase(findField);
            if (!String.IsNullOrEmpty(findField))
            {
                if (_stampFields.Contains(findField))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получить имя поля в корректном написании
        /// </summary>
        public static string GetNameInCorrectCase(string field)
        {
            return field?.Trim()?.ToUpper();
        }
    }
}
