using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public Creative Creative { get; set; }
    }
}