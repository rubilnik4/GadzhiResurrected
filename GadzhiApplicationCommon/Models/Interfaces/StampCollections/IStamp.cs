﻿using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public interface IStamp
    {
        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        StampType StampType { get; }

        /// <summary>
        /// Формат
        /// </summary>
        string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        OrientationType Orientation { get; }

        /// <summary>
        /// Сжать поля
        /// </summary>
        Unit CompressFieldsRanges();

        /// <summary>
        /// Вставить подписи
        /// </summary>
        IResultApplication InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        void DeleteSignatures();
    }
}
