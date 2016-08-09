using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;

namespace WebUI.Controllers
{
    public class BuildingController : Controller
    {
        //
        // GET: /Building/
        D8WeChatEntities db = new D8WeChatEntities();
        [Serializable]
        public class ant_classes : ant_class
        {
            public string com_img_src { get; set; }
        }
        public ActionResult Index()
        {
            try
            {

                Buildingclass();
                var querys = db.ant_trunk.OrderBy(q => q.ant_trunk_order).ToList();
                ViewBag.TotalPages = querys.Count;
                return View();
            }
            catch (Exception)
            {

                throw;
            }


        }

        #region 建筑列表
        public JsonResult BuildingList(int CurPage)
        {
            try
            {
                var query = db.ant_trunk.Where(q=>q.ant_trunk_stasus!=0).OrderBy(q => q.ant_trunk_order).ToList();
                var pagelist = query.Take(20 * CurPage).Skip(20 * (CurPage - 1)).ToList();
                return Json(pagelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult BuildingClass(string id)
        {
            try
            {
                ViewBag.title = db.ant_class.Where(q => q.ant_class_id == id).SingleOrDefault().ant_class_name;
                Buildingclass();
                var trlist = db.ant_trunk.Where(t => t.ant_trunk_class_id == id & t.ant_trunk_stasus != 0).OrderBy(t => t.ant_trunk_order).ToList();
                ViewBag.trlist = trlist;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult BuildingSearch(string id)
        {
            try
            {
                Buildingclass();
                string str = Server.UrlDecode(id);
                var trlist = db.ant_trunk.Where(t => t.ant_trunk_title.Contains(str) || t.ant_trunk_summary.Contains(str)).OrderBy(t => t.ant_trunk_order).ToList();
                ViewBag.trlist = trlist.Where(t => t.ant_trunk_stasus != 0).OrderBy(t => t.ant_trunk_order).ToList(); ;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult OpenScenicSpot()
        {
            try
            {
                Buildingclass();
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult OpenScenicSpots(int CurPage)
        {
            try
            {
                var query = db.ant_trunk.Where(q => q.ant_trunk_stasus == 2).OrderBy(q => q.ant_trunk_order).ToList();
                var pagelist = query.Take(10 * CurPage).Skip(10 * (CurPage - 1)).ToList();
                return Json(pagelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult SearchBuilding()
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

        private void Buildingclass()
        {
            var query = db.ant_class;
            var list = query.OrderBy(q => q.ant_class_adddate).ToList();
            IList<ant_classes> cllist = new List<ant_classes>();
            foreach (var item in list)
            {
                ant_classes cles = new ant_classes();
                cles.ant_class_id = item.ant_class_id;
                var img = db.com_img.Where(c => c.com_img_fk == item.ant_class_id).SingleOrDefault();
                if (img == null)
                {
                    cles.com_img_src = "/file/default.jpg";
                }
                else
                {
                    cles.com_img_src = img.com_img_src;
                }
                cles.ant_class_name = item.ant_class_name;
                cles.ant_class_attribute = item.ant_class_attribute;
                cles.ant_class_adduser = db.ant_admin.Where(a => a.ant_admin_id == item.ant_class_adduser).SingleOrDefault().ant_admin_name;
                cles.ant_class_adddate = item.ant_class_adddate;
                cllist.Add(cles);
            }
            ViewBag.classlist = cllist;
        }






        public ActionResult BuildingDetailes(string id)
        {
            try
            {
                ant_trunk tr = db.ant_trunk.Where(t => t.ant_trunk_id == id).SingleOrDefault();
                ViewBag.ant_trunk_title = tr.ant_trunk_title;
                ViewBag.ant_trunk_entitle = tr.ant_trunk_entitle;
                ViewBag.ant_trunk_summary = tr.ant_trunk_summary;
                ViewBag.ant_content_tags = tr.ant_content_tags;
                ViewBag.ant_trunk_area = tr.ant_trunk_area;
                ViewBag.ant_trunk_content = Server.HtmlDecode(tr.ant_trunk_content);
                ViewBag.ant_trunk_id = tr.ant_trunk_id;
                ant_class cl = db.ant_class.Where(c => c.ant_class_id == tr.ant_trunk_class_id).SingleOrDefault();
                ViewBag.ant_class_attribute = cl.ant_class_attribute;
                var imglist = db.com_img.Where(i => i.com_img_fk == id).OrderBy(q => q.com_img_adddate).ToList();
                if (imglist.Count > 0)
                {
                    var imglists = db.com_img.Where(i => i.com_img_fk == id).OrderBy(i=>i.com_img_adddate).ToList().Take(1).SingleOrDefault();
                    ViewBag.ant_trunk_img = imglists.com_img_src;
                    ViewBag.imglist = imglist;
                }
                else
                {
                    ViewBag.ant_trunk_img = "/file/default.jpg";
                    ViewBag.imglist = imglist;

                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }



        public ActionResult BuildingDetailesShowPic(string id)
        {
            try
            {

                ant_trunk tr = db.ant_trunk.Where(t => t.ant_trunk_id == id).SingleOrDefault();
                ViewBag.ant_trunk_title = tr.ant_trunk_title;
                var imglist = db.com_img.Where(i => i.com_img_fk == id).OrderBy(q => q.com_img_adddate).ToList();
                if (imglist.Count > 0)
                {
                    var imglists = db.com_img.Where(i => i.com_img_fk == id).ToList().Take(1).SingleOrDefault();
                    ViewBag.ant_trunk_img = imglists.com_img_src;
                    ViewBag.imglist = imglist;
                }
                else
                {
                    ViewBag.ant_trunk_img = "/file/default.jpg";
                    ViewBag.imglist = imglist;

                }
                return View();
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        public ActionResult BuildingMap(string id)
        {
            try
            {

                ant_trunk tr = db.ant_trunk.Where(t => t.ant_trunk_id == id).SingleOrDefault();
                ViewBag.ant_trunk_title = tr.ant_trunk_title;
                ant_class cl = db.ant_class.Where(c => c.ant_class_id == tr.ant_trunk_class_id).SingleOrDefault();
                ViewBag.ant_class_attribute = cl.ant_class_attribute;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult BuildingShowMap()
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
        #endregion

        #region 建筑文化
        public ActionResult BuildingCulture()
        {
            try
            {
                var query = db.ant_singlepage.Where(q => q.ant_singlepage_class == "culture" & q.ant_singlepage_stasus == "已启用").OrderByDescending(q => q.ant_singlepage_adddate).ToList();
                ViewBag.TotalPages = query.Count;
                var img = db.com_img.Where(i => i.com_img_fk == "JZWH").Take(1).SingleOrDefault();
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
            catch (Exception)
            {

                throw;
            }
        }

        public class ant_singlepages : ant_singlepage
        {
            public string com_img_src { get; set; }
        }
        public JsonResult Culture(string classstr, int CurPage)
        {
            try
            {
                var query = db.ant_singlepage.Where(q => q.ant_singlepage_class == classstr & q.ant_singlepage_stasus == "已启用").OrderByDescending(q => q.ant_singlepage_adddate).ToList();
                IList<ant_singlepage> querylist = new List<ant_singlepage>();
                foreach (var item in query)
                {
                    ant_singlepages s = new ant_singlepages();
                    s.ant_singlepage_id = item.ant_singlepage_id;
                    s.ant_singlepage_title = item.ant_singlepage_title.Length > 8 ? item.ant_singlepage_title.Substring(0, 8) + "…" : item.ant_singlepage_title;
                    s.ant_singlepage_description = item.ant_singlepage_description;
                    var img = db.com_img.Where(c => c.com_img_fk == item.ant_singlepage_id).SingleOrDefault();
                    if (img == null)
                    {
                        s.com_img_src = "/file/default.jpg";
                    }
                    else
                    {
                        s.com_img_src = img.com_img_src;
                    }
                    querylist.Add(s);
                }
                var pagelist = querylist.Take(20 * CurPage).Skip(20 * (CurPage - 1)).ToList();
                return Json(pagelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public ActionResult Detailes(string id)
        {
            try
            {
                ant_singlepage p = db.ant_singlepage.Where(s => s.ant_singlepage_id == id).SingleOrDefault();
                ViewBag.ant_singlepage_title = p.ant_singlepage_title;
                ViewBag.ant_singlepage_content = Server.HtmlDecode(p.ant_singlepage_content);
                var content = Server.HtmlDecode(p.ant_singlepage_content);
                var com_img_src = RegularExpression.GetImageUrlFromArticle(content);
                if (string.IsNullOrEmpty(com_img_src))
                {
                    ViewBag.com_img_src = "/file/default.jpg";
                }
                else
                {
                    ViewBag.com_img_src = com_img_src;
                }

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region 建筑资讯

        public ActionResult BuildingNews()
        {
            try
            {
                var query = db.ant_singlepage.Where(q => q.ant_singlepage_class == "news" & q.ant_singlepage_stasus == "已启用").OrderByDescending(q => q.ant_singlepage_adddate).ToList();
                ViewBag.TotalPages = query.Count;
                var img = db.com_img.Where(i => i.com_img_fk == "JZZX").Take(1).SingleOrDefault();
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
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
    }
}
