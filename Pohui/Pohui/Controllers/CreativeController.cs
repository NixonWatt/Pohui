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
        private readonly IUser userRepository;
        private readonly ILike likeRepository;

        public CreativeController()
        {

        }

        public CreativeController(ITag tag, ICreative creative, IChapter chapter, IUser user, ILike like)
        {
            this.tagRepository = tag;
            this.creativeRepository = creative;
            this.chapterRepository = chapter;
            this.userRepository = user;
            this.likeRepository = like;
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
            Chapter newChapter = new Chapter
            {
                Name = "Глава 1",
                Content = "",
                CreativeId = newCreative.Id,
                Position = 1
            };
            chapterRepository.Create(newChapter);
            return RedirectToAction("EditCreative\\" + newCreative.Id);
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

        public ActionResult AddLike(int id)
        {
            Like newLike = new Like
            {
                Users = userRepository.FindFirstBy(m => m.Login == User.Identity.Name),
                UserID = userRepository.FindFirstBy(m => m.Login == User.Identity.Name).UserId,
                Creatives = creativeRepository.Find(id),
                CreativeID = id
            };
            likeRepository.Create(newLike);
            likeRepository.Save();
            creativeRepository.Find(id).Votes = likeRepository.FindAllBy(m => m.CreativeID == id).Count();
            creativeRepository.EditVotes(creativeRepository.Find(id));
            creativeRepository.Save();
            return PartialView(creativeRepository.Find(id));
        }
    }
}
