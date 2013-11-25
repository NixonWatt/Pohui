using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace Pohui.Models
{
    public class Repository
    {
        private readonly PohuiContext db;
        public Repository()
        {
            db = new PohuiContext();
        }

        public void AddNewCreative(UploadedCreative creative, string userName)
        {
            User usr = (from user in db.UserProfiles where user.Login == userName select user).FirstOrDefault();
            Creative newCreative = new Creative
            {
                Description = creative.Description,
                Votes = 0,
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
            var user = GetUserById(id);
            db.UserProfiles.Remove(user);
            db.SaveChanges();
        }

        public void SetAdminRole(int id)
        {
            var user = GetUserById(id);
            if (isAdmin(id))
            {
                Roles.RemoveUserFromRole(user.Login, "Admin");
            }
            else
            {
                Roles.AddUserToRole(user.Login, "Admin");
            }
            
        }

        public bool isAdmin(int id)
        {
            var user = GetUserById(id);
            if (Roles.IsUserInRole(user.Login, "Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DropPassword(int id)
        {
            var user = GetUserById(id);
            var token = WebSecurity.GeneratePasswordResetToken(user.Login);
            WebSecurity.ResetPassword(token, "droppedpassword");
        }

        public void AddNewChapter(Chapter chapter)
        {
            db.Chapters.Add(chapter);
            var creative = (from creatives in db.Creatives where creatives.Id == chapter.CreativeId select creatives).FirstOrDefault();
            creative.Chapters.Add(chapter);
            db.SaveChanges();
        }
    }
}