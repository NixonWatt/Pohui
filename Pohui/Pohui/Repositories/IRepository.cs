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

namespace Pohui.Models
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Create(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> FindAllBy(Expression<Func<T, bool>> predicate);
        T Find(int id);
        T FindFirstBy(Expression<Func<T, bool>> predicate);
        void Delete(T entity);
        void Delete(IQueryable<T> entities);
        int Count();
        int Save();
        //void Edit(T entity);
    }
}

