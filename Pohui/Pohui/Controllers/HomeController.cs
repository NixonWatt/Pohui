using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pohui.Filters;
using Pohui.Models;
using Pohui.Lucene;

namespace Pohui.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private readonly ICreative creativeRepository;
        private readonly IChapter chapterRepository;
        public HomeController()
        {

        }

        public HomeController(ITag tag, ICreative creative, IChapter chapter, IUser user, ILike like)
        {         
            this.creativeRepository = creative;
            this.chapterRepository = chapter;
        }

        [OutputCache(Duration = 1)]
        public ActionResult Index()
        {
          
            return View(creativeRepository.GetAll());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult ViewCreativeText(int CreativeId)
        {
            var chapter = chapterRepository.Find(CreativeId);
            return PartialView("_Fotorama"); 
        }
    
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SetTheme(string theme)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            HttpCookie cookie = Request.Cookies["Theme"];
            if (cookie == null)
            {
                cookie = new HttpCookie("cookieValue");
                cookie.Name = "Theme";
                cookie.Value = theme;
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = theme;
                this.ControllerContext.HttpContext.Response.Cookies.Set(cookie);
            }
            return Redirect(returnUrl);
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            HttpCookie cookie = Request.Cookies["Lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {
                cookie = new HttpCookie("Lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        [OutputCache(Duration=5)]
        public ActionResult Search(string searchString)
        {
            SearchResult searchResult = new SearchResult
            {
                chapters = LuceneChapterSearch.Search(searchString),
                creatives = LuceneCreativeSearch.Search(searchString),
                tags = LuceneTagSearch.Search(searchString),
                users = LuceneUserSearch.Search(searchString),
            };
            ViewBag.asd = searchString;
            return View(searchResult);
        }
    }
}
