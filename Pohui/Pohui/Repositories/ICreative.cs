using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public interface ICreative : IRepository<Creative>
    {
        void EditVotes(Creative entity);
    }
}