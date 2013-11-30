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
    public class LikeRepository : Repository<Like>, ILike
    {
        public readonly DbContext Context = new PohuiContext();
        void IRepository<Like>.Create(Like entity)
        {
            if (Context.Set<Like>().Where(m => m.UserID == entity.UserID && m.CreativeID == entity.CreativeID).FirstOrDefault() == null)
            {
                Context.Set<Like>().Add(entity);
            }
            else
            {
                Context.Set<Like>().Remove(entity);
            }
        }
    }
}