using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiModules.Infrastructure.Implementations.Services;

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
        Task<IResultValue<TResult>> UsingConvertingService<TResult>(Expression<Func<IServiceConsumer<IFileConvertingClientService>, Task<TResult>>> fileConvertingExpression);

        /// <summary>
        /// Выполнить функцию для сервиса конвертации, проверить на ошибки и выполнить повторное подключение при сбое
        /// </summary>
        Task<IResultValue<TResult>> UsingConvertingServiceRetry<TResult>(Expression<Func<IServiceConsumer<IFileConvertingClientService>, Task<TResult>>> fileConvertingExpression, 
                                                                         RetryService retryService);

        /// <summary>
        /// Освободить сервисы
        /// </summary>
        void DisposeConvertingService();
    }
}