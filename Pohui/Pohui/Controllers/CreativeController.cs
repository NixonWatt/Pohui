using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pohui.Models;

namespace Pohui.Controllers
{
    public class CreativeController : Controller
    {
        private Repository repository = new Repository();
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult UploadCreative()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadCreative(UploadedCreative uploadedCreative)
        {
            repository.AddNewCreative(uploadedCreative, User.Identity.Name);
            return RedirectToAction("Index","Home");
        }
    }
}
