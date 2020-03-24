using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces
{
    /// <summary>
    /// Документ приложения Word
    /// </summary>
    public interface IDocumentWord : IDocumentLibrary
    {
        /// <summary>
        /// Класс для работы с приложением Word
        /// </summary>
        IApplicationWord ApplicationWord { get; }
    }
}
