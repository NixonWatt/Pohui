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
        private readonly Repository<Tag> tagRepository = new TagRepository();
        private readonly Repository<Creative> creativeRepository = new CreativeRepository();
        private readonly Repository<Chapter> chapterRepository = new ChapterRepository();

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
            Creative newCreative = new Creative
            {
                Description = uploadedCreative.Description,
                Votes = 0,
                Name = uploadedCreative.Name,
                Tags = uploadedCreative.Tags.GetTagsFromText(),
                User = User.Identity.Name
            };
            creativeRepository.Add(newCreative);
            creativeRepository.Save();
            foreach(var tag in newCreative.Tags)
                tagRepository.Add(tag);
            tagRepository.Save();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ChaptersEdit(int id)
        {
            var chapter = chapterRepository.Find(id);
            return PartialView(chapter);
        }

        [Authorize]
        public ActionResult EditCreative(int id)
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
            chapterRepository.Add(newChapter);
            chapterRepository.Save();
            return RedirectToAction("ChapterEdit", "Creative");
        }
    }
}
