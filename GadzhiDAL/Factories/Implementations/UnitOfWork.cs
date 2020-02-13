using GadzhiDAL.Entities.FilesConvert;
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
        /// Фабрика для создания сессии подключения к БД
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Сессия для подключения к базе
        /// </summary>
        private ISession _session;

        /// <summary>
        /// Открываемая транзацкия
        /// </summary>
        private ITransaction _transaction;

        /// <summary>
        /// Репозиторий для конвертируемых файлов
        /// </summary>
        public IRepository<FilesDataEntity, string> _repositoryFilesData { get; set; }

        public UnitOfWork(ISessionFactory sessionFactory)
        {           
            _sessionFactory = sessionFactory;          
        }

        /// <summary>
        /// Открыть транзакцию
        /// </summary>
        public IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _session = _sessionFactory.OpenSession();
            _transaction = _session.BeginTransaction(isolationLevel);
            _repositoryFilesData = new Repository<FilesDataEntity, string>(_session);

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
        /// Получить текущую сессию
        /// </summary>
        public ISession GetCurrentSession() => _session;

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _session?.Dispose();
           // GC.SuppressFinalize(this);
        }       
    }
}
