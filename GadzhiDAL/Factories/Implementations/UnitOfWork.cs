using GadzhiDAL.Factories.Interfaces;
using NHibernate;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GadzhiDAL.Factories.Implementations
{
    /// <summary>
    /// Класс обертка для управления транзакциями
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Фабрика для создания сессии подключения к БД
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Сессия для подключения к базе
        /// </summary>
        public ISession Session { get; }

        /// <summary>
        /// Открываемая транзацкия
        /// </summary>
        private ITransaction _transaction;

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            Session = _sessionFactory?.OpenSession();
            BeginTransaction();
        }

        /// <summary>
        /// Открыть транзакцию
        /// </summary>
        private void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Session.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Подтвердить транзакцию
        /// </summary>
        public void Commit()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                _transaction.Commit();
            }
        }

        /// <summary>
        /// Подтвердить транзакцию асинхронно
        /// </summary>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null && _transaction.IsActive)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        public void Rollback()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                _transaction.Rollback();
            }
        }

        /// <summary>
        /// Откатить транзакцию асинхронно
        /// </summary>
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null && _transaction.IsActive)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                _transaction?.Dispose();
                Session?.Dispose();

                disposedValue = true;
            }
        }

        ~UnitOfWork()
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
