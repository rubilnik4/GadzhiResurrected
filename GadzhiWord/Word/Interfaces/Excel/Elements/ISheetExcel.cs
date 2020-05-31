namespace GadzhiWord.Word.Interfaces.Excel.Elements
{
    /// <summary>
    /// Лист приложения Excel
    /// </summary>
    public interface ISheetExcel
    {
        /// <summary>
        /// Изменить ширину колонки
        /// </summary>
        void ChangeColumnWidth(int columnIndex, float width);
    }
}
