using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;

namespace GadzhiConvertingLibrary.Infrastructure.Interfaces.Services
{
    public interface IWcfServerServicesFactory
    {
        /// <summary>
        /// Фабрика для создания сервиса конвертации
        /// </summary>
        ConvertingServerServiceFactory ConvertingServerServiceFactory { get; }

        /// <summary>
        /// Фабрика для создания сервиса подписей
        /// </summary>
        SignatureServerServiceFactory SignatureServerServiceFactory { get; }
    }
}