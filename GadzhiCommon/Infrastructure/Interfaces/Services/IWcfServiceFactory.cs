using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Фабрика для создания подключения у WCF сервису
    /// </summary>
    public interface IWcfServiceFactory<TService> 
        where TService : class, IDisposable
    {
        /// <summary>
        /// Выполнить действие для сервиса и проверить на ошибки
        /// </summary>
        Task<IResultError> UsingService(Func<TService, Task> serviceFunc);

        /// <summary>
        /// Выполнить функцию для сервиса и проверить на ошибки
        /// </summary>
        Task<IResultValue<TResult>> UsingService<TResult>(Expression<Func<TService, Task<TResult>>> serviceExpression);

        /// <summary>
        /// Выполнить функцию для сервиса, проверить на ошибки и выполнить повторное подключение при сбое
        /// </summary>
        Task<IResultValue<TResult>> UsingServiceRetry<TResult>(Expression<Func<TService, Task<TResult>>> serviceExpression,
                                                                            RetryService retryService);
    }
}