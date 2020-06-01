using System;

namespace GadzhiWord.Word.Implementations.Excel.Helpers
{
    public static class ColumnNamesExcel
    {
        /// <summary>
        /// Получить индекс колонки по имени
        /// </summary>
        public static int GetExcelColumnNumber(string columnName)
        {
            if (String.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));

            columnName = columnName.ToUpperInvariant();

            var sum = 0;
            foreach (char ch in columnName)
            {
                if (Char.IsDigit(ch)) throw new ArgumentNullException("Invalid column name parameter on character " + ch);

                sum *= 26;
                sum += (ch - 'A' + 1);
            }

            return sum;
        }

        /// <summary>
        /// Получить имя колонки по индексу
        /// </summary>
        public static string GetExcelColumnName(int columnIndex)
        {
            if (columnIndex < 0) throw new IndexOutOfRangeException(nameof(columnIndex));

            int dividend = columnIndex + 1;
            var columnName = String.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// Получить имя ячейки по индексам
        /// </summary>
        public static string GetCellNameByIndexes(int columnIndex, int rowIndex)
        {
            if (columnIndex < 0) throw new IndexOutOfRangeException(nameof(columnIndex));
            if (rowIndex < 0) throw new IndexOutOfRangeException(nameof(rowIndex));

            return GetExcelColumnName(columnIndex) + rowIndex;
        }
    }
}