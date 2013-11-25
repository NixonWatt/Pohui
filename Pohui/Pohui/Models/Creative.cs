using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public class Creative
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Votes { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
    }
}