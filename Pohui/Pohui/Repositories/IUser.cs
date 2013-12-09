using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pohui.Models
{
    public interface IUser : IRepository<User>
    {
        void SetAdminRole(int id);
        void DropPassword(int id);
        void EditCreativeCount(User entity);
    }
}