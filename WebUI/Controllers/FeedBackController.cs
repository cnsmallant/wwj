using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;
using System.IO;

namespace WebUI.Controllers
{
    public class FeedBackController : Controller
    {
        //
        // GET: /FeedBack/
        D8WeChatEntities db = new D8WeChatEntities();

        public ActionResult Index()
        {
            var img = db.com_img.Where(i => i.com_img_fk == "YJFK").Take(1).SingleOrDefault();
            if (img == null)
            {
                ViewBag.com_img_src = "/file/default.jpg";
            }
            else
            {
                ViewBag.com_img_src = img.com_img_src;
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult FeedBackAdd()
        {
            try
            {
                var stre = HttpContext.Request.InputStream;
                var jsonstr = new StreamReader(stre).ReadToEnd();
                var ant_feedback_name = JSONHelper.JsonToString(jsonstr, "ant_feedback_name");
                var ant_feedback_tel = JSONHelper.JsonToString(jsonstr, "ant_feedback_tel");
                var ant_feedback_content = JSONHelper.JsonToString(jsonstr, "ant_feedback_content");
                ant_feedback fb = new ant_feedback();
                fb.ant_feedback_id = Guid.NewGuid().ToString("N").ToUpper();
                fb.ant_feedback_name = ant_feedback_name;
                fb.ant_feedback_tel = ant_feedback_tel;
                fb.ant_feedback_content = ant_feedback_content;
                fb.ant_feedback_adddate = DateTime.Now;
                db.ant_feedback.Add(fb);
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NO", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
