using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EASY_SHOP.Models
{
    public class Comment
    {
        public long CommentID { get; set; }

        [ForeignKey("Blog")]
        public int BlogID { get; set; }
        public long UserID { get; set; }
        public string discription { get; set; }

        public DateTime creationTime { get; set; }
        public Blog Blog { get; set; }
        
    }
}