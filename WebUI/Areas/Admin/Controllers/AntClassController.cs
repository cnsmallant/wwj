using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class AntClassController : Controller
    {
        //
        // GET: /Admin/AntClass/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add()
        {
            return View();
        }
    }
}
