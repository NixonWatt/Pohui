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

        public virtual void Add(T entity)
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

    public class UserRepository : Repository<User>
    {
        public void SetAdminRole(int id)
        {
            var user = Find(id);
            if (isAdmin(id))
            {
                Roles.RemoveUserFromRole(user.Login, "Admin");
            }
            else
            {
                Roles.AddUserToRole(user.Login, "Admin");
            }
        }

        public bool isAdmin(int id)
        {
            var user = Find(id);
            if (Roles.IsUserInRole(user.Login, "Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DropPassword(int id)
        {
            var user = Find(id);
            var token = WebSecurity.GeneratePasswordResetToken(user.Login);
            WebSecurity.ResetPassword(token, "droppedpassword");
        }
    }

    public class CreativeRepository : Repository<Creative>
    {
    }
    

    public class ChapterRepository : Repository<Chapter>
    {
    }

    public class TagRepository : Repository<Tag>
    {
    }
}

