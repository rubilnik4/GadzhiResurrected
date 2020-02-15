using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Helpers.Dialogs
{
    public static class DialogFilters
    {
        /// <summary>
        /// Список фильтров для диалоговых окон
        /// </summary>
        public static readonly string DocAndDgn = "Files|*.doc;*.docx;*.dgn";

     

        /// <summary>
        /// Входит ли расшерение в список
        /// </summary>
        public static bool IsInDocAndDgnFileTypes(string path)
        {
            string extensionWithoutPoint = FileHelpers.ExtensionWithoutPoint(Path.GetExtension(path));
            return ValidFileExtentions.DocAndDgnFileTypes?.Contains(extensionWithoutPoint) == true;
        }
    }
}
