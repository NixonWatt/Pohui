using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pohui.Models;
using Pohui.Filters;
using Pohui.Lucene;

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
        [OutputCache(Duration = 5)]
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
            var user = userRepository.FindFirstBy(m => m.Login == User.Identity.Name);
            user.CreativeCount++;
            userRepository.EditCreativeCount(user);
            LuceneCreativeSearch.AddUpdateLuceneIndex(newCreative);
            foreach (var tag in newCreative.Tags)
            {
                tagRepository.Create(tag);
                LuceneTagSearch.AddUpdateLuceneIndex(tag);
            }
            tagRepository.Save();
            Chapter newChapter = new Chapter
            {
                Name = "Chapter 0",
                Content = "",
                CreativeId = newCreative.Id,
                Position = 0
            };
            chapterRepository.Create(newChapter);
            chapterRepository.Save();
            LuceneChapterSearch.AddUpdateLuceneIndex(newChapter);
            newCreative = creativeRepository.FindFirstBy(n => n.Name == newCreative.Name && n.User == newCreative.User);
            var raki = "EditCreative"+"/" + newCreative.Id.ToString();
            return RedirectToAction(raki);
        }

        [Authorize]
        [OutputCache(Duration=20)]
        public ActionResult CheptersEditor(int id)
        {
            var chapter = chapterRepository.Find(id);
            return PartialView(chapter);
        }

        public EmptyResult SaveChapter(int id, string content)
        {
            var chapter = chapterRepository.FindFirstBy(m => m.Id== id);
            chapter.Content = content;
            chapterRepository.EditContent(chapter);
                chapterRepository.Save();
                LuceneChapterSearch.AddUpdateLuceneIndex(chapter);
            
            return null;
        } 

        [Authorize]
        [OutputCache(Duration=20)]
        public ActionResult EditCreative(int id)
        {
            return View(creativeRepository.Find(id));
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
            LuceneChapterSearch.AddUpdateLuceneIndex(newChapter);
            return RedirectToAction("ChapterEdit", "Creative");
        }

        public ActionResult AddLike(int id)
        {
            Like newLike = new Like
            {
                UserID = userRepository.FindFirstBy(m => m.Login == User.Identity.Name).UserId,
                CreativeID = id
            };
            if (likeRepository.FindFirstBy(m => (m.UserID == newLike.UserID) && (m.CreativeID == id)) == null)
            {
                likeRepository.Create(newLike);
            }
            else
            {
                var like = likeRepository.FindFirstBy(m => (m.UserID == newLike.UserID) && (m.CreativeID == id));
                likeRepository.Delete(like);
            }
            likeRepository.Save();
            creativeRepository.Find(id).Votes = likeRepository.FindAllBy(m => m.CreativeID == id).Count();
            creativeRepository.EditVotes(creativeRepository.Find(id));
            creativeRepository.Save();
            LuceneCreativeSearch.AddUpdateLuceneIndex(creativeRepository.Find(id));
            return PartialView(creativeRepository.Find(id));
        }

        [OutputCache(Duration = 2)]
        public ActionResult ViewAllChapters(int id)
        {
            return PartialView(chapterRepository.FindAllBy(m => m.CreativeId == id).OrderBy(m => m.Position));
        }

        [OutputCache(Duration = 2)]
        public ActionResult GetAllChapters(int id)
        {
            return PartialView(chapterRepository.FindAllBy(m => m.CreativeId == id).OrderBy(m => m.Position));
        }

        public EmptyResult AddEmptyChapter(int id)
        {
            Chapter newChapter = new Chapter
            {
                Name = "Chapter" + chapterRepository.FindAllBy(m => m.CreativeId == id).Count(),
                Content = "",
                Position = chapterRepository.FindAllBy(m => m.CreativeId == id).Count() + 1,
                CreativeId = id
            };
            chapterRepository.Create(newChapter);
            LuceneChapterSearch.AddUpdateLuceneIndex(newChapter);
            chapterRepository.Save();
            return null;
        }

        public EmptyResult EditPosition(int creativeId, int oldPos, int newPos)
        {
            var chapter = chapterRepository.FindFirstBy(m => m.CreativeId == creativeId && m.Position == oldPos);
            if (chapter != null)
            {
                chapterRepository.EditPosition(chapter, newPos);
                chapterRepository.Save();
                LuceneChapterSearch.AddUpdateLuceneIndex(chapter);
            }
            return null;
        }


        [OutputCache(Duration = 300)]
        public ActionResult ViewCreative(int id)
        {
            var creative = creativeRepository.Find(id);
            return View(creative);
        }

        [OutputCache(Duration = 5)] 
        public ActionResult ViewChapter(int id)
        {
            return PartialView(chapterRepository.Find(id));
        }

        [OutputCache(Duration = 60)]
        public ActionResult TagCloud()
        {
            var tags = tagRepository.GetAll().ToList();
            for (int i = 0; i < tags.Count(); i++)
            {
                for (int j = 0; j < tags.Count() - 1; j++)
                {
                    if (tags.ElementAt(i).Name == tags.ElementAt(j).Name)
                    {
                        tags.Remove(tags.ElementAt(j));
                    }
                }
            }
            return PartialView(tags);
        }

        public ActionResult ViewCreatives(int? id)
        {
            if (id == null)
                return PartialView(creativeRepository.GetAll().OrderByDescending(m => m.Votes));
            else
            {
                if (tagRepository.Find(id.Value) != null)
                {
                    var tag = tagRepository.Find(id.Value);
                    var creatives = tagRepository.FindAllBy(m => m.Name == tag.Name).Select(m => m.Creative).OrderByDescending(m => m.Votes);
                    return PartialView(creatives);
                }
                if (userRepository.Find(id.Value) != null)
                {
                    var user = userRepository.Find(id.Value);
                    var creatives = creativeRepository.FindAllBy(m => m.User == user.Login);
                    return PartialView(creatives);
                }
                return PartialView(creativeRepository.GetAll().OrderByDescending(m => m.Votes));
            }
        }
    }
}