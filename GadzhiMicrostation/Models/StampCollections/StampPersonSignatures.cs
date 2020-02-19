using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Поля штампа с ответсвенным лицом и подписью
    /// </summary>
    public class StampPersonSignatures
    {
        public StampPersonSignatures()
        {

        }

        /// <summary>
        /// Разработчик
        /// </summary>
        public static StampPersonSignature DeveloperPerson =>
            new StampPersonSignature("G_C_ROW1_1",
                                     "G_C_NAME1_1",
                                     "G_E_DATE_1");

        /// <summary>
        /// Начальник группы
        /// </summary>
        public static StampPersonSignature HeadLeaderPerson =>
            new StampPersonSignature("G_C_ROW2_1",
                                     "G_C_NAME2_1",
                                     "G_E_DATE_2");

        /// <summary>
        /// Начальник сектора
        /// </summary>
        public static StampPersonSignature SectorLeaderPerson =>
            new StampPersonSignature("G_C_ROW3_1",
                                     "G_C_NAME2_2",
                                     "G_E_DATE_3");

        /// <summary>
        /// Начальник отдела 
        /// </summary>
        public static StampPersonSignature DepartmentLeaderPerson =>
            new StampPersonSignature("G_C_ROW4_1",
                                     "G_C_NAME2_3",
                                     "G_E_DATE_4");

        /// <summary>
        /// Нормоконтроль
        /// </summary>
        public static StampPersonSignature NormControlPerson =>
            new StampPersonSignature("G_C_ROW5_1",
                                     "G_C_NAME2_4",
                                     "G_E_DATE_5");

        /// <summary>
        /// ГИП
        /// </summary>
        public static StampPersonSignature GipPerson => new
            StampPersonSignature("G_C_ROW5_2",
                                 "G_C_NAME2_5",
                                 "G_E_DATE_6");


        /// <summary>
        /// Список строк с ответсвенным лицом и подписью
        /// </summary>
        public HashSet<StampPersonSignature> StampRowPersonSignatures
        {
            get
            {
                var stampFields = new HashSet<StampPersonSignature>()
                {
                    DeveloperPerson,
                    HeadLeaderPerson,
                    SectorLeaderPerson,
                    DepartmentLeaderPerson,
                    NormControlPerson,
                    GipPerson
                };
                return stampFields;
            }
        }

        /// <summary>
        /// Список всех полей с ответсвенным лицом и подписью
        /// </summary>
        public HashSet<StampBaseField> StampFieldsPersonSignatures => 
            new HashSet<StampBaseField>(StampRowPersonSignatures?.
                                        SelectMany(rowPerson => rowPerson.StampPersonSignatureFields));


    }
}
