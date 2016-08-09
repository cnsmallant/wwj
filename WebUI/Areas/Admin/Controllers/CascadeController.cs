using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;

namespace WebUI.Areas.Admin.Controllers
{
    public class CascadeController : Controller
    {
        //
        // GET: /Admin/Cascade/
        D8WeChatEntities db = new D8WeChatEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Cascade(string parentid)
        {
            try
            {
                var result = db.com_area.Where(c => c.com_area_parentid == parentid).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
