using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EASY_SHOP.Models
{
    public class DbCalls : DbContext
    {
        public DbCalls() : base("Connection")
        {
        }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Subscribe> Subscribes { get; set; }

    }
}