using System.Collections.Generic;

namespace GadzhiWord.Word.Interfaces.Excel
{
    public interface IBookExcel
    {
        /// <summary>
        /// Листы в книге
        /// </summary>
        IReadOnlyList<ISheetExcel> Sheets { get; }
    }
}