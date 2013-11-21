using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public class Like
    {
        public int Id { get; set; }
        public User User { get; set; }
        public ICollection<Creative> Creatives { get; set; }
    }
}