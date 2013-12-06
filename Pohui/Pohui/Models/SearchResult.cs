using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public class SearchResult
    {
       public IEnumerable<Chapter> chapters;
       public IEnumerable<Creative> creatives;
       public IEnumerable<Tag> tags;
       public IEnumerable<User> users;
    }
}