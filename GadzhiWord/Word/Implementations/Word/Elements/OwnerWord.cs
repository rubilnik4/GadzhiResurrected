using System;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.Word;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Word.Implementations.Word.Elements
{
    /// <summary>
    /// Родительский элемент
    /// </summary>
    public class OwnerWord : IOwnerWord
    {
        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        public IDocumentWord DocumentWord { get; }

        public OwnerWord(IDocumentWord documentWord)
        {
            DocumentWord = documentWord ?? throw new ArgumentNullException(nameof(documentWord)); ;
        }

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationOffice ApplicationOffice => DocumentWord.ApplicationOffice;
    }
}
