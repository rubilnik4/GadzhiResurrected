using GadzhiApplicationCommon.Models.Enums;
using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Класс для определения типа поля
    /// </summary>
    public static class CheckFieldType
    {
        /// <summary>
        /// Опередлить тип поля штампа
        /// </summary>
        public static StampFieldType GetStampFieldType(ICellElement cellElement, ITableElement stampTable)
        {
            StampFieldType stampFieldType = StampFieldType.Unknown;

            string cellText = StringAdditionalExtensions.PrepareCellTextToCompare(cellElement?.Text);
            if (IsFieldPersonSignature(cellText))
            {
                stampFieldType = StampFieldType.PersonSignature;
            }
            else if (IsFieldChangeSignature(cellElement, stampTable))
            {
                stampFieldType = StampFieldType.ChangeSignature;
            }

            return stampFieldType;
        }

        /// <summary>
        /// Находится ли поле в строке с изменениями
        /// </summary>        
        public static bool IsFieldChangeSignature(ICellElement cellElement, ITableElement stampTable)
        {
            (int rowIndex, int columnIndex) = (cellElement?.RowIndex ?? throw new ArgumentNullException(nameof(cellElement)),
                                                       cellElement?.ColumnIndex ?? throw new ArgumentNullException(nameof(cellElement)));
            if (columnIndex == 0)
            {
                stampTable?.RowsElementWord?.
                            Any(row => row.Index >= rowIndex && stampTable?.HasCellElement(rowIndex, columnIndex) == true &&
                                IsFieldChangeHeader(stampTable.RowsElementWord[row.Index].CellsElementWord[columnIndex].Text));
            }
            return false;
        }

        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью
        /// </summary>        
        public static bool IsFieldPersonSignature(string cellText, bool needToPrepareText = false) =>
            StampSettingsWord.MarkersActionTypeSignature.MarkerContain(needToPrepareText ?
                                                                       cellText.PrepareCellTextToCompare() :
                                                                       cellText);


        /// <summary>
        /// Находится ли поле в строке заголовком изменений. Обработка входной строки
        /// </summary>        
        public static bool IsFieldChangeHeader(string cellText) =>
            StampSettingsWord.MarkersChangeHeader.MarkerContain(cellText.PrepareCellTextToCompare());
    }
}
