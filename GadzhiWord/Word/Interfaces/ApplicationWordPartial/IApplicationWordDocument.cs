using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.ApplicationWordPartial
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public interface IApplicationWordDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        IDocumentWord OpenDocument(string filePath);

        /// <summary>
        /// Сохранить документ
        /// </summary>
        void SaveDocument(string filePath);

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        void CreatePdfFile(string filePath);
    }
}
