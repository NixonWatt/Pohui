using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public class CreativeRepository : IRepository
    {
        private readonly PohuiContext db;
        public CreativeRepository()
        {
            db = new PohuiContext();

        }

        public void Create(UploadedCreative creative, string userName)
        {
            Creative newCreative = new Creative
            {
                Description = creative.Description,
                Votes = 0,
                Name = creative.Name,
                Tags = creative.Tags.GetTagsFromText(),
                User = userName
            };
            db.Creatives.Add(newCreative);
            foreach (var uploadedTag in newCreative.Tags)
                db.Tags.Add(uploadedTag);
            Save();
        }

        public void Delete(int id)
        {
            var creative = Search(id);
            db.Creatives.Remove(creative);
            Save();
        }

        public Creative Search(int id)
        {
            return (from creatives in db.Creatives where creatives.Id == id select creatives).FirstOrDefault();
        }

        public void Save()
        {
            db.SaveChanges();
        }
            //create
            //find by
            //find one
            //find all
            //delete
            //update


        void IRepository.Save()
        {
            throw new NotImplementedException();
        }

        void IRepository.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}