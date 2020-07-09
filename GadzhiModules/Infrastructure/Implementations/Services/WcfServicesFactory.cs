using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
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
        public async Task<IResultValue<TResult>> UsingConvertingService<TResult>(Expression<Func<IServiceConsumer<IFileConvertingClientService>, Task<TResult>>> fileConvertingExpression) =>
            await GetFileConvertingServiceService(IsInitConvertingService(ReflectionInfo.GetExpressionName(fileConvertingExpression))).
            Map(fileConvertingService => ExecuteAndHandleErrorAsync(() => fileConvertingExpression.Compile()(fileConvertingService))).
            ResultVoidBadAsync(errors => _loggerService.ErrorsLog(errors)).
            WhereOkAsync(result => result.HasErrors || IsDisposeConvertingService(ReflectionInfo.GetExpressionName(fileConvertingExpression)),
                okFunc: result => result.ResultVoid(_ => DisposeConvertingService()));

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
        public void DisposeConvertingService()
        {
            _signatureService?.Dispose();
            _signatureService = null;
        }

        /// <summary>
        /// Инициализировать сервис конвертации
        /// </summary>
        private IServiceConsumer<IFileConvertingClientService> GetFileConvertingServiceService(bool reinitialize) =>
            reinitialize 
                ? _signatureService = _container.Resolve<IServiceConsumer<IFileConvertingClientService>>() 
                : _signatureService ??= _container.Resolve<IServiceConsumer<IFileConvertingClientService>>();


        /// <summary>
        /// Инициализировать сервис для получения подписей
        /// </summary>
        private IServiceConsumer<ISignatureClientService> GetSignatureService() =>
            _container.Resolve<IServiceConsumer<ISignatureClientService>>();

        /// <summary>
        /// Необходимость инициализации сервиса при вызове метода
        /// </summary>
        private static bool IsInitConvertingService(string methodName) =>
            nameof(IFileConvertingClientService.SendFiles) == methodName;

        /// <summary>
        /// Необходимость освобождения сервиса при вызове метода
        /// </summary>
        private static bool IsDisposeConvertingService(string methodName) =>
            nameof(IFileConvertingClientService.SetFilesDataLoadedByClient) == methodName ||
            nameof(IFileConvertingClientService.AbortConvertingById) == methodName;

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