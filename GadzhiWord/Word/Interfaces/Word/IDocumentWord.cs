using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;

namespace GadzhiWord.Word.Interfaces.Word
{
    /// <summary>
    /// Документ приложения Word
    /// </summary>
    public interface IDocumentWord : IDocumentLibrary
    {
        /// <summary>
        /// Класс для работы с приложением Word
        /// </summary>
        IApplicationOffice ApplicationOffice { get; }
    }
}
