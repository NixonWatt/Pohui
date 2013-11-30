using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pohui.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int CreativeID { get; set; }

        [ForeignKey("UserID")]
        public virtual User Users { get; set; }

        [ForeignKey("CreativeID")]
        public virtual Creative Creatives { get; set; }
    }
}