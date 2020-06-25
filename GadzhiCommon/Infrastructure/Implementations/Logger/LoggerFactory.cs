using GadzhiCommon.Infrastructure.Interfaces.Logger;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Фабрика для инициализации хранилища системных сообщений
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Получить хранилище системных сообщений
        /// </summary>
        public static ILoggerService GetFileLogger => new FileLoggerService();
    }
}