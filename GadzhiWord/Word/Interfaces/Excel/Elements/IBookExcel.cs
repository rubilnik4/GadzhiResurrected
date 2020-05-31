using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiWord.Word.Interfaces.Excel.Elements
{
    public interface IBookExcel
    {
        /// <summary>
        /// Листы в книге
        /// </summary>
        IReadOnlyList<ISheetExcel> Sheets { get; }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        IResultAppValue<string> SaveAs(string filePath);
    }
}