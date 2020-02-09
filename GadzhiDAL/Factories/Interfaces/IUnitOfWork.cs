using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GadzhiDAL.Factories.Interfaces
{
    /// <summary>
    /// Класс обертка для управления транзакциями
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Открыть транзакцию
        /// </summary>
        IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Подтвердить транзакцию
        /// </summary>
        void Commit();

        /// <summary>
        /// Подтвердить транзакцию асинхронно
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        void Rollback();

        /// <summary>
        /// Откатить транзакцию асинхронно
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Получить текущую сессию
        /// </summary>
        ISession GetCurrentSession();
    }
}
