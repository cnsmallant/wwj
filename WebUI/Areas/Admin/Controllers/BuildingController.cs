using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFClassLibrary;


namespace WebUI.Areas.Admin.Controllers
{
    [AdminVerification]
    public class BuildingController : Controller
    {
        //
        // GET: /Admin/Building/
        D8WeChatEntities db = new D8WeChatEntities();
        public ActionResult Index()
        {
            return View();
        }

        #region 建筑区域
        [Serializable]
        public class ant_classes : ant_class
        {
            public string com_img_src { get; set; }
        }

        public ActionResult BuildingArea()
        {
            try
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
                ViewBag.list = cllist;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult BuildingAreaCreate()
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
        public ActionResult BuildingAreaAdd()
        {
            try
            {
                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                ant_class cl = new ant_class();
                cl.ant_class_id = Guid.NewGuid().ToString("N").ToUpper();
                cl.ant_class_name = Request.Form["ant_class_name"];
                cl.ant_class_attribute = Request.Form["ant_class_attribute"];
                cl.ant_class_adduser = ant_admin.ant_admin_id;
                cl.ant_class_adddate = DateTime.Now;
                cl.ant_class_edituser = ant_admin.ant_admin_id;
                cl.ant_class_editdate = DateTime.Now;
                db.ant_class.Add(cl);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return RedirectToAction("BuildingArea", "Building");
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

        public ActionResult BuildingAreaModify(string ant_class_id)
        {
            try
            {
                ant_class cl = db.ant_class.Where(c => c.ant_class_id == ant_class_id).SingleOrDefault();
                ViewBag.ant_class_name = cl.ant_class_name;
                ViewBag.ant_class_attribute = cl.ant_class_attribute;
                ViewBag.ant_class_id = cl.ant_class_id;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult BuildingAreaEdit()
        {
            try
            {
                var ant_class_id = Request.Form["ant_class_id"];
                ant_class cl = db.ant_class.Where(c => c.ant_class_id == ant_class_id).SingleOrDefault();
                cl.ant_class_name = Request.Form["ant_class_name"];
                cl.ant_class_attribute = Request.Form["ant_class_attribute"];
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("BuildingArea", "Building");
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

        [HttpGet]
        public ActionResult BuildingAreaDel(string ant_class_id)
        {
            try
            {
                ant_class cl = db.ant_class.Where(c => c.ant_class_id == ant_class_id).SingleOrDefault();
                db.ant_class.Remove(cl);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return RedirectToAction("BuildingArea", "Building");
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
        #endregion



        #region 建筑列表
        public class ant_trunks : ant_trunk
        {
            public string ant_trunk_stasuses { get; set; }
        }
        /// <summary>
        /// 建筑列表
        /// </summary>
        /// <returns></returns>
        public ActionResult BuildingList(int pageid = 1)
        {
            ViewData["ant_class"] = from a in db.ant_class.OrderBy(c => c.ant_class_adddate).ToList()
                                    select new SelectListItem
                                    {
                                        Text = a.ant_class_name,
                                        Value = a.ant_class_id
                                    };

            var query = db.ant_trunk;
            var list = query.OrderByDescending(q => q.ant_trunk_order).ToList();
            IList<ant_trunks> tlist = new List<ant_trunks>();
            foreach (var item in list)
            {
                ant_trunks tr = new ant_trunks();
                tr.ant_trunk_id = item.ant_trunk_id;
                tr.ant_trunk_title = item.ant_trunk_title;
                tr.ant_trunk_entitle = item.ant_trunk_entitle;
                tr.ant_trunk_code = item.ant_trunk_code;
                tr.ant_trunk_summary = item.ant_trunk_summary;
                tr.ant_content_tags = item.ant_content_tags;
                tr.ant_trunk_area = item.ant_trunk_area;
                tr.ant_trunk_stasuses = stasus(item.ant_trunk_stasus);
                tr.ant_trunk_class_id = db.ant_class.Where(c => c.ant_class_id == item.ant_trunk_class_id).SingleOrDefault().ant_class_name;
                tr.ant_trunk_adduser = db.ant_admin.Where(a => a.ant_admin_id == item.ant_trunk_adduser).SingleOrDefault().ant_admin_name;
                tr.ant_trunk_adddate = item.ant_trunk_adddate;
                tlist.Add(tr);
            }
            IList<ant_trunks> mPage = PageCommon.PageList<ant_trunks>(pageid, 20, tlist.Count, tlist);
            return View("BuildingList", mPage);
        }


        public ActionResult BuildingClass(string classid, int pageid = 1)
        {
            try
            {
                ViewData["ant_class"] = from a in db.ant_class.OrderBy(c => c.ant_class_adddate).ToList()
                                        select new SelectListItem
                                        {
                                            Text = a.ant_class_name,
                                            Value = a.ant_class_id
                                        };

                var query = db.ant_trunk;
                var list = query.Where(q => q.ant_trunk_class_id == classid).OrderByDescending(q => q.ant_trunk_order).ToList();
                IList<ant_trunks> tlist = new List<ant_trunks>();
                foreach (var item in list)
                {
                    ant_trunks tr = new ant_trunks();
                    tr.ant_trunk_id = item.ant_trunk_id;
                    tr.ant_trunk_title = item.ant_trunk_title;
                    tr.ant_trunk_entitle = item.ant_trunk_entitle;
                    tr.ant_trunk_code = item.ant_trunk_code;
                    tr.ant_trunk_summary = item.ant_trunk_summary;
                    tr.ant_content_tags = item.ant_content_tags;
                    tr.ant_trunk_area = item.ant_trunk_area;
                    tr.ant_trunk_stasuses = stasus(item.ant_trunk_stasus);
                    tr.ant_trunk_class_id = db.ant_class.Where(c => c.ant_class_id == item.ant_trunk_class_id).SingleOrDefault().ant_class_name;
                    tr.ant_trunk_adduser = db.ant_admin.Where(a => a.ant_admin_id == item.ant_trunk_adduser).SingleOrDefault().ant_admin_name;
                    tr.ant_trunk_adddate = item.ant_trunk_adddate;
                    tlist.Add(tr);
                }
                IList<ant_trunks> mPage = PageCommon.PageList<ant_trunks>(pageid, 20, tlist.Count, tlist);
                return View("BuildingList", mPage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string stasus(int? stasus)
        {
            string str = string.Empty;
            if (stasus == 0)
            {
                str = "未启用";
            }
            if (stasus == 1)
            {
                str = "未推荐";
            }
            if (stasus == 2)
            {
                str = "已推荐";
            }
            return str;
        }
        public ActionResult BuildingCreate()
        {
            try
            {
                ViewData["ant_class"] = from a in db.ant_class.OrderBy(c => c.ant_class_adddate).ToList()
                                        select new SelectListItem
                                        {
                                            Text = a.ant_class_name,
                                            Value = a.ant_class_id
                                        };
                return View();

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BuildingAdd()
        {
            try
            {
                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                ant_trunk tr = new ant_trunk();
                tr.ant_trunk_class_id = Request.Form["ant_trunk_class_id"];
                tr.ant_trunk_id = Guid.NewGuid().ToString("N").ToUpper();
                tr.ant_trunk_code = Request.Form["ant_trunk_code"];
                tr.ant_trunk_title = Request.Form["ant_trunk_title"];
                tr.ant_trunk_entitle = Request.Form["ant_trunk_entitle"];
                tr.ant_trunk_summary = Request.Form["ant_trunk_summary"];
                tr.ant_content_tags = Request.Form["ant_content_tags"];
                tr.ant_trunk_area = Request.Form["ant_trunk_area"];
                tr.ant_trunk_order = Convert.ToInt32(Request.Form["ant_trunk_order"].ToString());
                tr.ant_trunk_content = Server.HtmlEncode(Request.Form["ant_trunk_content"].ToString());
                tr.ant_trunk_stasus = 0;
                tr.ant_trunk_adduser = ant_admin.ant_admin_id;
                tr.ant_trunk_adddate = DateTime.Now;
                tr.ant_trunk_edituser = ant_admin.ant_admin_id;
                tr.ant_trunk_editdate = DateTime.Now;
                db.ant_trunk.Add(tr);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return RedirectToAction("BuildingList", "Building");
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

        public ActionResult BuildingModify(string ant_trunk_id)
        {
            try
            {
                ant_trunk tr = db.ant_trunk.Where(t => t.ant_trunk_id == ant_trunk_id).SingleOrDefault();
                var cl = db.ant_class.OrderBy(c => c.ant_class_adddate).ToList();
                SelectList selList = new SelectList(cl, "ant_class_id", "ant_class_name", tr.ant_trunk_class_id);
                ViewData["ant_class"] = selList;
                ViewBag.ant_trunk_id = tr.ant_trunk_id;
                ViewBag.ant_trunk_code = tr.ant_trunk_code;
                ViewBag.ant_trunk_title = tr.ant_trunk_title;
                ViewBag.ant_trunk_entitle = tr.ant_trunk_entitle;
                ViewBag.ant_trunk_summary = tr.ant_trunk_summary;
                ViewBag.ant_content_tags = tr.ant_content_tags;
                ViewBag.ant_trunk_area = tr.ant_trunk_area;
                ViewBag.ant_trunk_order = tr.ant_trunk_order;
                ViewBag.ant_trunk_content =Server.HtmlDecode(tr.ant_trunk_content);
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BuildingEdit()
        {
            try
            {

                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                string ant_trunk_id = Request.Form["ant_trunk_id"];
                ant_trunk tr = db.ant_trunk.Where(t => t.ant_trunk_id == ant_trunk_id).SingleOrDefault();
                var ant_trunk_class_id = Request.Form["ant_trunk_class_id"];
                if (!string.IsNullOrEmpty(ant_trunk_class_id))
                {
                    tr.ant_trunk_class_id = ant_trunk_class_id;
                }
                tr.ant_trunk_code = Request.Form["ant_trunk_code"];
                tr.ant_trunk_title = Request.Form["ant_trunk_title"];
                tr.ant_trunk_entitle = Request.Form["ant_trunk_entitle"];
                tr.ant_trunk_summary = Request.Form["ant_trunk_summary"];
                tr.ant_content_tags = Request.Form["ant_content_tags"];
                tr.ant_trunk_area = Request.Form["ant_trunk_area"];
                tr.ant_trunk_order = Convert.ToInt32(Request.Form["ant_trunk_order"].ToString());
                tr.ant_trunk_content = Server.HtmlEncode(Request.Form["ant_trunk_content"].ToString());
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("BuildingList", "Building");
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


        public ActionResult BuildingDel(string ant_trunk_id)
        {
            try
            {
                ant_trunk tr = db.ant_trunk.Where(t => t.ant_trunk_id == ant_trunk_id).SingleOrDefault();
                db.ant_trunk.Remove(tr);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return RedirectToAction("BuildingList", "Building");
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

        public ActionResult BuildingStasus(string ant_trunk_id, string ant_trunk_stasus)
        {
            ant_trunk tr = db.ant_trunk.Where(t => t.ant_trunk_id == ant_trunk_id).SingleOrDefault();
            tr.ant_trunk_stasus = Convert.ToInt32(ant_trunk_stasus);
            int result = db.SaveChanges();
            if (result > 0)
            {
                return RedirectToAction("BuildingList", "Building");
            }
            else
            {
                string msg = Server.UrlEncode("操作失败！");
                return RedirectToAction("Index", "Message", new { mid = msg });
            }
        }
        #endregion


        #region 建筑文化

        public ActionResult AntSinglepage(string classid, int pageid = 1)
        {
            try
            {
                var query = db.ant_singlepage;
                var list = query.Where(q => q.ant_singlepage_class == classid).OrderByDescending(q => q.ant_singlepage_adddate).ToList();
                IList<ant_singlepage> tlist = new List<ant_singlepage>();
                ViewBag.title = Title(classid);
                foreach (var item in list)
                {
                    ant_singlepage si = new ant_singlepage();
                    si.ant_singlepage_id = item.ant_singlepage_id;
                    si.ant_singlepage_title = item.ant_singlepage_title;
                    si.ant_singlepage_stasus = item.ant_singlepage_stasus;
                    si.ant_singlepage_adduser = db.ant_admin.Where(a => a.ant_admin_id == item.ant_singlepage_adduser).SingleOrDefault().ant_admin_name;
                    si.ant_singlepage_adddate = item.ant_singlepage_adddate;
                    tlist.Add(si);
                }
                IList<ant_singlepage> mPage = PageCommon.PageList<ant_singlepage>(pageid, 20, tlist.Count, tlist);
                return View("AntSinglepage", mPage);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AntSinglepageCreate(string classid)
        {
            try
            {
                ViewBag.title = Title(classid);
                ViewBag.classid = classid;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AntSinglepageAdd()
        {
            try
            {
                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                string classid = Request.Form["classid"];

                ant_singlepage si = new ant_singlepage();
                si.ant_singlepage_id = Guid.NewGuid().ToString("N").ToUpper();
                si.ant_singlepage_class = classid;
                si.ant_singlepage_stasus = "未启用";
                si.ant_singlepage_title = Request.Form["ant_singlepage_title"];
                si.ant_singlepage_content = Server.HtmlEncode(Request.Form["ant_singlepage_content"]);
                si.ant_singlepage_description = RegularExpression.NullHtmlStr(Request.Form["ant_singlepage_description"]);
                si.ant_singlepage_adddate = DateTime.Now;
                si.ant_singlepage_adduser = ant_admin.ant_admin_id;

                db.ant_singlepage.Add(si);

                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("AntSinglepage", "Building", new { classid = classid });
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

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="ant_singlepage_id"></param>
        /// <param name="classid"></param>
        /// <param name="ant_singlepage_stasus"></param>
        /// <returns></returns>
        public ActionResult AntSinglepageStasus(string ant_singlepage_id, string classid, string ant_singlepage_stasus)
        {
            try
            {
                ant_singlepage si = db.ant_singlepage.Where(s => s.ant_singlepage_id == ant_singlepage_id).SingleOrDefault();
                si.ant_singlepage_stasus = Server.UrlDecode(ant_singlepage_stasus);
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("AntSinglepage", "Building", new { classid = classid });
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

        public ActionResult AntSinglepageModify(string ant_singlepage_id, string classid)
        {
            try
            {
                ViewBag.title = Title(classid);
                ant_singlepage si = db.ant_singlepage.Where(s => s.ant_singlepage_id == ant_singlepage_id).SingleOrDefault();
                ViewBag.ant_singlepage_id = si.ant_singlepage_id;
                ViewBag.ant_singlepage_class = classid;
                ViewBag.ant_singlepage_title = si.ant_singlepage_title;
                ViewBag.ant_singlepage_content = Server.HtmlDecode(si.ant_singlepage_content);
                ViewBag.ant_singlepage_description = si.ant_singlepage_description;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AntSinglepageEdit()
        {
            try
            {
                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                string classid = Request.Form["classid"];
                string ant_singlepage_id = Request.Form["ant_singlepage_id"];
                ant_singlepage si = db.ant_singlepage.Where(s => s.ant_singlepage_id == ant_singlepage_id).SingleOrDefault();
                si.ant_singlepage_title = Request.Form["ant_singlepage_title"];
                si.ant_singlepage_content = Server.HtmlEncode(Request.Form["ant_singlepage_content"]);
                si.ant_singlepage_description = RegularExpression.NullHtmlStr(Request.Form["ant_singlepage_description"]);
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("AntSinglepage", "Building", new { classid = classid });
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

        public ActionResult AntSinglepageDel(string ant_singlepage_id, string classid)
        {
            try
            {
                ant_singlepage si = db.ant_singlepage.Where(s => s.ant_singlepage_id == ant_singlepage_id).SingleOrDefault();
                db.ant_singlepage.Remove(si);
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("AntSinglepage", "Building", new { classid = classid });
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



        public ActionResult AntSinglepageContactUs()
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AntSinglepageContactUsAdd()
        {
            try
            {
                var uname = TDESHelper.DecryptString(HttpContext.Request.Cookies["uname"].Value);
                var upwd = HttpContext.Request.Cookies["upwd"].Value;
                var ant_admin = db.ant_admin.Where(u => u.ant_admin_name == uname & u.ant_admin_pwd == upwd).SingleOrDefault();
                ant_singlepage sis = db.ant_singlepage.Where(s => s.ant_singlepage_class == "tel").SingleOrDefault();
                if (sis == null)
                {
                    ant_singlepage si = new ant_singlepage();
                    si.ant_singlepage_id = Guid.NewGuid().ToString("N").ToUpper();
                    si.ant_singlepage_class = "tel";

                    si.ant_singlepage_title = Request.Form["ant_singlepage_title"];
                    si.ant_singlepage_content = Server.HtmlEncode(Request.Form["ant_singlepage_content"]);
                    si.ant_singlepage_description = RegularExpression.NullHtmlStr(Request.Form["ant_singlepage_description"]);
                    si.ant_singlepage_adddate = DateTime.Now;
                    si.ant_singlepage_adduser = ant_admin.ant_admin_id;
                    db.ant_singlepage.Add(si);
                }
                else
                {
                   
                    ant_singlepage si = db.ant_singlepage.Where(s => s.ant_singlepage_class == "tel").SingleOrDefault();
                    si.ant_singlepage_title = Request.Form["ant_singlepage_title"];
                    si.ant_singlepage_content = Server.HtmlEncode(Request.Form["ant_singlepage_content"]);
                    si.ant_singlepage_description = RegularExpression.NullHtmlStr(Request.Form["ant_singlepage_description"]);
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                if (result > 0)
                {
                    return RedirectToAction("AntSinglepageContactUs", "Building");
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



        private string Title(string classid)
        {
            string title = string.Empty;
            if (classid == "culture")
            {
                title = "建筑文化";
            }
            if (classid == "news")
            {
                title = "建筑资讯";
            }
            if (classid == "line")
            {
                title = "推荐路线";
            }

            return title;

        }


        #endregion


        #region 意见反馈

        public ActionResult Feedback(int pageid = 1)
        {
            try
            {
                var query = db.ant_feedback;
                var list = query.OrderByDescending(q => q.ant_feedback_adddate).ToList();
                IList<ant_feedback> mPage = PageCommon.PageList<ant_feedback>(pageid, 20, list.Count, list);
                return View("Feedback", mPage);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult FeedbackDel(string ant_feedback_id)
        {
            try
            {
                ant_feedback tr = db.ant_feedback.Where(t => t.ant_feedback_id == ant_feedback_id).SingleOrDefault();
                db.ant_feedback.Remove(tr);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return RedirectToAction("Feedback", "Building");
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
        #endregion
    }
}
