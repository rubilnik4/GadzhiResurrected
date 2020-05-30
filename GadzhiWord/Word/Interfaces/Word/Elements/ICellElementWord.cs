namespace GadzhiWord.Word.Interfaces.Word.Elements
{
    /// <summary>
    /// Элемент ячейка. Базовый вариант
    /// </summary>
    public interface ICellElementWord
    {
        /// <summary>
        /// Номер строки
        /// </summary>
        int RowIndex { get; }

        /// <summary>
        /// Номер колонки
        /// </summary>
        int ColumnIndex { get; }

        /// <summary>
        /// Текст ячейки
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Присутствует ли картинка в ячейке
        /// </summary>
        bool HasPicture { get; }

        /// <summary>
        /// Вставить подпись
        /// </summary>
        void InsertPicture(string filePath);

        /// <summary>
        /// Удалить все подписи
        /// </summary>
        void DeleteAllPictures();
    }
}
