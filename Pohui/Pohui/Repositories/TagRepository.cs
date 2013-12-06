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
    public class TagRepository : Repository<Tag>, ITag
    {
        //private readonly PohuiContext Context;
        ////IQueryable<Tag> IRepository<Tag>.GetAll()
        ////{
        ////    ICollection<Tag> tags = Context.Set<Tag>().ToList();
        ////    for (int i = 0; i < tags.Count(); i++)
        ////    {
        ////        for (int j = 0; j < tags.Count() - 1; j++)
        ////        {
        ////            if (tags.ElementAt(i).Name == tags.ElementAt(j).Name)
        ////            {
        ////                tags.Remove(tags.ElementAt(j));
        ////            }
        ////        }
        ////    }
        //    return tags;
        //}
    }
}