using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pohui.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreativeId { get; set; }
        [ForeignKey("CreativeId")]
        public Creative Creative { get; set; }
    }
}