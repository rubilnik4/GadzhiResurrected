using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Implementations.Services;

namespace GadzhiModules.Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public interface IWcfClientServicesFactory : IDisposable
    {
        /// <summary>
        /// Фабрика для создания сервиса конвертации
        /// </summary>
        ConvertingClientServiceFactory ConvertingClientServiceFactory { get; }

        /// <summary>
        /// Фабрика для создания сервиса подписей
        /// </summary>
        SignatureClientServiceFactory SignatureClientServiceFactory { get; }

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису состояния сервера
        /// </summary>
        ServerStateClientServiceFactory ServerStateClientServiceFactory { get; }
    }
}