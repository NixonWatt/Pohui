using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pohui.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SetTheme(string theme)
        {
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
            return RedirectToAction("Index");
        }
        public ActionResult SetLanguage(string lang)
        {
            HttpCookie cookie = Request.Cookies["Lang"];
            if (cookie == null)
            {
                cookie = new HttpCookie("cookieValue");
                cookie.Name = "Lang";
                cookie.Value = lang;
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = lang;
                this.ControllerContext.HttpContext.Response.Cookies.Set(cookie);
            }
            return RedirectToAction("Index");
        }
    }
}
