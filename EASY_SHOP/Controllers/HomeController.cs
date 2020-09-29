using EASY_SHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EASY_SHOP.Controllers
{
    public class HomeController : Controller
    {
        DbCalls db = new DbCalls();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        [Route("/Contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [Route("/Services")]
        public ActionResult Services()
        {
            return View();
        }
        [Route("/About")]

        [HttpGet]
        public JsonResult UserDeatile()
        {
            string Message;
            int code;
            if (Session["ApplicationUser"] != null)
            {
                var User = (Models.User)Session["ApplicationUser"];
                var adminRights = db.Role.Where(u => u.name == "Admin").FirstOrDefault();
                bool isAdmin = false;

                if (User.RoleID == adminRights.RoleID)
                {
                    isAdmin = true;
                }
                else { isAdmin = false; }
                var currentUser = (from k in db.User.Where(u => u.UserID == User.UserID).ToList()
                                   select new
                                   {
                                       k.UserID,
                                       k.userName,
                                       k.email,
                                       isAdmin,
                                   }).FirstOrDefault();
                code = 200;
                Message = "User Detail available";
                return Json(new { code, Message, currentUser }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                code = 401;
                Message = "login first";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult addContactUs(ContactUs currentContactUs)
        {
            string Message;
            int code;
            if (currentContactUs.name!="" && currentContactUs.email != "" && currentContactUs.subject != "" && currentContactUs.company != "" && currentContactUs.message != "")
            {
                Message = "ContactUs successfully added";
                code = 200;
                db.ContactUs.Add(currentContactUs);
                db.SaveChanges();
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Message = "Unauthorized Changes";
                code = 400;
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult addSubscribeUs(string email)
        {
            string Message;
            int code;
            if (email != "")
            {
                Subscribe currentSubscribe = new Subscribe();
                currentSubscribe.email = email;
                currentSubscribe.creationTime = DateTime.UtcNow;
                Message = "Subscriber added";
                code = 200;
                db.Subscribes.Add(currentSubscribe);
                db.SaveChanges();
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Message = "Unauthorized Changes";
                code = 400;
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}