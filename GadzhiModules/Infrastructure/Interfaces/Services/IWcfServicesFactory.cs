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
    public interface IWcfServicesFactory: IDisposable
    {
        /// <summary>
        /// Выполнить функцию для сервиса подписей и проверить на ошибки
        /// </summary>
        Task<IResultValue<TResult>> UsingSignatureService<TResult>(Func<IServiceConsumer<ISignatureClientService>, Task<TResult>> signatureFunc);

        /// <summary>
        /// Выполнить действие для сервиса конвертации и проверить на ошибки
        /// </summary>
        Task<IResultError> UsingConvertingService(Func<IServiceConsumer<IFileConvertingClientService>, Task> fileConvertingFunc);

        /// <summary>
        /// Выполнить функцию для сервиса конвертации и проверить на ошибки
        /// </summary>
        Task<IResultValue<TResult>> UsingConvertingService<TResult>(Func<IServiceConsumer<IFileConvertingClientService>, Task<TResult>> fileConvertingFunc);

        /// <summary>
        /// Освободить сервисы
        /// </summary>
        void DisposeServices();
    }
}