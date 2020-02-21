using GadzhiMicrostation.Extensions.String;
using System;
using System.Collections.Generic;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public class StampMain
    {
        public StampMain()
        {
        }

        /// <summary>
        /// Основной шифр
        /// </summary>
        public static StampBaseField FullCode => new StampBaseField("G_E_FULLCODE_1");

        /// <summary>
        /// Наименование проекта
        /// </summary>
        public static StampBaseField ProjectName => new StampBaseField("G_E_PRNAME_1");

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public static StampBaseField ObjectName => new StampBaseField("G_E_OBJNAME_1");

        /// <summary>
        /// Наименование листа
        /// </summary>
        public static StampBaseField SheetName => new StampBaseField("G_E_LISTNAME_1");

        /// <summary>
        /// Стадия проекта
        /// </summary>
        public static StampBaseField ProjectStage => new StampBaseField("G_E_STADY_1",
                                                                        isNeedCompress: false);

        /// <summary>
        /// Текущий лист
        /// </summary>
        public static StampBaseField CurrentSheet => new StampBaseField("G_E_LISTNUM_1",
                                                                        isNeedCompress: false);

        /// <summary>
        /// Всего листов
        /// </summary>
        public static StampBaseField TotalSheet => new StampBaseField("G_E_LISTOV_1",
                                                                      isNeedCompress: false);

        /// <summary>
        /// Формат
        /// </summary>
        public static StampBaseField Format => new StampBaseField("FORMAT",
                                                                  isNeedCompress: false);

        /// <summary>
        /// Список всех полей
        /// </summary>
        public static HashSet<StampBaseField> StampMainFields
        {
            get
            {
                var stampFields = new HashSet<StampBaseField>()
                {
                    FullCode,
                    ProjectName,
                    ObjectName,
                    SheetName,
                    ProjectStage,
                    CurrentSheet,
                    TotalSheet,
                    Format,
                };

                return stampFields;
            }
        }

        /// <summary>
        /// Проверка, является ли поле указателем формата
        /// </summary>      
        public static bool IsFormatField(string fieldText)
        {
            if (!String.IsNullOrEmpty(fieldText) &&
                fieldText.StartsWith("Формат"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получить формат штампа
        /// </summary>      
        public static string GetFormatFromField(string fieldText) =>
             fieldText.TrimSubstring("Формат").Trim();

        // {
        //string format = String.Empty;

        //if (!String.IsNullOrEmpty(fieldText))
        //{
        //    format = Regex.Match(fieldText, @"\d+").Value;
        //}

        //return format;
        // }
    }
}
