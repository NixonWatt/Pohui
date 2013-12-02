using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using Pohui.Lucene;

namespace Pohui.Models
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly PohuiContext Context;
        public Repository()
        {
            Context = new PohuiContext();
        }

        IQueryable<T> IRepository<T>.GetAll()
        {
            return Context.Set<T>();
        }

        IQueryable<T> IRepository<T>.FindAllBy(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        T IRepository<T>.FindFirstBy(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate).FirstOrDefault();
        }

        void IRepository<T>.Create(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        void IRepository<T>.Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        void IRepository<T>.Delete(IQueryable<T> entities)
        {
            foreach (var entity in entities.ToList())
                Context.Set<T>().Remove(entity);
        }

        int IRepository<T>.Save()
        {
            return Context.SaveChanges();
        }

        T IRepository<T>.Find(int id)
        {
            return Context.Set<T>().Find(id);
        }

        int IRepository<T>.Count()
        {
            return Context.Set<T>().Count();
        }

        void IDisposable.Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}

