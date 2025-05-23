﻿using NHibernate;
using System.Collections.Generic;

namespace sw.auth.repository.Repositories.Base
{
    public abstract class RepositoryBase<T, TEntityKey>
    {
        protected ISession Session;

        protected RepositoryBase(ISession session)
        {
            Session = session;
        }

        public void Add(T entity)
        {
            Session.Save(entity);
        }

        public void Remove(T entity)
        {
            Session.Delete(entity);
        }

        public void Save(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void Save(T entity, TEntityKey id)
        {
            var existingEntity = FindBy(id);
            Session.SaveOrUpdate(existingEntity ?? entity);
        }

        public T FindBy(TEntityKey id)
        {
            return Session.Get<T>(id);
        }

        public IList<T> FindAll()
        {
            var criteriaQuery =
              Session.CreateCriteria(typeof(T))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Never);

            return (List<T>)criteriaQuery.List<T>();
        }

        public IList<T> FindAll(int index, int count)
        {
            var criteriaQuery =
              Session.CreateCriteria(typeof(T))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                ;

            return (List<T>)criteriaQuery.SetFetchSize(count)
              .SetFirstResult(index).List<T>();
        }
    }
}
