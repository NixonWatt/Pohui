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
    public class CreativeRepository : Repository<Creative>, ICreative
    {
        public readonly DbContext Context = new PohuiContext();
        void ICreative.EditVotes(Creative entity)
        {
            Context.Set<Creative>().Attach(entity);
            Context.Entry(entity).Property(m => m.Votes).IsModified = true;
        }
    }
}