namespace GadzhiCommonServer.Infrastructure.Implementations
{
    /// <summary>
    /// Общие параметры для серверной части
    /// </summary>
    public static class SettingsServer
    {
        /// <summary>
        /// Имя базы данных по умолчанию
        /// </summary>
        public static string DataBaseNameDefault => "GadzhiSQLite.db";

        /// <summary>
        /// Папка для хранения базы данных по умолчанию
        /// </summary>
        public static string DataBaseDirectoryDefault => "DataBase.gitignore";
    }
}
