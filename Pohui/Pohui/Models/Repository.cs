using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Pohui.Models
{
    public class Repository
    {
        private readonly CreativeContext db;
        public Repository()
        {
            db = new CreativeContext();
        }


        public void AddNewCreative(UploadedCreative creative, string userName)
        {
            User usr = (from user in db.UserProfiles where user.Login == userName select user).FirstOrDefault();
            Creative newCreative = new Creative
            {
                Description = creative.Description,
                Votes = 0,
                Path = creative.Name + "/",
                Name = creative.Name,
                Tags = creative.Tags.GetTagsFromText(),
                User = usr.Login
            };
            db.Creatives.Add(newCreative);
            foreach (var uploadedTag in newCreative.Tags)
                db.Tags.Add(uploadedTag);
            db.SaveChanges();
        }
    }
}