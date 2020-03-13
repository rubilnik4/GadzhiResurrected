﻿using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>
    public class StampField : IStampField
    {
        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        public ICellElement CellElementStamp { get; }

        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        public StampFieldType StampFieldType { get; }

        public StampField(ICellElement cellElementStamp)
        {
            CellElementStamp = cellElementStamp;
            StampFieldType = CheckFieldType.GetStampFieldType(cellElementStamp?.Text);
        }

        /// <summary>
        /// Родительский элемент строка
        /// </summary>
        public IRowElement RowElementStamp => CellElementStamp.RowElement;
    }
}
