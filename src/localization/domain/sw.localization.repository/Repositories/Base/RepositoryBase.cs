using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace sw.localization.repository.Repositories.Base
{
    public abstract class RepositoryBase<T, TEntityKey> where T : class
    {
        protected swDbContext Context;

        protected RepositoryBase(swDbContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Set<T>()
                .Add(entity);
        }

        public void Remove(T entity)
        {
            Context.Set<T>()
                .Remove(entity);
        }

        public void Save(T entity, TEntityKey id)
        {
            var existingEntity = Context.Find<T>(id);

            if (existingEntity != null)
            {
                Context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
        }

        public T FindBy(TEntityKey id)
        {
            return Context.Find<T>(id);
        }

        public IList<T> FindAll()
        {
            return Context.Set<T>()
                .AsNoTracking()
                .ToList();
        }

        public IList<T> FindAll(int index, int count)
        {
            return Context.Set<T>()
                .AsNoTracking()
                .Skip(index)
                .Take(count)
                .ToList();
        }
    }
}
