using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using Pohui.Models;
using Pohui.Filters;
using Pohui.Lucene;
using System.Net.Mail;
using System.Net;
using System.Web.Caching;

namespace Pohui.Controllers
{
    [Culture]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private readonly IUser repository;
        //
        // GET: /Account/Login
        public AccountController()
        {

        }

        public AccountController(IUser user)
        {
            this.repository = user;
        }

        [AllowAnonymous]
        [OutputCache(Duration = 1)]
        public ActionResult Login(string returnUrl)
        {
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.Login, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        [OutputCache(Duration = 3600)]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Попытка зарегистрировать пользователя
                try
                {
                    WebSecurity.CreateUserAndAccount(model.Login, model.Password, new { Email = model.Email, CreativeCount = 0 });
                    if (model.Login == "Admin")
                        Roles.AddUserToRole("Admin", "Admin");
                    else Roles.AddUserToRole(model.Login, "User");
                    WebSecurity.Login(model.Login, model.Password);
                    SendMail(model);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }
     
        public static void SendMail(RegisterModel model)
        {
            using (MailMessage mail = new MailMessage("nixon31031@gmail.com", model.Email))
            {
                mail.Subject = "";
                string token = Membership.GeneratePassword(10, 5);

                mail.Body = "Перейдите по ссылке " + "http://localhost:2089/confirm?" + model.Login + "&" + token;

                using (SmtpClient sc = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("nixon31031@gmail.com", "220_360ass")
                })
                {
                    sc.Send(mail);
                }
            }
        }
        //
        // GET: /Account/Manage

        [OutputCache()]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Пароль изменен."
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
                : message == ManageMessageId.RemoveLoginSuccess ? "Внешняя учетная запись удалена."
                : "";
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (ModelState.IsValid)
            {
                // В ряде случаев при сбое ChangePassword породит исключение, а не вернет false.
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный текущий пароль или недопустимый новый пароль.");
                }
            }
            return View(model);
        }

        [OutputCache(Duration = 120)]
        [Authorize(Roles = ("Admin"))]
        public ActionResult Administration()
        {
            var users = repository.GetAll();
            LuceneUserSearch.AddUpdateLuceneIndex(users);
            return View(users.ToList());
        }

        [Authorize]
        [OutputCache(Duration=2)]
        public ActionResult ProfilePage(string userName)
        {
            var user = repository.FindFirstBy(m => m.Login == userName);
            return View(user);
        }

        [Authorize(Roles = ("Admin"))]
        public ActionResult Delete(int id)
        {
            repository.Delete(repository.Find(id));
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = ("Admin"))]
        public ActionResult SetAdmin(int id)
        {
            repository.SetAdminRole(id);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles=("Admin"))]
        public ActionResult DropPassword(int id)
        {
            repository.DropPassword(id);
            return RedirectToAction("Index", "Home");
        }
        #region Вспомогательные методы
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
            //.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }
        #endregion
    }
}
