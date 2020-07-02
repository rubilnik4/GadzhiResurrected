using ChannelAdam.ServiceModel;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Signatures;

namespace GadzhiModules.Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public interface IWcfServicesFactory
    {
        /// <summary>
        /// Инициализировать сервис конвертации
        /// </summary>
        IServiceConsumer<IFileConvertingClientService> GetFileConvertingServiceService();

        /// <summary>
        /// Инициализировать сервис для получения подписей
        /// </summary>
        IServiceConsumer<ISignatureClientService> GetSignatureService();
    }
}