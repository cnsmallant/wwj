using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;

namespace WebUI.Areas.Admin.Controllers
{

    [AdminVerification]
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/
        D8WeChatEntities db = new D8WeChatEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPwd()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult UserPwdEdit()
        {
            var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
            var upwd = HttpContext.Request.Cookies["upwd"].Value;
            var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
            ant_admin.ant_admin_pwd = TDESHelper.EncryptString(Request.Form["ant_admin_pwd"]);
            db.Configuration.ValidateOnSaveEnabled = false;
            int result = db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;
            if (result > 0)
            {
                Response.Cookies["uname"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["upwd"].Expires = DateTime.Now.AddDays(-1);
                return RedirectToAction("Index", "Login");
            }
            else
            {
                string msg = Server.UrlEncode("操作失败！");
                return RedirectToAction("Index", "Message", new { mid = msg });
            }

        }


        public ActionResult Users()
        {
            try
            {
                ViewBag.list = db.ant_admin.ToList();
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult UserRole(string ant_admin_id, string ant_admin_role)
        {
            try
            {
                ant_admin admin = db.ant_admin.Where(a => a.ant_admin_id == ant_admin_id).SingleOrDefault();
                admin.ant_admin_role = ant_admin_role;
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("Users", "Home");
                }
                else
                {
                    string msg = Server.UrlEncode("操作失败！");
                    return RedirectToAction("Index", "Message", new { mid = msg });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult UserAdd()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult UserDistribution()
        {
            try
            {
                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                ant_admin admin = new ant_admin();
                admin.ant_admin_id = Guid.NewGuid().ToString("N").ToUpper();
                admin.ant_admin_name = Request.Form["ant_admin_name"];
                admin.ant_admin_pwd = TDESHelper.EncryptString("123456");
                admin.ant_admin_role = "1";
                admin.ant_admin_adduser = ant_admin.ant_admin_id;
                admin.ant_admin_adddate = DateTime.Now;
                admin.ant_admin_edituser = ant_admin.ant_admin_id;
                admin.ant_admin_editdate = DateTime.Now;
                db.ant_admin.Add(admin);
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("Users", "Home");
                }
                else
                {
                    string msg = Server.UrlEncode("操作失败！");
                    return RedirectToAction("Index", "Message", new { mid = msg });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult Logout()
        {
            Response.Cookies["uname"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["upwd"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Login");
        }
    }
}
