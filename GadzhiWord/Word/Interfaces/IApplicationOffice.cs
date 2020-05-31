using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Implementation.Resources;
using GadzhiWord.Word.Interfaces.Excel;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using GadzhiWord.Word.Interfaces.Word;

namespace GadzhiWord.Word.Interfaces
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public interface IApplicationOffice : IApplicationLibrary<IDocumentWord>
    {
        /// <summary>
        /// Ресурсы, используемые модулем Word
        /// </summary>
        ResourcesWord ResourcesWord { get; }

        /// <summary>
        /// Создать новую книгу Excel
        /// </summary>
        public IBookExcel CreateWorkbook();
    }
}
