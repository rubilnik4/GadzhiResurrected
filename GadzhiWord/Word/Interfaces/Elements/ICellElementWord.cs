using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент ячейка. Базовый вариант
    /// </summary>
    public interface ICellElement
    {
        /// <summary>
        /// Текст ячейки
        /// </summary>
        string Text { get; }

        /// <summary>
        /// родительский элемент строка
        /// </summary>
        IRowElement RowElementWord { get; }

        /// <summary>
        /// Номер строки
        /// </summary>
        int RowIndex { get; }

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
