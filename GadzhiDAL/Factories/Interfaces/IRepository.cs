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
    public interface IRepository<T> where T : EntityBase
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

        Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        void Delete(int id);

        T Get(int id);

        Task<T> GetAsync(int id);

    }
}
