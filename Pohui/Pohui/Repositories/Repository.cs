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

namespace Pohui.Models
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly PohuiContext Context;
        public Repository()
        {
            Context = new PohuiContext();
        }

        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual IQueryable<T> FindAllBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public virtual T FindFirstBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate).FirstOrDefault();
        }

        public virtual void Create(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Delete(IQueryable<T> entities)
        {
            foreach (var entity in entities.ToList())
                Context.Set<T>().Remove(entity);
        }

        public virtual int Save()
        {
            return Context.SaveChanges();
        }

        public virtual T Find(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public virtual int Count()
        {
            return Context.Set<T>().Count();
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}

