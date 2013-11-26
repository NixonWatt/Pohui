using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public class ChapterRepository : IRepository
    {
        private readonly PohuiContext db;
        public ChapterRepository()
        {
            db = new PohuiContext();

        }

        public void Create(Chapter chapter)
        {
            db.Chapters.Add(chapter);
            var creative = SearchCreative(chapter.CreativeId);
            creative.Chapters.Add(chapter);
            Save();
        }

        public Creative SearchCreative(int id)
        {
            return (from creatives in db.Creatives where creatives.Id == id select creatives).FirstOrDefault();
        }

        public Chapter Search(int id)
        {
            return (from chapter in db.Chapters where chapter.Id == id select chapter).FirstOrDefault();
        }

        public void Delete(int id)
        {
            var chapter = Search(id);
            db.Chapters.Remove(chapter);
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }

    }

}