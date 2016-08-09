using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;

namespace WebUI.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        D8WeChatEntities db = new D8WeChatEntities();
        public ActionResult Index()
        {

            ant_singlepage sis = db.ant_singlepage.Where(s => s.ant_singlepage_class == "tel").SingleOrDefault();
            if (sis != null)
            {
                ViewBag.ant_singlepage_title = sis.ant_singlepage_title;
                ViewBag.ant_singlepage_content = sis.ant_singlepage_content;
                ViewBag.ant_singlepage_description = sis.ant_singlepage_description;
            }
            return View();
        }

    }
}
