
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.Elements;
using System;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
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
        public IApplicationWord ApplicationWord => DocumentWord.ApplicationWord;
    }
}
