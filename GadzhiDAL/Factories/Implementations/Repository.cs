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
        protected ISession Session;

        protected Repository(ISession session)
        {
            Session = session;
        }

        public IEnumerable<T> GetAll()
        {
            return Session.Query<T>().ToList();
        }

        public IQueryable<T> Query()
        {
            return Session.Query<T>();
        }

        public IQueryOver<T> QueryOver()
        {
            return Session.QueryOver<T>();
        }

        public void Create(T entity)
        {
            Session.Save(entity);
        }

        public async Task CreateAsync(T entity)
        {
            await Session.SaveAsync(entity);
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await Session.UpdateAsync(entity);
        }


        public void Delete(T entity)
        {
            Session.Delete(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await Session.DeleteAsync(entity);
        }

        public void Delete(int id)
        {
            Session.Query<T>().Where(x => x.Id == id).Delete();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Session.Query<T>().Where(x => x.Id == id).DeleteAsync(cancellationToken);
        }

        public T Get(int id)
        {
            return Session.Get<T>(id);
        }

        public async Task<T> GetAsync(int id)
        {
            return await Session.GetAsync<T>(id);
        }
    }
}
