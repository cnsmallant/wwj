using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;

namespace WebUI.Areas.Admin.Controllers
{
    [AdminVerification]
    public class AntImgController : Controller
    {
        //
        // GET: /Admin/AntImg/
        D8WeChatEntities db = new D8WeChatEntities();
        public ActionResult Index(string com_img_fk, int pageid = 1)
        {
            try
            {
                var query = db.com_img;
                var list = query.Where(c => c.com_img_fk == com_img_fk).OrderBy(c=>c.com_img_adddate).ToList();
                IList<com_img> listimg = new List<com_img>();
                foreach (var item in list)
                {
                    com_img img = new com_img();
                    img.com_img_id = item.com_img_id;
                    img.com_img_title = item.com_img_title;
                    img.com_img_alt = item.com_img_alt;
                    img.com_img_src = item.com_img_src;
                    img.com_img_adduser = db.ant_admin.Where(a => a.ant_admin_id == item.com_img_adduser).SingleOrDefault().ant_admin_name;
                    img.com_img_adddate = item.com_img_adddate;
                    img.com_img_edituser = db.ant_admin.Where(a => a.ant_admin_id == item.com_img_adduser).SingleOrDefault().ant_admin_name;
                    img.com_img_editdate = item.com_img_editdate;

                    listimg.Add(img);
                }
                IList<com_img> mPage = PageCommon.PageList<com_img>(pageid, 20, listimg.Count, listimg);
                return View("Index", mPage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult UploadImage()
        {

            if (!string.IsNullOrEmpty(Request["com_img_fk"]))
            {
                ViewBag.com_img_fk = Request["com_img_fk"].ToString();

            }
            return View();
        }

        public ActionResult AddImages(HttpPostedFileBase com_img_src)
        {
            try
            {
                string fileName = com_img_src.FileName;
                var guid = Guid.NewGuid().ToString("N");
                //转换只取得文件名 去掉路。
                if (fileName.LastIndexOf("\\") > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                }
                var com_img_fk = Request.Form["com_img_fk"];
                var returnurl = Server.UrlDecode(Request.Form["returnurl"]);
                string pathstr = "/file/img/" + com_img_fk + "/";
                string path = Server.MapPath(pathstr);

                string src = string.Empty;
                if (FileHelper.CreateDir(path))
                {
                    src = path + fileName;
                    com_img_src.SaveAs(src);  //保存到相对路径下
                }
                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var sys_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();

                com_img img = new com_img();
                img.com_img_id = guid;
                img.com_img_src = pathstr + fileName;
                img.com_img_fk = com_img_fk;
                img.com_img_adduser = sys_admin.ant_admin_id;
                img.com_img_adddate = DateTime.Now;
                img.com_img_edituser = sys_admin.ant_admin_id;
                img.com_img_editdate = DateTime.Now;
                db.com_img.Add(img);
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("Index", "AntImg", new { com_img_fk = com_img_fk });
                }
                else
                {
                    string msg = Server.UrlEncode("上传失败！");
                    return RedirectToAction("Index", "Message", new { mid = msg });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult EditImage()
        {

            if (!string.IsNullOrEmpty(Request["com_img_fk"]) && !string.IsNullOrEmpty(Request["com_img_id"]))
            {
                var com_img_fk = Request["com_img_fk"].ToString();
                var com_img_id = Request["com_img_id"].ToString();
                ViewBag.com_img_fk = com_img_fk;
                ViewBag.com_img_id = com_img_id;
                com_img img = db.com_img.Where(i => i.com_img_fk == com_img_fk & i.com_img_id == com_img_id).SingleOrDefault();
                ViewBag.com_img_srcs = img.com_img_src;


            }
            else if (!string.IsNullOrEmpty(Request["com_img_fk"]))
            {
                var com_img_fk = Request["com_img_fk"].ToString();
                ViewBag.com_img_fk = com_img_fk;
                com_img img = db.com_img.Where(i => i.com_img_fk == com_img_fk).SingleOrDefault();
                ViewBag.com_img_srcs = img.com_img_src;


            }

            return View();
        }

        public ActionResult EditImages(HttpPostedFileBase com_img_src)
        {

            try
            {
                string fileName = com_img_src.FileName;
                //转换只取得文件名 去掉路。
                if (fileName.LastIndexOf("\\") > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                }
                var com_img_fk = Request.Form["com_img_fk"].ToString();
                var com_img_id = Request.Form["com_img_id"];
                int result = 0;
                if (!string.IsNullOrEmpty(com_img_fk) && !string.IsNullOrEmpty(com_img_id))
                {

                   
                    var returnurl = Server.UrlDecode(Request.Form["returnurl"]);
                    com_img imgs = db.com_img.Where(i => i.com_img_fk == com_img_fk & i.com_img_id == com_img_id).SingleOrDefault();
                    string pathstr = "/file/img/" + com_img_fk + "/";
                    string path = Server.MapPath(pathstr);
                    string delsrc = Server.MapPath(imgs.com_img_src);
                    string src = string.Empty;
                    if (FileHelper.DelFile(delsrc))
                    {
                        src = path + fileName;
                        com_img_src.SaveAs(src);  //保存到相对路径下
                    }
                    var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                    var upwd = HttpContext.Request.Cookies["upwd"].Value;
                    var sys_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                    imgs.com_img_src = pathstr + fileName;
                    imgs.com_img_fk = com_img_fk;
                    imgs.com_img_edituser = sys_admin.ant_admin_id;
                    imgs.com_img_editdate = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    result = db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;
                }
                else if (!string.IsNullOrEmpty(com_img_fk))
                {
                    var returnurl = Server.UrlDecode(Request.Form["returnurl"]);
                    com_img imgs = db.com_img.Where(i => i.com_img_fk == com_img_fk).SingleOrDefault();
                    string pathstr = "/file/img/" + com_img_fk + "/";
                    string path = Server.MapPath(pathstr);
                    string delsrc = Server.MapPath(imgs.com_img_src);
                    string src = string.Empty;
                    if (FileHelper.DelFile(delsrc))
                    {
                        src = path + fileName;
                        com_img_src.SaveAs(src);  //保存到相对路径下
                    }
                    var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                    var upwd = HttpContext.Request.Cookies["upwd"].Value;
                    var sys_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                    imgs.com_img_src = pathstr + fileName;
                    imgs.com_img_fk = com_img_fk;
                    imgs.com_img_edituser = sys_admin.ant_admin_id;
                    imgs.com_img_editdate = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    result = db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;
                }

                if (result > 0)
                {
                    return RedirectToAction("Index", "AntImg", new { com_img_fk = com_img_fk });
                }
                else
                {
                    string msg = Server.UrlEncode("上传失败！");
                    return RedirectToAction("Index", "Message", new { mid = msg });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult ImgaeDel(string com_img_fk, string com_img_id)
        {
            try
            {
                com_img img = db.com_img.Where(i => i.com_img_id == com_img_id).SingleOrDefault();
                string pathsrc = Server.MapPath(img.com_img_src);
                if (FileHelper.DelFile(pathsrc))
                {
                    db.com_img.Remove(img);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AntImg", new { com_img_fk = com_img_fk });
                }
                else
                {
                    string msg = Server.UrlEncode("删除失败！");
                    return RedirectToAction("Index", "Message", new { mid = msg });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
