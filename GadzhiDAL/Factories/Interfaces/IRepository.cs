using GadzhiDAL.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GadzhiDAL.Factories.Interfaces
{
    public interface IRepository<T, IdType> where T : EntityBase<IdType>
                                            where IdType : IEquatable<IdType>
    {
        /// <summary>
        /// Осуществить Linq запрос к базе
        /// </summary>      
        IQueryable<T> Query();

        /// <summary>
        /// Осуществить Nhibernate запрос к базе
        /// </summary>   
        IQueryOver<T> QueryOver();

        /// <summary>
        /// Добавить сущность
        /// </summary>       
        void Add(T entity);

        /// <summary>
        /// Добавить сущность асинхронно
        /// </summary> 
        Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Обновить объект
        /// </summary>        
        void Update(T entity);

        /// <summary>
        /// Обновить объект асинхронно
        /// </summary> 
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Удалить объект
        /// </summary> 
        void Delete(T entity);

        /// <summary>
        /// Удалить объект асинхронно
        /// </summary> 
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Удалить объект по ключу
        /// </summary> 
        void Delete(IdType id);

        /// <summary>
        /// Удалить объект по ключу асинхронно
        /// </summary> 
        Task DeleteAsync(IdType id, CancellationToken cancellationToken = default(CancellationToken));      

        /// <summary>
        /// Получить сущность через первичный ключ. Если отсутсвует вернется null
        /// </summary>      
        T Get(IdType id);

        /// <summary>
        /// Получить сущность асинхронно через первичный ключ. Если отсутсвует вернется null
        /// </summary>   
        Task<T> GetAsync(IdType id);

        /// <summary>
        /// Получить сущность через первичный ключ. Если отсутсвует вернется exception
        /// </summary>        
        T Load(IdType id);

        /// <summary>
        /// Получить сущность асинхронно через первичный ключ. Если отсутсвует вернется exception
        /// </summary>   
        Task<T> LoadAsync(IdType id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Получить первый объект удовлетворяющий условиям или null
        /// </summary>       
        T GetFirstOrDefault(Func<T, bool> predicate = null);

        /// <summary>
        /// Получить первый объект удовлетворяющий условиям или null асинхронно
        /// </summary>   
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, 
                                       CancellationToken cancellationToken = default(CancellationToken));
       
        /// <summary>
        /// Получить все объекты
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Получить все объекты асинхронно
        /// </summary>
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
