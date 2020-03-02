using GadzhiMicrostation.Extentions.StringAdditional;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public static class StampMain
    {
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
        public static StampBaseField PaperSize => new StampBaseField("FORMAT",
                                                                  isNeedCompress: false);

        /// <summary>
        /// Список всех полей
        /// </summary>
        public static HashSet<StampBaseField> GetStampMainFields() =>
                new HashSet<StampBaseField>()
                {
                    FullCode,
                    ProjectName,
                    ObjectName,
                    SheetName,
                    ProjectStage,
                    CurrentSheet,
                    TotalSheet,
                    PaperSize,
                };

        /// <summary>
        /// Маркер подписи в аттритубах
        /// </summary>
        public static string SignatureAttributeMarker => "NameSig";

        /// <summary>
        /// Проверка, является ли поле указателем формата
        /// </summary>      
        public static bool IsFormatField(string fieldText)
        {
            if (!String.IsNullOrEmpty(fieldText) &&
                fieldText.StartsWith("Формат", StringComparison.Ordinal))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получить формат штампа
        /// </summary>      
        public static string GetPaperSizeFromField(string fieldText) =>
             fieldText?.TrimSubstring("Формат")?.Trim();

        /// <summary>
        /// Определить яляется ли строка названием ячейки штампа
        /// </summary>       
        public static bool IsStampName(string name)
        {
            string cellElementName = name?.ToUpper(CultureInfo.CurrentCulture);

            return cellElementName.StartsWith("STAMP", StringComparison.Ordinal) &&
                   !cellElementName.Contains("STAMP_AUDIT") &&
                   !cellElementName.Contains("STAMP_ISM");
        }
    }
}
