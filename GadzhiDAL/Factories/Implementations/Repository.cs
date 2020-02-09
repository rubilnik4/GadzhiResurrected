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
    public class Repository<T> : IRepository<T> where T : EntityBase
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

        public void Delete(int id)
        {
            _session.Query<T>().Where(x => x.Id == id).Delete();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _session.Query<T>().Where(x => x.Id == id).DeleteAsync(cancellationToken);
        }

        public T Get(int id)
        {
            return _session.Get<T>(id);
        }

        public async Task<T> GetAsync(int id)
        {
            return await _session.GetAsync<T>(id);
        }
    }
}
