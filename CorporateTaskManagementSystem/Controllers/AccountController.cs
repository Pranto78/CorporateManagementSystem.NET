using System.Web.Mvc;
using System.Web.Security;
using CorporateTaskManagementSystem.BLL.Services;
using CorporateTaskManagementSystem.Common.Enums;
using CorporateTaskManagementSystem.Web.ViewModels;

namespace CorporateTaskManagementSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService authService = new AuthService();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = authService.Login(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);

            Session["UserId"] = user.UserId;
            Session["FullName"] = user.FullName;
            Session["Email"] = user.Email;
            Session["RoleId"] = user.RoleId;
            Session["RoleName"] = user.Role.RoleName;

            if (user.RoleId == (int)UserRole.Admin)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.RoleId == (int)UserRole.Leader)
            {
                return RedirectToAction("Index", "Leader");
            }
            else if (user.RoleId == (int)UserRole.Employee)
            {
                return RedirectToAction("Index", "Employee");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }
    }
}