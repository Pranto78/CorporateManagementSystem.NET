using CorporateTaskManagementSystem.BLL.Services;
using CorporateTaskManagementSystem.Common.Enums;
using CorporateTaskManagementSystem.Entities.Models;
using CorporateTaskManagementSystem.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CorporateTaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaderController : Controller
    {
        private readonly TaskService taskService = new TaskService();

        public ActionResult Index()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateTask()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Employees = taskService.GetEmployees();
            ViewBag.Priorities = Enum.GetValues(typeof(TaskPriority));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask(TaskCreateViewModel model)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Employees = taskService.GetEmployees();
            ViewBag.Priorities = Enum.GetValues(typeof(TaskPriority));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int leaderUserId = Convert.ToInt32(Session["UserId"]);

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                Priority = (TaskPriority)model.Priority,
                DueDate = model.DueDate,
                CreatedByUserId = leaderUserId
            };

            taskService.CreateTask(task, model.AssignedToUserId, leaderUserId);

            TempData["SuccessMessage"] = "Task created and assigned successfully.";

            return RedirectToAction("AssignedTasks");
        }

        public ActionResult AssignedTasks()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            int leaderUserId = Convert.ToInt32(Session["UserId"]);

            var tasks = taskService.GetTasksCreatedByLeader(leaderUserId);

            var remarksData = new Dictionary<int, string>();

            foreach (var item in tasks)
            {
                var latestHistory = taskService.GetLatestStatusHistoryByTaskId(item.TaskItemId);

                if (latestHistory != null && !string.IsNullOrEmpty(latestHistory.Remarks))
                {
                    remarksData[item.TaskItemId] = latestHistory.Remarks;
                }
                else
                {
                    remarksData[item.TaskItemId] = "No remarks";
                }
            }

            ViewBag.RemarksData = remarksData;

            // New: latest submitted file data for leader assigned tasks
            ViewBag.SubmittedFiles = taskService.GetLatestSubmittedFilesForLeader(leaderUserId);

            return View(tasks);
        }

        [HttpGet]
        public ActionResult ReviewTask(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            int leaderUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForLeader(id, leaderUserId);

            if (assignment == null)
            {
                TempData["Error"] = "Task not found or you are not allowed to review this task.";
                return RedirectToAction("AssignedTasks");
            }

            if (assignment.TaskItem.Status != TaskStatus.Submitted)
            {
                TempData["Error"] = "Only submitted tasks can be reviewed.";
                return RedirectToAction("AssignedTasks");
            }

            var latestHistory = taskService.GetLatestStatusHistoryByTaskId(assignment.TaskItemId);

            var model = new TaskReviewViewModel
            {
                TaskAssignmentId = assignment.TaskAssignmentId,
                TaskItemId = assignment.TaskItemId,
                Title = assignment.TaskItem.Title,
                AssignedTo = assignment.AssignedToUser.FullName,
                CurrentStatus = assignment.TaskItem.Status.ToString(),
                EmployeeRemarks = latestHistory != null && !string.IsNullOrEmpty(latestHistory.Remarks)
                    ? latestHistory.Remarks
                    : "No remarks",
                ReviewStatus = assignment.TaskItem.Status
            };

            ViewBag.ReviewStatusList = Enum.GetValues(typeof(TaskStatus))
                .Cast<TaskStatus>()
                .Where(s => s == TaskStatus.Completed ||
                            s == TaskStatus.Rejected)
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = s == model.ReviewStatus
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewTask(TaskReviewViewModel model)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            int leaderUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForLeader(model.TaskAssignmentId, leaderUserId);

            if (assignment == null)
            {
                TempData["Error"] = "Task not found or you are not allowed to review this task.";
                return RedirectToAction("AssignedTasks");
            }

            if (assignment.TaskItem.Status != TaskStatus.Submitted)
            {
                TempData["Error"] = "Only submitted tasks can be reviewed.";
                return RedirectToAction("AssignedTasks");
            }

            if (!ModelState.IsValid)
            {
                var latestHistory = taskService.GetLatestStatusHistoryByTaskId(assignment.TaskItemId);

                model.Title = assignment.TaskItem.Title;
                model.AssignedTo = assignment.AssignedToUser.FullName;
                model.CurrentStatus = assignment.TaskItem.Status.ToString();
                model.EmployeeRemarks = latestHistory != null && !string.IsNullOrEmpty(latestHistory.Remarks)
                    ? latestHistory.Remarks
                    : "No remarks";

                ViewBag.ReviewStatusList = Enum.GetValues(typeof(TaskStatus))
                    .Cast<TaskStatus>()
                    .Where(s => s == TaskStatus.Completed ||
                                s == TaskStatus.Rejected)
                    .Select(s => new SelectListItem
                    {
                        Value = ((int)s).ToString(),
                        Text = s.ToString(),
                        Selected = s == model.ReviewStatus
                    })
                    .ToList();

                return View(model);
            }

            taskService.UpdateTaskStatus(
                assignment.TaskItemId,
                model.ReviewStatus,
                leaderUserId,
                model.LeaderRemarks
            );

            TempData["SuccessMessage"] = "Task reviewed successfully.";
            return RedirectToAction("AssignedTasks");
        }


        [HttpGet]
        public ActionResult TaskHistory(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            int leaderUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForLeader(id, leaderUserId);

            if (assignment == null)
            {
                TempData["Error"] = "Task not found or you are not allowed to view this history.";
                return RedirectToAction("AssignedTasks");
            }

            var history = taskService.GetTaskStatusHistoryByTaskId(assignment.TaskItemId);

            ViewBag.TaskTitle = assignment.TaskItem.Title;
            ViewBag.AssignedTo = assignment.AssignedToUser.FullName;

            return View(history);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTask(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Leader")
            {
                return RedirectToAction("Login", "Account");
            }

            int leaderUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForLeader(id, leaderUserId);

            if (assignment == null)
            {
                TempData["Error"] = "Task not found or you are not allowed to delete this task.";
                return RedirectToAction("AssignedTasks");
            }

            bool deleted = taskService.SoftDeleteTask(assignment.TaskItemId, leaderUserId);

            if (deleted)
            {
                TempData["SuccessMessage"] = "Task deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Task could not be deleted.";
            }

            return RedirectToAction("AssignedTasks");
        }
    }
}