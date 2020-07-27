using GadzhiConverting.Infrastructure.Implementations.Services;

namespace GadzhiConverting.Infrastructure.Interfaces.Services
{
    public interface IWcfServerSevicesFactory
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