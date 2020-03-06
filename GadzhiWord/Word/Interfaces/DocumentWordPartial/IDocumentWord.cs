using GadzhiWord.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.DocumentWordPartial
{
    /// <summary>
    /// Документ Word
    /// </summary>
    public interface IDocumentWord: IDocumentWordElements, IDocumentWordStamp
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        string FullName { get; }

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

        /// <summary>
        /// Найти все доступные штампы на всех листах. Начать обработку каждого из них
        /// </summary>       
        IEnumerable<IFileDataSourceServerWord> CreatePdfInDocument(string filePath);
    }
}
