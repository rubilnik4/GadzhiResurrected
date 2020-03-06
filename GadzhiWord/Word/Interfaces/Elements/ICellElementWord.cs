using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент ячейка
    /// </summary>
    public interface ICellElementWord
    {
        /// <summary>
        /// Текст ячейки
        /// </summary>
        string Text { get; }

        /// <summary>
        /// родительский элемент строка
        /// </summary>
        IRowElementWord RowElementWord { get; }

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
