using System;

namespace GadzhiCommon.Extensions.StringAdditional
{
    /// <summary>
    /// Методы расширения для путей файлов
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// Добавить дроби в конце пути
        /// </summary>      
        public static string AddSlashesToPath(this string path) =>
            path?.EndsWith("\\", StringComparison.Ordinal) == false ?
            path + "\\" :
            path;
    }
}
