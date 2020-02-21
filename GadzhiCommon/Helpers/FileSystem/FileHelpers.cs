using System.IO;

namespace GadzhiCommon.Helpers.FileSystem
{
    public static class FileHelpers
    {
        /// <summary>
        /// Убрать точку из расширения файла и привести к нижнему регистру
        /// </summary>      
        public static string ExtensionWithoutPoint(string extension)
        {
            return extension?.ToLower().TrimStart('.');
        }

        /// <summary>
        /// Взять расширение. Убрать точку из расширения файла и привести к нижнему регистру
        /// </summary>      
        public static string ExtensionWithoutPointFromPath(string path)
        {
            string extensionWithPoint = Path.GetExtension(path);
            return ExtensionWithoutPoint(extensionWithPoint);
        }
    }
}
