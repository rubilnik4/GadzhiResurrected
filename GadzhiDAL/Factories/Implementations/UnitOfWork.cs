using GadzhiDAL.Factories.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GadzhiDAL.Factories.Implementations
{
    /// <summary>
    /// Класс обертка для управления транзакциями
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Сессия для подключения к базе
        /// </summary>
        private ISession _session;

        /// <summary>
        /// Открываемая транзацкия
        /// </summary>
        private ITransaction _transaction;

        public UnitOfWork(ISessionFactory sessionFactory)
        {           
            ISession session = sessionFactory.OpenSession();        
        }

        /// <summary>
        /// Открыть транзакцию
        /// </summary>
        public IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
           
            _transaction = _session.BeginTransaction(isolationLevel);           
            return _transaction;
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
        public async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
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
        public async Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_transaction != null && _transaction.IsActive)
            {
                await _transaction.RollbackAsync(cancellationToken);             
            }               
        }
      
        /// <summary>
        /// Закрыть соединение
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
