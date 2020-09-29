using EASY_SHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EASY_SHOP.Controllers
{
    public class AuthenticationController : Controller
    {
        DbCalls db = new DbCalls();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult login(string userNameOremail, string password)
        {
            string Message;
            int code;
            if (db.User.Any())
            {
                User user = db.User.Where(u => (u.userName == userNameOremail && u.password == password) || (u.email == userNameOremail && u.password == password)).FirstOrDefault();
                if (user != null)
                {
                    
                    Message = "successfully logged in";
                    code = 200;
                    Session["ApplicationUser"] = user;
                    Session.Timeout = 525600;
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 400;
                    Message = "Incorrect Email or Password";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                code = 400;
                Message = "No User Exist";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult registeration(User currentUser)
        {
            string Message;
            int code;
            if (currentUser.userName != "" && currentUser.email != "" && !(currentUser.password.Length <= 5))
            {
                var emailExist = db.User.Where(u => (u.userName == currentUser.userName) || (u.email == currentUser.email)).FirstOrDefault();
                if (emailExist == null)
                {
                    var roles = db.Role.Where(u => u.name== "Normal").FirstOrDefault();
                    currentUser.RoleID = roles.RoleID;
                    db.User.Add(currentUser);
                    db.SaveChanges();
                    code = 200;
                    Message = "User registered";
                    Session["ApplicationUser"] = currentUser;
                    Session.Timeout = 525600;
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Message = "Email or User Name Already Exist";
                    code = 101;
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
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