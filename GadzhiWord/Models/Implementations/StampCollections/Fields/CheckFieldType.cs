using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Extensions.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Interfaces.Word.Elements;
using static GadzhiWord.Models.Implementations.StampCollections.AdditionalSettingsWord;

namespace GadzhiWord.Models.Implementations.StampCollections.Fields
{
    /// <summary>
    /// Класс для определения типа поля
    /// </summary>
    public static class CheckFieldType
    {
        /// <summary>
        /// Определить тип поля штампа
        /// </summary>
        public static StampFieldType GetStampFieldType(ICellElementWord cellElement, ITableElementWord stampTable) =>
            cellElement switch
            {
                _ when IsFieldFullCode(cellElement) => StampFieldType.FullRow,
                _ when IsFieldCurrentSheet(cellElement, stampTable) => StampFieldType.CurrentSheet,
                _ when IsFieldPersonSignature(cellElement) => StampFieldType.PersonSignature,
                _ when IsFieldChangeSignature(cellElement, stampTable) => StampFieldType.ChangeSignature,
                _ => StampFieldType.Unknown
            };

        /// <summary>
        /// Находится ли поле в строке с изменениями
        /// </summary>        
        public static bool IsFieldChangeSignature(ICellElementWord cellElement, ITableElementWord stampTable) =>
            cellElement?.ColumnIndex == 0 &&
            stampTable?.RowsElementWord?.
            Any(row => row.Index > cellElement.RowIndex &&
                       stampTable.HasCellElement(cellElement.RowIndex, cellElement.ColumnIndex) &&
                       IsFieldChangeHeader(stampTable.RowsElementWord[row.Index].CellsElement[cellElement.ColumnIndex].Text)) == true;

        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью
        /// </summary>        
        public static bool IsFieldPersonSignature(ICellElementWord cellElement) =>
            cellElement?.
                Text.PrepareCellTextToCompare().
                Map(cellText => AdditionalSettingsWord.MarkersActionType.MarkerContain(cellText))
            ?? false;

        /// <summary>
        /// Находится ли поле в строке заголовком изменений. Обработка входной строки
        /// </summary>        
        public static bool IsFieldChangeHeader(string cellText) =>
            AdditionalSettingsWord.MarkersChangeHeader.MarkerContain(cellText.PrepareCellTextToCompare());

        /// <summary>
        /// Является ли поле шифром
        /// </summary>
        public static bool IsFieldFullCode(ICellElementWord cellElement) =>
            cellElement.RowIndex == 0 &&
            !String.IsNullOrWhiteSpace(cellElement.Text) &&
            cellElement.Text.Length >= 4 &&
            Int32.TryParse(cellElement.Text.Substring(0, 4), out _);

        /// <summary>
        /// Является ли поле номером текущего листа
        /// </summary>
        public static bool IsFieldCurrentSheet(ICellElementWord cellElement, ITableElementWord stampTable) =>
            cellElement.RowIndex >= 1 &&
            !String.IsNullOrWhiteSpace(cellElement.Text) &&
            Int32.TryParse(cellElement.Text, out _) &&
            cellElement.ColumnIndex == stampTable.RowsElementWord[cellElement.RowIndex].CellsElement.Count - 2;

        /// <summary>
        /// Определить тип отдела по типу действия
        /// </summary>
        public static PersonDepartmentType GetDepartmentType(string actionType) =>
            actionType switch
            {
                _ when MarkersActionTypeDepartment.MarkerContain(actionType) => PersonDepartmentType.Department,
                _ when MarkersActionTypeChief.MarkerContain(actionType) => PersonDepartmentType.ChiefProject,
                _ => PersonDepartmentType.Undefined,
            };
    }
}
