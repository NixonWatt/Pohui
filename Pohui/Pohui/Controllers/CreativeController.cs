using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pohui.Models;
using Pohui.Filters;

namespace Pohui.Controllers
{
    [Culture]
    public class CreativeController : Controller
    {
        private Repository repository = new Repository();
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult UploadCreative()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadCreative(UploadedCreative uploadedCreative)
        {
            repository.AddNewCreative(uploadedCreative, User.Identity.Name);
            return RedirectToAction("ChaptersEdit", "Creative");
        }
        [Authorize]
        public ActionResult ChaptersEdit()
        {
            return View();
        }

        [Authorize]
        public ActionResult UploadChapter(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadChapter(Chapter newChapter)
        {
            repository.AddNewChapter(newChapter);
            return RedirectToAction("ChapterEdit", "Creative");
        }

        
    }
}
