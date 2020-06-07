using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Extensions.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Interfaces.Word.Elements;

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
                _ when IsFieldFullCode(cellElement, stampTable) => StampFieldType.FullRow,
                _ when IsFieldCurrentSheet(cellElement, stampTable) => StampFieldType.CurrentSheet,
                _ when IsFieldPersonSignatureFull(cellElement, stampTable) => StampFieldType.PersonSignature,
                _ when IsFieldPersonSignatureChange(cellElement) => StampFieldType.PersonSignature,
                _ when IsFieldChangeSignature(cellElement, stampTable) => StampFieldType.ChangeSignature,
                _ when IsFieldApprovalChangeSignature(cellElement, stampTable) => StampFieldType.ApprovalChangeSignature,
                _ => StampFieldType.Unknown
            };

        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью для полного штампа
        /// </summary>        
        public static bool IsFieldPersonSignatureFull(ICellElementWord cellElement, ITableElementWord stampTable) =>
            StampMarkersWord.MarkersActionType.MarkerContain(cellElement.Text) &&
            stampTable?.RowsElementWord?.
            Any(row => row.Index < cellElement.RowIndex &&
                       stampTable.HasCellElement(row.Index, cellElement.ColumnIndex) &&
                       IsFieldChangeHeader(stampTable.RowsElementWord[row.Index].CellsElement[cellElement.ColumnIndex].Text)) == true;

        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью для штампа с изменениями
        /// </summary>        
        public static bool IsFieldPersonSignatureChange(ICellElementWord cellElement) =>
            StampMarkersWord.MarkersActionTypeChangeNotice.MarkerContain(cellElement.Text);

        /// <summary>
        /// Находится ли поле в строке с изменениями
        /// </summary>        
        public static bool IsFieldChangeSignature(ICellElementWord cellElement, ITableElementWord stampTable) =>
            Int32.TryParse(cellElement.Text, out _) &&
            stampTable?.RowsElementWord?.
            Any(row => row.Index > cellElement.RowIndex &&
                       stampTable.HasCellElement(row.Index, cellElement.ColumnIndex) &&
                       IsFieldChangeHeader(stampTable.RowsElementWord[row.Index].CellsElement[cellElement.ColumnIndex].Text)) == true;

        /// <summary>
        /// Находится ли поле в строке с согласованиями
        /// </summary>        
        public static bool IsFieldApprovalChangeSignature(ICellElementWord cellElement, ITableElementWord stampTable) =>
            StampMarkersWord.MarkersApprovalChange.MarkerContain(cellElement.Text) &&
            stampTable?.RowsElementWord?.
            Any(row => row.Index < cellElement.RowIndex &&
                       stampTable.HasCellElement(row.Index, cellElement.ColumnIndex) &&
                       StampMarkersWord.MarkersApprovalChangeStamp.MarkerContain(row.CellsElement[cellElement.ColumnIndex].Text)) == true;

        /// <summary>
        /// Находится ли поле в строке заголовком изменений. Обработка входной строки
        /// </summary>        
        public static bool IsFieldChangeHeader(string cellText) =>
            StampMarkersWord.MarkersChangeHeader.MarkerContain(cellText.PrepareCellTextToCompare());

        /// <summary>
        /// Является ли поле шифром
        /// </summary>
        public static bool IsFieldFullCode(ICellElementWord cellElement, ITableElementWord stampTable) =>
            (cellElement.RowIndex == 0 ||
             cellElement.ColumnIndex == stampTable.RowsElementWord[cellElement.RowIndex].CellsElement.Count - 1) &&
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
                _ when StampMarkersWord.MarkersActionTypeDepartment.MarkerContain(actionType) => PersonDepartmentType.Department,
                _ when StampMarkersWord.MarkersActionTypeChief.MarkerContain(actionType) => PersonDepartmentType.ChiefProject,
                _ => PersonDepartmentType.Undefined,
            };
    }
}
