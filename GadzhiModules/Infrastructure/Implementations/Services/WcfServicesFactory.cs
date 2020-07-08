using System;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiModules.Infrastructure.Interfaces.Services;
using Unity;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public class WcfServicesFactory : IWcfServicesFactory
    {
        /// <summary>
        /// Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        public WcfServicesFactory(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Сервис конвертации
        /// </summary>
        private IServiceConsumer<IFileConvertingClientService> _signatureService;

        /// <summary>
        /// Выполнить действие для сервиса конвертации и проверить на ошибки
        /// </summary>
        public async Task<IResultError> UsingConvertingService(Func<IServiceConsumer<IFileConvertingClientService>, Task> fileConvertingFunc) =>
            await UsingConvertingService(fileConvertingService => Unit.Value.
                                                                  VoidBindAsync(_ => fileConvertingFunc(fileConvertingService))).
            MapAsync(result => result.ToResult());

        /// <summary>
        /// Выполнить функцию для сервиса конвертации и проверить на ошибки
        /// </summary>
        public async Task<IResultValue<TResult>> UsingConvertingService<TResult>(Func<IServiceConsumer<IFileConvertingClientService>, Task<TResult>> fileConvertingFunc) =>
            await GetFileConvertingServiceService().
            Map(fileConvertingService => ExecuteAndHandleErrorAsync(() => fileConvertingFunc(fileConvertingService))).
            ResultVoidBadAsync(errors => _loggerService.ErrorsLog(errors));

        /// <summary>
        /// Выполнить функцию для сервиса подписей и проверить на ошибки
        /// </summary>
        public async Task<IResultValue<TResult>> UsingSignatureService<TResult>(Func<IServiceConsumer<ISignatureClientService>, Task<TResult>> signatureFunc)
        {
            using var signatureService = GetSignatureService();
            return (await ExecuteAndHandleErrorAsync(() => signatureFunc(signatureService))).
                    ResultVoidBad(errors => _loggerService.ErrorsLog(errors));
        }

        /// <summary>
        /// Освободить сервисы
        /// </summary>
        public void DisposeServices()
        {
            _signatureService?.Dispose();
            _signatureService = null;
        }

        /// <summary>
        /// Инициализировать сервис конвертации
        /// </summary>
        private IServiceConsumer<IFileConvertingClientService> GetFileConvertingServiceService() =>
            _signatureService ??= _container.Resolve<IServiceConsumer<IFileConvertingClientService>>();

        /// <summary>
        /// Инициализировать сервис для получения подписей
        /// </summary>
        private IServiceConsumer<ISignatureClientService> GetSignatureService() =>
            _container.Resolve<IServiceConsumer<ISignatureClientService>>();

        #region IDisposable Support
        private bool _disposedValue;

        [Logger]
        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {

            }
            _signatureService?.Dispose();
            _signatureService = null;
            _disposedValue = true;
        }

        ~WcfServicesFactory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}