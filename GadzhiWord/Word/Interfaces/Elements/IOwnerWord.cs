using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Родительский элемент
    /// </summary>
    public interface IOwnerWord
    {
        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        IApplicationWord ApplicationWord { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        IDocumentWord DocumentWord { get; }
    }
}
