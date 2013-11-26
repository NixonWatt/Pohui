using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkdownDeepSample.Controllers
{
	[HandleError]
	public class MarkdownDeepController : Controller
	{
		
		public ActionResult Index()
		{
			// View the user editable content

			// Create and setup Markdown translator
			var md = new MarkdownDeep.Markdown();
			md.SafeMode = true;
			md.ExtraMode = true;
			return View();
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Edit()
		{
			// For editing the content, just pass the raw Markdown to the view
			
			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit(string content)
		{
			// Save the content and switch back to the main view
			
			return RedirectToAction("Index");
		}

		public ActionResult About()
		{
			// Nothing special here
			return View();
		}

		public ActionResult QuickRef()
		{
			// This view translates the readme file that is installed by the MarkdownDeep NuGet package.

			// The readme is in Markdown format so this is a good example of just rendering content that
			// has been authored in markdown.

			// The view uses the extension method defined in HtmlHelperExtensions.cs to load and translate
			// a specified file.

			return View();
		}
	}
}
