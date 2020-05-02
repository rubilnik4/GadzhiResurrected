using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Implementations;
using System.IO;
using System.Linq;

namespace GadzhiCommon.Helpers.Dialogs
{
    public static class DialogFilters
    {
        /// <summary>
        /// Список фильтров для диалоговых окон
        /// </summary>
        public static readonly string DocAndDgn = "Files|*.doc;*.docx;*.Dgn";

        /// <summary>
        /// Входит ли расширение в список
        /// </summary>
        public static bool IsInDocAndDgnFileTypes(string path)
        {
            string extensionWithoutPoint = FileSystemOperations.ExtensionWithoutPoint(Path.GetExtension(path));
            return ValidFileExtentions.DocAndDgnFileTypes?.Keys.Contains(extensionWithoutPoint) == true;
        }
    }
}
