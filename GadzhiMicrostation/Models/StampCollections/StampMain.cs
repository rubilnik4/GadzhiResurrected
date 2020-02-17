using GadzhiMicrostation.Extensions.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        /// Стадия проекта
        /// </summary>
        public static string ProjectStage => "G_E_STADY_1";

        /// <summary>
        /// Текущий лист
        /// </summary>
        public static string CurrentSheet => "G_E_LISTNUM_1";

        /// <summary>
        /// Всего листов
        /// </summary>
        public static string TotalSheet => "G_E_LISTOV_1";

        /// <summary>
        /// Формат
        /// </summary>
        public static string Format => "Format";

        /// <summary>
        /// Список всех полей
        /// </summary>
        public static HashSet<string> StampMainFields
        {
            get
            {
                var stampFields = new HashSet<string>()
                {
                    FullCode,
                    ProjectName,
                    ObjectName,
                    SheetName,
                    ProjectStage,
                    CurrentSheet,
                    TotalSheet,
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
