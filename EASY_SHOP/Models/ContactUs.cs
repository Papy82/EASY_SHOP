using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EASY_SHOP.Models
{
    public class ContactUs
    {
        public int ContactUsID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string company { get; set; }
        public string message { get; set; }
    }
}