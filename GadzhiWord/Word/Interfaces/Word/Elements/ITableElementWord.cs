using System.Collections.Generic;
using GadzhiWord.Word.Implementations.Word.Enums;

namespace GadzhiWord.Word.Interfaces.Word.Elements
{
    /// <summary>
    /// Элемент таблица. Базовый вариант
    /// </summary>
    public interface ITableElementWord: IOwnerWord
    {
        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        IReadOnlyList<IRowElementWord> RowsElementWord { get; }

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        IEnumerable<ICellElementWord> CellsElementWord { get; }

        /// <summary>
        /// Количество строк в начальной таблице Word без обертки
        /// </summary>
        int RowsCountInitial { get; }

        /// <summary>
        /// Количество колонок в начальной таблице Word без обертки
        /// </summary>
        int ColumnsCountInitial { get; }

        /// <summary>
        /// Тип высоты строк
        /// </summary>
        HeightRowsType HeightRowsType { get; }

        /// <summary>
        /// Высота строк
        /// </summary>
        decimal HeightRows { get; }

        /// <summary>
        /// Проверить существование ячейки 
        /// </summary>
        bool HasCellElement(int rowIndex, int columnIndex);

        /// <summary>
        /// Скопировать таблицу в буфер
        /// </summary>
        void CopyToClipBoard();

        /// <summary>
        /// Установить автоподбор ширины таблицы
        /// </summary>
        void SetAutoFit(bool autoFit);

        /// <summary>
        /// Установить тип высоты строк
        /// </summary>
        void SetHeightRowsType(HeightRowsType heightRowsType);

        /// <summary>
        /// Установить высоту строк
        /// </summary>
        void SetHeightRows(decimal height);
    }
}
