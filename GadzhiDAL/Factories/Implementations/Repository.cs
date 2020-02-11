using GadzhiDAL.Entities;
using GadzhiDAL.Factories.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GadzhiDAL.Factories.Implementations
{
    /// <summary>
    /// Репозиторий для работы с данными в базе
    /// </summary>
    /// <typeparam name="T">Класс в баз данных</typeparam>
    /// <typeparam name="TKey">Тип ключа в таблице</typeparam>
    public class Repository<T, IdType> : IRepository<T, IdType> where T : EntityBase<IdType>
                                                                where IdType : IEquatable<IdType>
    {
        /// <summary>
        /// Текущая сессия
        /// </summary>
        private readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<T> GetAll()
        {
            return _session.Query<T>().ToList();
        }

        public IQueryable<T> Query()
        {
            return _session.Query<T>();
        }

        public IQueryOver<T> QueryOver()
        {
            return _session.QueryOver<T>();
        }

        /// <summary>
        /// Добавить сущность
        /// </summary>       
        public void Add(T entity)
        {
            _session.Save(entity);
        }

        /// <summary>
        /// Добавить сущность асинхронно
        /// </summary> 
        public async Task AddAsync(T entity)
        {
            await _session.SaveAsync(entity);
        }

        public void Update(T entity)
        {
            _session.Update(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _session.UpdateAsync(entity);
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await _session.DeleteAsync(entity);
        }

        public void Delete(IdType id)
        {
            _session.Query<T>().Where(x => x.Id.Equals(id)).Delete();
        }

        public async Task DeleteAsync(IdType id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _session.Query<T>().Where(x => x.Id.Equals(id)).DeleteAsync(cancellationToken);
        }

        /// <summary>
        /// Получить сущность через первичный ключ. Если отсутсвует вернется null
        /// </summary>        
        public T Get(IdType id)
        {
            return _session.Get<T>(id);
        }

        /// <summary>
        /// Получить сущность асинхронно через первичный ключ. Если отсутсвует вернется null
        /// </summary>   
        public async Task<T> GetAsync(IdType id)
        {
            return await _session.GetAsync<T>(id);
        }

        /// <summary>
        /// Получить сущность через первичный ключ. Если отсутсвует вернется exception
        /// </summary>        
        public T Load(IdType id)
        {
            return _session.Load<T>(id);
        }

        /// <summary>
        /// Получить сущность асинхронно через первичный ключ. Если отсутсвует вернется exception
        /// </summary>   
        public async Task<T> LoadAsync(IdType id)
        {
            return await _session.LoadAsync<T>(id);
        }
    }
}
