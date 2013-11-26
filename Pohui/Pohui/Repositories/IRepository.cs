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
    public interface IRepository
    {
        void Save();
        void Delete(int id);
    }
}