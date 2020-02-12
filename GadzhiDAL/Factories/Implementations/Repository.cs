using GadzhiDAL.Entities;
using GadzhiDAL.Factories.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        /// <summary>
        /// Осуществить Linq запрос к базе
        /// </summary>      
        public IQueryable<T> Query()
        {
            return _session.Query<T>();
        }

        /// <summary>
        /// Осуществить Nhibernate запрос к базе
        /// </summary>   
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
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _session.SaveAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Обновить объект
        /// </summary>   
        public void Update(T entity)
        {
            _session.Update(entity);
        }

        /// <summary>
        /// Обновить объект асинхронно
        /// </summary> 
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _session.UpdateAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Удалить объект
        /// </summary> 
        public void Delete(T entity)
        {
            _session.Delete(entity);
        }

        /// <summary>
        /// Удалить объект асинхронно
        /// </summary> 
        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _session.DeleteAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Удалить объект по ключу
        /// </summary> 
        public void Delete(IdType id)
        {
            _session.Query<T>().Where(x => x.Id.Equals(id)).Delete();
        }

        /// <summary>
        /// Удалить объект по ключу асинхронно
        /// </summary> 
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
        public async Task<T> LoadAsync(IdType id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _session.LoadAsync<T>(id, cancellationToken);
        }

        /// <summary>
        /// Получить первый объект удовлетворяющий условиям или null
        /// </summary>       
        public T GetFirstOrDefault(Func<T, bool> predicate = null)
        {           
            return predicate == null ?
                   _session.Query<T>().FirstOrDefault() :
                   _session.Query<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Получить первый объект удовлетворяющий условиям или null асинхронно
        /// </summary>   
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
                                                    CancellationToken cancellationToken = default(CancellationToken))
        {

            return predicate == null ?
                   await _session.Query<T>().FirstOrDefaultAsync(cancellationToken):
                   await _session.Query<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// Получить все объекты
        /// </summary>
        public IEnumerable<T> GetAll()
        {
            return _session.Query<T>().ToList();
        }

        /// <summary>
        /// Получить все объекты асинхронно
        /// </summary>
        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _session.Query<T>().ToListAsync(cancellationToken);
        }
    }
}
