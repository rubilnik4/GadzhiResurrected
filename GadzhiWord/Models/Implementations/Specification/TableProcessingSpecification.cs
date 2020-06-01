using GadzhiWord.Word.Interfaces.Excel.Elements;

namespace GadzhiWord.Models.Implementations.Specification
{
    /// <summary>
    /// Обработка таблицы спецификации
    /// </summary>
    public static class TableProcessingSpecification
    {
        /// <summary>
        /// Установить ширину колонок
        /// </summary>
        public static void SetColumnsWidth(ISheetExcel sheetExcel)
        {
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.POSITION, 7.65f);
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.NAME, 54f);
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.MARKING, 25f);
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.CODE, 18f);
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.SUPPLIER, 7.56f);
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.UNIT, 7.56f);
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.WEIGHT, 9.67f);
            sheetExcel.ChangeColumnWidth(ColumnIndexesSpecification.NOTE, 16.11f);
        }
    }
}