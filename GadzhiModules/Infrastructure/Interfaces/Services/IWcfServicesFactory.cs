using System;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Models.Interfaces.Errors;
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

        /// <summary>
        /// Выполнить функцию для сервиса подписей и проверить на ошибки
        /// </summary>
        Task<IResultValue<TResult>> UsingSignatureServiceAsync<TResult>(Func<IServiceConsumer<ISignatureClientService>, Task<TResult>> signatureFunc);
    }
}