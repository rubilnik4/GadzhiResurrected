using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiWord.Word.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Implementation.Resources;

namespace GadzhiWord.Word.Interfaces
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public interface IApplicationWord : IApplicationLibrary<IDocumentWord>
    {
        /// <summary>
        /// Ресурсы, используемые модулем Word
        /// </summary>
        ResourcesWord ResourcesWord { get; }
    }
}
