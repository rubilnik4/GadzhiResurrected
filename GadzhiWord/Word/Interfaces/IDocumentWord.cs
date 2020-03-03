using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces
{
    /// <summary>
    /// Документ Word
    /// </summary>
    public interface IDocumentWord
    {
        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        bool IsDocumentValid { get; }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        void Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        void SaveAs(string filePath);

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        void Close() ;

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        void CloseWithSaving();       
    }
}
