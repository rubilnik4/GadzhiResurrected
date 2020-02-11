using GadzhiDAL.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GadzhiDAL.Factories.Interfaces
{
    public interface IRepository<T, IdType> where T : EntityBase<IdType>
                                            where IdType : IEquatable<IdType>
    {
        IEnumerable<T> GetAll();

        IQueryable<T> Query();

        IQueryOver<T> QueryOver();

        /// <summary>
        /// Добавить сущность
        /// </summary>       
        void Add(T entity);

        /// <summary>
        /// Добавить сущность асинхронно
        /// </summary> 
        Task AddAsync(T entity);

        void Update(T entity);

        Task UpdateAsync(T entity);

        void Delete(T entity);

        Task DeleteAsync(IdType id, CancellationToken cancellationToken = default(CancellationToken));

        void Delete(IdType id);

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
        Task<T> LoadAsync(IdType id);
    }
}
