using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public interface IChapter : IRepository<Chapter>
    {
        void EditPosition(Chapter chapter, int id);
        void EditContent(Chapter chapter);
    }
}