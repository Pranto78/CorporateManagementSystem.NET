using System;
using System.Web.Mvc;
using CorporateTaskManagementSystem.BLL.Services;
using CorporateTaskManagementSystem.Entities.Models;
using CorporateTaskManagementSystem.Web.ViewModels;

using CorporateTaskManagementSystem.Common.Enums;

namespace CorporateTaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserService userService = new UserService();
        private readonly TaskService taskService = new TaskService();

        public ActionResult Index()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AdminDashboardViewModel
            {
                TotalUsers = userService.GetTotalUsersCount(),
                ActiveUsers = userService.GetActiveUsersCount(),

                TotalAssignedTasks = taskService.GetTotalAssignedTasks(),
                PendingTasks = taskService.GetTaskCountByStatus(TaskStatus.Pending),
                InProgressTasks = taskService.GetTaskCountByStatus(TaskStatus.InProgress),
                SubmittedTasks = taskService.GetTaskCountByStatus(TaskStatus.Submitted),
                CompletedTasks = taskService.GetTaskCountByStatus(TaskStatus.Completed),
                RejectedTasks = taskService.GetTaskCountByStatus(TaskStatus.Rejected)
            };

            return View(model);
        }

        public ActionResult Users(string searchText, int? departmentId)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var users = userService.GetAllUsers(searchText, departmentId);

            ViewBag.Departments = userService.GetActiveDepartments();
            ViewBag.SearchText = searchText;
            ViewBag.DepartmentId = departmentId;

            return View(users);
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Roles = userService.GetUserCreateRoles();
            ViewBag.Departments = userService.GetActiveDepartments();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserCreateViewModel model)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Roles = userService.GetUserCreateRoles();
            ViewBag.Departments = userService.GetActiveDepartments();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (userService.EmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "This email already exists.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                RoleId = model.RoleId,
                DepartmentId = model.DepartmentId
            };

            userService.CreateUser(user, model.Password);

            TempData["SuccessMessage"] = "User created successfully.";

            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateUser(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            bool isUpdated = userService.DeactivateUser(id);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "User deactivated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User could not be deactivated. Admin user cannot be deactivated.";
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateUser(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            bool isUpdated = userService.ActivateUser(id);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "User activated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User could not be activated.";
            }

            return RedirectToAction("Users");
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var user = userService.GetUserById(id);

            if (user == null || user.RoleId == 1)
            {
                TempData["ErrorMessage"] = "Admin user cannot be edited or user not found.";
                return RedirectToAction("Users");
            }

            ViewBag.Roles = userService.GetUserCreateRoles();
            ViewBag.Departments = userService.GetActiveDepartments();

            var model = new UserEditViewModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                RoleId = user.RoleId,
                DepartmentId = user.DepartmentId ?? 0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(UserEditViewModel model)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Roles = userService.GetUserCreateRoles();
            ViewBag.Departments = userService.GetActiveDepartments();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                UserId = model.UserId,
                FullName = model.FullName,
                RoleId = model.RoleId,
                DepartmentId = model.DepartmentId
            };

            bool isUpdated = userService.UpdateUser(user);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "User updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User could not be updated.";
            }

            return RedirectToAction("Users");
        }

        public ActionResult Tasks()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var tasks = taskService.GetAllActiveTaskAssignments();

            return View(tasks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTask(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            int adminUserId = Convert.ToInt32(Session["UserId"]);

            bool deleted = taskService.SoftDeleteTask(id, adminUserId);

            if (deleted)
            {
                TempData["SuccessMessage"] = "Task deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Task could not be deleted.";
            }

            return RedirectToAction("Tasks");
        }


        public ActionResult DeletedTasks()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var deletedTasks = taskService.GetDeletedTaskAssignments();

            return View(deletedTasks);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PermanentlyDeleteTask(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            bool deleted = taskService.PermanentlyDeleteTask(id);

            if (deleted)
            {
                TempData["SuccessMessage"] = "Task permanently deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Task could not be permanently deleted.";
            }

            return RedirectToAction("DeletedTasks");
        }

    }
}