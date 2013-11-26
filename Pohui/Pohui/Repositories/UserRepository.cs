using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace Pohui.Models
{
    public class UserRepository : IRepository
    {
        private readonly PohuiContext db;
        public UserRepository()
        {
            db = new PohuiContext();

        }

        public void Delete(int id)
        {
            var user = Search(id);
            db.UserProfiles.Remove(user);
            Save();
        }

        public User Search(int id)
        {
            return (from users in db.UserProfiles where users.UserId == id select users).FirstOrDefault();
        }

        public ICollection<User> Search()
        {
            return (from users in db.UserProfiles select users).ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void SetAdminRole(int id)
        {
            var user = Search(id);
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
            var user = Search(id);
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
            var user = Search(id);
            var token = WebSecurity.GeneratePasswordResetToken(user.Login);
            WebSecurity.ResetPassword(token, "droppedpassword");
        }
    }

}