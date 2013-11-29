using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pohui.Filters;
using System.Data.Entity;

namespace Pohui.Models
{
    public class PohuiContext : DbContext
    {
        public PohuiContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Like> Likes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Creative> Creatives { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
    }
}