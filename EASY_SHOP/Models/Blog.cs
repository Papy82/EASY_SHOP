using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EASY_SHOP.Models
{
    public class Blog
    {
        public int BlogID { get; set; }

        [ForeignKey("User")]
        public long UserID { get; set; }
        public string heading { get; set; }
        public string image { get; set; }
        public string discription { get; set; }

        public DateTime creationDate { get; set; }

        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}