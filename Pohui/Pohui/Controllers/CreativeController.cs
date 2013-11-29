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
        private readonly ITag tagRepository;
        private readonly ICreative creativeRepository;
        private readonly IChapter chapterRepository;

        public CreativeController(ITag tag, ICreative creative, IChapter chapter)
        {
            this.tagRepository = tag;
            this.creativeRepository = creative;
            this.chapterRepository = chapter;

        }

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
            creativeRepository.Create(newCreative);
            creativeRepository.Save();
            foreach(var tag in newCreative.Tags)
                tagRepository.Create(tag);
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
            chapterRepository.Create(newChapter);
            chapterRepository.Save();
            return RedirectToAction("ChapterEdit", "Creative");
        }
    }
}
