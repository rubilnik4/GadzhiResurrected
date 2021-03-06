﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GadzhiApplicationCommon.Extensions.StringAdditional;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public static class StampFieldMain
    {
        /// <summary>
        /// Наименование проекта
        /// </summary>
        public static StampFieldBase ProjectName => new StampFieldBase("G_E_PRNAME_1");

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public static StampFieldBase ObjectName => new StampFieldBase("G_E_OBJNAME_1");

        /// <summary>
        /// Наименование листа
        /// </summary>
        public static StampFieldBase SheetName => new StampFieldBase("G_E_LISTNAME_1");

        /// <summary>
        /// Стадия проекта
        /// </summary>
        public static StampFieldBase ProjectStage => new StampFieldBase("G_E_STADY_1", isNeedCompress: false);

        /// <summary>
        /// Всего листов
        /// </summary>
        public static StampFieldBase TotalSheet => new StampFieldBase("G_E_LISTOV_1", isNeedCompress: false);

        /// <summary>
        /// Формат
        /// </summary>
        public static StampFieldBase PaperSize => new StampFieldBase("FORMAT", isNeedCompress: false);

        /// <summary>
        /// Список всех полей
        /// </summary>
        public static HashSet<StampFieldBase> GetStampMainFields() =>
                new HashSet<StampFieldBase>()
                {
                    ProjectName,
                    ObjectName,
                    SheetName,
                    ProjectStage,
                    TotalSheet,
                    PaperSize,
                };

        /// <summary>
        /// Список всех полей, включая базовые
        /// </summary>
        public static HashSet<StampFieldBase> GetStampMainAndBasicFields() =>
            new HashSet<StampFieldBase>(GetStampMainFields().
                                        Concat(StampFieldBasic.GetStampBasicFields()));

        /// <summary>
        /// Маркер подписи в атрибутах
        /// </summary>
        public static string SignatureAttributeMarker => "NameSig";

        /// <summary>
        /// Проверка, является ли поле указателем формата
        /// </summary>      
        public static bool IsFormatField(string fieldText) => !String.IsNullOrEmpty(fieldText) &&
                                                               fieldText.StartsWith("Формат", StringComparison.Ordinal);

        /// <summary>
        /// Получить формат штампа
        /// </summary>      
        public static string GetPaperSizeFromField(string fieldText) => fieldText?.TrimSubstring("Формат")?.Trim();

        /// <summary>
        /// Определить является ли строка названием ячейки штампа
        /// </summary>       
        public static bool IsStampName(string name)
        {
            string cellElementName = name?.ToUpper(CultureInfo.CurrentCulture)
                                     ?? String.Empty;

            return cellElementName.StartsWith("STAMP", StringComparison.Ordinal) &&
                   !cellElementName.Contains("STAMP_AUDIT") &&
                   !cellElementName.Contains("STAMP_ISM");
        }

        /// <summary>
        /// Является ли элемент вертикальным
        /// </summary>     
        public static bool IsControlVertical(string name) =>
            !String.IsNullOrEmpty(name) &&
            StampFieldApprovals.GetFieldsApprovalSignatures().
                                Where(field => field.IsVertical).
                                Select(field => field.Name).
                                Contains(name);
    }
}
