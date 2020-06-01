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

        /// <summary>
        /// Вставить данные из буфера
        /// </summary>
        void PasteFromClipBoard();

        /// <summary>
        /// Перейти на новую строку после последней ячейки
        /// </summary>
        void ToEndNewRow();
    }
}
