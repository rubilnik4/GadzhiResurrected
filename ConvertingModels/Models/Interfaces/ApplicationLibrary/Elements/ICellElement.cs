using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Word.Interfaces.Elements
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
        IRowElement RowElement { get; }

        /// <summary>
        /// Номер строки
        /// </summary>
        int RowIndex { get; }

        /// <summary>
        /// Вставить картинку
        /// </summary>
        void InsertPicture(string filePath);

        /// <summary>
        /// Удалить все картинки
        /// </summary>
        void DeleteAllPictures();
    }
}
