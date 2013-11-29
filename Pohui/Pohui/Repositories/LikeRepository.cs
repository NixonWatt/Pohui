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
    }
}