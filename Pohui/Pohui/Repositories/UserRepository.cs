using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
namespace Pohui.Models
{
        public class UserRepository : Repository<User>, IUser
        {
            void IUser.SetAdminRole(int id)
            {
                var user = Find(id);
                if (isAdmin(id))
                {
                    Roles.RemoveUserFromRole(user.Login, "Admin");
                }
                else
                {
                    Roles.AddUserToRole(user.Login, "Admin");
                }
            }

            private bool isAdmin(int id)
            {
                var user = Find(id);
                if (Roles.IsUserInRole(user.Login, "Admin"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            void IUser.DropPassword(int id)
            {
                var user = Find(id);
                var token = WebSecurity.GeneratePasswordResetToken(user.Login);
                WebSecurity.ResetPassword(token, "droppedpassword");
            }

        }
    
}