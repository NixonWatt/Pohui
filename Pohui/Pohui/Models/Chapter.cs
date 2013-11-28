using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public int CreativeId { get; set; }
        public int Position { get; set; }
    }
}