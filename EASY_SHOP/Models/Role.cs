using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EASY_SHOP.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string name { get; set; }
        public string discription { get; set; }

        public ICollection<User> Users { get; set; }
    }
}