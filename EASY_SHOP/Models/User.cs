using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EASY_SHOP.Models
{
    public class User
    {
        [Key]
        public long UserID { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool isVerified { get; set; }

        public Role Role { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}