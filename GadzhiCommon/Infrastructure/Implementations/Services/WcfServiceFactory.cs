using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Services;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using Unity;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiCommon.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения у WCF сервису
    /// </summary>
    public abstract class WcfServiceFactory<TService> : IWcfServiceFactory<TService>, IDisposable
        where TService : class, IDisposable
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Функция инициализации сервиса конвертации
        /// </summary>
        private readonly Func<TService> _getService;

        protected WcfServiceFactory(Func<TService> getService)
            :this(getService, new RetryService())
        { }

        protected WcfServiceFactory(Func<TService> getService, RetryService retryService)
        {
            _getService = getService ?? throw new ArgumentNullException(nameof(getService));
            _retryService = retryService ?? throw new ArgumentNullException(nameof(retryService));
        }

        /// <summary>
        /// Сервис конвертации
        /// </summary>
        private TService _service;

        /// <summary>
        /// Параметры повторных обращений к сервису
        /// </summary>
        private readonly RetryService _retryService;

        /// <summary>
        /// Получить сервис конвертации
        /// </summary>
        private TService GetService(bool reinitialize) =>
            reinitialize
            ? _service = _getService()
            : _service ??= _getService();

        /// <summary>
        /// Необходимость инициализации сервиса при вызове метода
        /// </summary>
        protected abstract bool IsInitMethodService(string methodName);

        /// <summary>
        /// Необходимость освобождения сервиса при вызове метода
        /// </summary>
        protected abstract bool IsDisposeMethodService(string methodName);

        /// <summary>
        /// Выполнить действие для сервиса и проверить на ошибки
        /// </summary>
        public async Task<IResultError> UsingService(Func<TService, Task> serviceFunc) =>
            await UsingService(service => Unit.Value.VoidBindAsync(_ => serviceFunc(service))).
            MapAsync(result => result.ToResult());

        /// <summary>
        /// Выполнить функцию для сервиса и проверить на ошибки
        /// </summary>
        public async Task<IResultValue<TResult>> UsingService<TResult>(Expression<Func<TService, Task<TResult>>> serviceExpression) =>
            await UsingServiceRetry(serviceExpression, new RetryService());

        /// <summary>
        /// Выполнить функцию для сервиса, проверить на ошибки и выполнить повторное подключение при сбое со стандартными параметрами
        /// </summary>
        public async Task<IResultValue<TResult>> UsingServiceRetry<TResult>(Expression<Func<TService, Task<TResult>>> serviceExpression) =>
            await UsingServiceRetry(serviceExpression, _retryService);

        /// <summary>
        /// Выполнить функцию для сервиса, проверить на ошибки и выполнить повторное подключение при сбое
        /// </summary>
        public async Task<IResultValue<TResult>> UsingServiceRetry<TResult>(Expression<Func<TService, Task<TResult>>> serviceExpression,
                                                                            RetryService retryService) =>
            await UsingServiceDefault(serviceExpression).
            ResultVoidBadBindAsync(_ => Task.Delay(RetryService.RetryDelayMilliseconds)).
            ResultValueBadBindAsync(errors => retryService.IsRetryLast()
                                              ? Task.FromResult((IResultValue<TResult>)new ResultValue<TResult>(errors))
                                              : UsingServiceRetry(serviceExpression, retryService.NextRetry()));

        /// <summary>
        /// Выполнить функцию для сервиса конвертации и проверить на ошибки
        /// </summary>
        protected async Task<IResultValue<TResult>> UsingServiceDefault<TResult>(Expression<Func<TService, Task<TResult>>> serviceExpression) =>
            await GetService(IsInitMethodService(ReflectionInfo.GetExpressionName(serviceExpression))).
            Map(fileConvertingService => ExecuteAndHandleErrorAsync(() => serviceExpression.Compile()(fileConvertingService))).
            ResultVoidBadAsync(errors => _loggerService.ErrorsLog(errors)).
            WhereOkAsync(result => IsDisposeMethodService(ReflectionInfo.GetExpressionName(serviceExpression)),
                         okFunc: result => result.ResultVoid(_ => Dispose()));

        #region IDisposable
        public void Dispose()
        {
            _service?.Dispose();
            _service = null;
        }
        #endregion
    }
}