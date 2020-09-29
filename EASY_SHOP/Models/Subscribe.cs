using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EASY_SHOP.Models
{
    public class Subscribe
    {
        public int SubscribeID { get; set; }
        public string email { get; set; }
        public DateTime creationTime { get; set; }
    }
}