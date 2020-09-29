using EASY_SHOP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EASY_SHOP.Controllers
{
    public class BlogController : Controller
    {
        DbCalls db = new DbCalls();
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        //Blogs
        public JsonResult addBlog(Blog currentBlog)
        {
            string Message;
            int code;
            if (Session["ApplicationUser"] != null)
            {
                var User = (Models.User)Session["ApplicationUser"];

                if (currentBlog.heading != "" && currentBlog.image.Length >= 100 && currentBlog.discription != "")
                {
                        currentBlog.UserID = User.UserID;
                        byte[] contents = System.Convert.FromBase64String(currentBlog.image);
                        string subpath = "~/Content/img/Blogs/";
                        string fileName = Guid.NewGuid() + ".jpg";
                        var uploadPath = HttpContext.Server.MapPath(subpath);
                        var path = Path.Combine(uploadPath, Path.GetFileName(fileName));
                        System.IO.File.WriteAllBytes(path, contents);
                        currentBlog.image = "/Content/img/Blogs/" + fileName;

                    currentBlog.creationDate = DateTime.UtcNow;
                    db.Blogs.Add(currentBlog);
                    db.SaveChanges();
                    code = 200;
                    Message = "Blog Successfully added";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 400;
                    Message = "UnAuthorized Changing";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                code = 401;
                Message = "Login First";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult deletePost(int BlogID)
        {
            string Message;
            int code;
            if (Session["ApplicationUser"] != null)
            {
                var User = (Models.User)Session["ApplicationUser"];
                var currentBlog = db.Blogs.Where(u => u.BlogID == BlogID && u.UserID == User.UserID).FirstOrDefault();
                var adminRights = db.Role.Where(u => u.name == "Admin").FirstOrDefault();
               

                if (currentBlog != null || User.RoleID==adminRights.RoleID)
                {
                    var comments = db.Comments.Where(u => u.BlogID == currentBlog.BlogID).ToList();
                    
                    foreach (var comment in comments)
                    {
                        db.Comments.Remove(comment);
                    }
                    
                    db.Blogs.Remove(currentBlog);
                    db.SaveChanges();
                    code = 200;
                    Message = "Blog Deleted";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 400;
                    Message = "UnAuthorized Changing";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                code = 401;
                Message = "Login First";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult updateBlog(Blog currentBlog)
        {
            string Message;
            int code;
            if (Session["ApplicationUser"] != null && currentBlog.BlogID!=0)
            {
                var User = (Models.User)Session["ApplicationUser"];
                var isBlogExist = db.Blogs.Where(u => u.BlogID == currentBlog.BlogID).FirstOrDefault();
                if (User.UserID== isBlogExist.UserID && currentBlog.heading != "" && currentBlog.discription != "")
                {
                    isBlogExist.heading = currentBlog.heading;
                    isBlogExist.discription = currentBlog.discription;
                    db.Entry(isBlogExist).State = EntityState.Modified;
                    db.SaveChanges();
                    code = 200;
                    Message = "Blog Successfully updated";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 400;
                    Message = "UnAuthorized Changing";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                code = 401;
                Message = "Login First";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult blogList()
        {
            string Message;
            int code;
               
                var blogList =( from u in db.Blogs.Include("Comments").ToList()
                               select new
                               {
                                   u.BlogID,
                                   u.heading,
                                   u.image,
                                   u.discription,
                                   creationDate=u.creationDate.ToString("dd-MMM-yy hh:mm tt"),
                                   user = (from k in db.User.Where(k => k.UserID == u.UserID).ToList()
                                                  select new
                                                  {
                                                      k.UserID,
                                                      k.userName
                                                  }).LastOrDefault(),
                                   currentComment ="",
                                   Comments = (from c in u.Comments
                                               select new
                                               {
                                                
                                                   user = (from x in db.User.Where(x => x.UserID == c.UserID)
                                                           select new
                                                           {
                                                               x.UserID,
                                                               x.userName
                                                           }).FirstOrDefault(),
                                                   c.CommentID,
                                                   c.discription,
                                                   commentTime = c.creationTime.ToString("dd-MMM-yy hh:mm tt")
                                               }).ToList(),
                               }).ToList().OrderByDescending(u => u.BlogID).Take(10);


            code = 200;
                Message = "Blog List Available";
                return Json(new { code, Message, blogList }, JsonRequestBehavior.AllowGet);
        }

        //Comment
        public JsonResult addComment(int BlogID, string currentCommentDiscription)
        {
            string Message;
            int code;
            if (Session["ApplicationUser"] != null)
            {
                var User = (Models.User)Session["ApplicationUser"];
                Comment currentComment = new Comment();
                if (BlogID!=0 && currentCommentDiscription != "")
                {


                    currentComment.BlogID = BlogID;
                    currentComment.discription = currentCommentDiscription;
                    currentComment.UserID = User.UserID;
                    currentComment.creationTime = DateTime.UtcNow;
                    db.Comments.Add(currentComment);
                    db.SaveChanges();
                    code = 200;
                    Message = "Comment Successfully added";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 400;
                    Message = "UnAuthorized Changing";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                code = 401;
                Message = "Login First";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult deleteComment(int CommentID)
        {
            string Message;
            int code;
            if (Session["ApplicationUser"] != null)
            {
                var User = (Models.User)Session["ApplicationUser"];
                var currentComment = db.Comments.Where(u => u.CommentID == CommentID && u.UserID == User.UserID).FirstOrDefault();
                var adminRights = db.Role.Where(u => u.name == "Admin").FirstOrDefault();


                if (currentComment != null || User.RoleID == adminRights.RoleID)
                {
                   

                    db.Comments.Remove(currentComment);
                    db.SaveChanges();
                    code = 200;
                    Message = "Comment Deleted";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 400;
                    Message = "UnAuthorized Changing";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                code = 401;
                Message = "Login First";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult updateComment(Comment currentComment)
        {
            string Message;
            int code;
            if (Session["ApplicationUser"] != null && currentComment.CommentID!=0)
            {
                var User = (Models.User)Session["ApplicationUser"];
                var isCommentExist = db.Comments.Where(u => u.CommentID == currentComment.CommentID).FirstOrDefault();
                if (User.UserID == Convert.ToInt32(isCommentExist.UserID) && currentComment.discription != "")
                {
                    
                    db.Entry(currentComment).State = EntityState.Modified;
                    db.SaveChanges();
                    code = 200;
                    Message = "Comment Successfully updated";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 400;
                    Message = "UnAuthorized Changing";
                    return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                code = 401;
                Message = "Login First";
                return Json(new { code, Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}