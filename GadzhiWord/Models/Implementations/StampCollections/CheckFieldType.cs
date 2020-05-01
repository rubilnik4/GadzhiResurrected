using GadzhiApplicationCommon.Models.Enums;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiWord.Extensions.StringAdditional;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Класс для определения типа поля
    /// </summary>
    public static class CheckFieldType
    {
        /// <summary>
        /// Определить тип поля штампа
        /// </summary>
        public static StampFieldType GetStampFieldType(ICellElement cellElement, ITableElement stampTable) =>
            cellElement switch
            {
                _ when IsFieldPersonSignature(cellElement) => StampFieldType.PersonSignature,
                _ when IsFieldChangeSignature(cellElement, stampTable) => StampFieldType.ChangeSignature,
                _ => StampFieldType.Unknown
            };

        /// <summary>
        /// Находится ли поле в строке с изменениями
        /// </summary>        
        public static bool IsFieldChangeSignature(ICellElement cellElement, ITableElement stampTable) =>
            cellElement?.ColumnIndex == 0 &&
            stampTable?.RowsElementWord?.
                        Any(row => row.Index >= cellElement.RowIndex && 
                                   stampTable.HasCellElement(cellElement.RowIndex, cellElement.ColumnIndex) &&
                                   IsFieldChangeHeader(stampTable.RowsElementWord[row.Index].CellsElementWord[cellElement.ColumnIndex].Text)) == true;
        
        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью
        /// </summary>        
        public static bool IsFieldPersonSignature(ICellElement cellElement) =>
            cellElement?.
                Text.PrepareCellTextToCompare().
                Map(cellText => StampSettingsWord.MarkersActionTypeSignature.MarkerContain(cellText))
            ?? false;

        /// <summary>
        /// Находится ли поле в строке заголовком изменений. Обработка входной строки
        /// </summary>        
        public static bool IsFieldChangeHeader(string cellText) =>
            StampSettingsWord.MarkersChangeHeader.MarkerContain(cellText.PrepareCellTextToCompare());
    }
}
