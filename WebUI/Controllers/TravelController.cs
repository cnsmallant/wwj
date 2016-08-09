using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;

namespace WebUI.Controllers
{
    public class TravelController : Controller
    {
        //
        // GET: /Travel/
        D8WeChatEntities db = new D8WeChatEntities();
        public ActionResult Index()
        {
            var query = db.ant_singlepage.Where(q => q.ant_singlepage_class == "line" & q.ant_singlepage_stasus == "已启用").OrderByDescending(q => q.ant_singlepage_adddate).ToList();
            ViewBag.TotalPages = query.Count;
            var img = db.com_img.Where(i => i.com_img_fk == "TJLX").Take(1).SingleOrDefault();
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

        public JsonResult Culture(int CurPage)
        {
            try
            {
                var query = db.ant_singlepage.Where(q => q.ant_singlepage_class == "line"&q.ant_singlepage_stasus == "已启用").OrderBy(q => q.ant_singlepage_adddate).ToList();
                IList<ant_singlepage> querylist = new List<ant_singlepage>();
                foreach (var item in query)
                {
                    ant_singlepage s = new ant_singlepage();
                    s.ant_singlepage_id = item.ant_singlepage_id;
                    s.ant_singlepage_title = item.ant_singlepage_title.Length > 8 ? item.ant_singlepage_title.Substring(0, 8) + "…" : item.ant_singlepage_title;
                    s.ant_singlepage_description = item.ant_singlepage_description;
                    s.ant_singlepage_content = RegularExpression.NullHtmlStr(Server.HtmlDecode(item.ant_singlepage_content)).Trim();
                    querylist.Add(s);
                }
                var pagelist = querylist.Take(10 * CurPage).Skip(10 * (CurPage - 1)).ToList();
                return Json(pagelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
