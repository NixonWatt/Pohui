using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

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

        public ICollection<User> GetAllUsers()
        {
            var users = (from user in db.UserProfiles select user).ToList();
            return users;
        }

        public User GetUserById(int id)
        {
            var user = (from users in db.UserProfiles where users.UserId == id select users).FirstOrDefault();
            return user;
        }

        public void DeleteUserById(int id)
        {
            var user = (from users in db.UserProfiles where users.UserId == id select users).FirstOrDefault();
            db.UserProfiles.Remove(user);
            db.SaveChanges();
        }

        public void Admin(int id)
        {
            var user = (from users in db.UserProfiles where users.UserId == id select users).FirstOrDefault();
            if (Roles.IsUserInRole(user.Login, "Admin"))
                Roles.RemoveUserFromRole(user.Login, "Admin");
            else
                Roles.AddUserToRole(user.Login, "Admin");
        }

    }
}