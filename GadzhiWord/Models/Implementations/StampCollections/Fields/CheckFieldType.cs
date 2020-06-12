using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections.Signatures;
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
        /// Является ли таблица списком исполнителей для согласования тех требований
        /// </summary>
        public static bool IsTableApprovalPerformers(ITableElementWord table) =>
            table?.RowsCountInitial <= ApprovalPerformersSignatureWord.MAX_ROWS_COUNT && 
            table.ColumnsCountInitial == ApprovalPerformersSignatureWord.FIELDS_COUNT &&
            table.HasCellElement(0, 0) &&
            StampMarkersWord.MarkersApprovalPerformanceTable.MarkerContain(table.RowsElementWord[0].CellsElement[0].TextNoSpaces);

        /// <summary>
        /// Является ли таблица списком согласований с директорами для тех требований
        /// </summary>
        public static bool IsTableApprovalChief(ITableElementWord table) =>
            table?.RowsCountInitial <= ApprovalChiefSignatureWord.MAX_ROWS_COUNT &&
            table.ColumnsCountInitial == ApprovalChiefSignatureWord.FIELDS_COUNT &&
            table.HasCellElement(0, 0) &&
            StampMarkersWord.MarkersApprovalChiefTable.MarkerContain(table.RowsElementWord[0].CellsElement[0].TextNoSpaces);

        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью для полного штампа
        /// </summary>        
        public static bool IsFieldPersonSignatureFull(ICellElementWord cellElement, ITableElementWord stampTable) =>
            StampMarkersWord.MarkersActionType.MarkerContain(cellElement.TextNoSpaces) &&
            stampTable?.RowsElementWord?.
            Any(row => row.Index < cellElement.RowIndex &&
                       stampTable.HasCellElement(row.Index, cellElement.ColumnIndex) &&
                       IsFieldChangeHeader(stampTable.RowsElementWord[row.Index].CellsElement[cellElement.ColumnIndex].TextNoSpaces)) == true;

        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью для штампа с изменениями
        /// </summary>        
        public static bool IsFieldPersonSignatureChange(ICellElementWord cellElement) =>
            StampMarkersWord.MarkersActionTypeChangeNotice.MarkerContain(cellElement.TextNoSpaces);

        /// <summary>
        /// Находится ли поле в строке с изменениями
        /// </summary>        
        public static bool IsFieldChangeSignature(ICellElementWord cellElement, ITableElementWord stampTable) =>
            Int32.TryParse(cellElement.TextNoSpaces, out _) &&
            stampTable?.RowsElementWord?.
            Any(row => row.Index > cellElement.RowIndex &&
                       stampTable.HasCellElement(row.Index, cellElement.ColumnIndex) &&
                       IsFieldChangeHeader(stampTable.RowsElementWord[row.Index].CellsElement[cellElement.ColumnIndex].TextNoSpaces)) == true;

        /// <summary>
        /// Находится ли поле в строке с согласованиями
        /// </summary>        
        public static bool IsFieldApprovalChangeSignature(ICellElementWord cellElement, ITableElementWord stampTable) =>
            StampMarkersWord.MarkersApprovalChange.MarkerContain(cellElement.TextNoSpaces) &&
            stampTable?.RowsElementWord?.
            Any(row => row.Index < cellElement.RowIndex &&
                       stampTable.HasCellElement(row.Index, cellElement.ColumnIndex) &&
                       StampMarkersWord.MarkersApprovalChangeStamp.MarkerContain(row.CellsElement[cellElement.ColumnIndex].TextNoSpaces)) == true;

        /// <summary>
        /// Находится ли поле в строке заголовком изменений. Обработка входной строки
        /// </summary>        
        public static bool IsFieldChangeHeader(string cellText) =>
            StampMarkersWord.MarkersChangeHeader.MarkerContain(cellText.RemoveSpacesAndArtefacts());

        /// <summary>
        /// Является ли поле шифром
        /// </summary>
        public static bool IsFieldFullCode(ICellElementWord cellElement, ITableElementWord stampTable) =>
            (cellElement.RowIndex == 0 ||
             cellElement.ColumnIndex == stampTable.RowsElementWord[cellElement.RowIndex].CellsElement.Count - 1) &&
            !String.IsNullOrWhiteSpace(cellElement.TextNoSpaces) &&
            cellElement.TextNoSpaces.Length >= 4 &&
            Int32.TryParse(cellElement.TextNoSpaces.Substring(0, 4), out _);

        /// <summary>
        /// Является ли поле номером текущего листа
        /// </summary>
        public static bool IsFieldCurrentSheet(ICellElementWord cellElement, ITableElementWord stampTable) =>
            cellElement.RowIndex >= 1 &&
            !String.IsNullOrWhiteSpace(cellElement.TextNoSpaces) &&
            Int32.TryParse(cellElement.TextNoSpaces, out _) &&
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
