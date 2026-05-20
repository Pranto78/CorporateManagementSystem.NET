using CorporateTaskManagementSystem.BLL.Services;
using CorporateTaskManagementSystem.Common.Enums;
using CorporateTaskManagementSystem.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorporateTaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly TaskService taskService = new TaskService();

        public ActionResult Index()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public ActionResult MyTasks()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            int employeeUserId = Convert.ToInt32(Session["UserId"]);

            var tasks = taskService.GetTasksAssignedToEmployee(employeeUserId);

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

            return View(tasks);
        }



        [HttpGet]
        public ActionResult UpdateTaskStatus(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            int employeeUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForEmployee(id, employeeUserId);

            if (assignment == null)
            {
                TempData["Error"] = "Task not found or you are not allowed to update this task.";
                return RedirectToAction("MyTasks");
            }

            if (assignment.TaskItem.Status == TaskStatus.Completed)
            {
                TempData["Error"] = "This task is already completed and cannot be updated.";
                return RedirectToAction("MyTasks");
            }

            var model = new TaskStatusUpdateViewModel
            {
                TaskAssignmentId = assignment.TaskAssignmentId,
                TaskItemId = assignment.TaskItemId,
                Title = assignment.TaskItem.Title,
                CurrentStatus = assignment.TaskItem.Status.ToString(),
                NewStatus = assignment.TaskItem.Status
            };

            var allowedStatuses = new List<TaskStatus>();

            if (assignment.TaskItem.Status == TaskStatus.Rejected)
            {
                allowedStatuses.Add(TaskStatus.InProgress);
                allowedStatuses.Add(TaskStatus.Submitted);
            }
            else
            {
                allowedStatuses.Add(TaskStatus.Pending);
                allowedStatuses.Add(TaskStatus.InProgress);
                allowedStatuses.Add(TaskStatus.Submitted);
            }

            ViewBag.StatusList = allowedStatuses
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = s == model.NewStatus
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTaskStatus(TaskStatusUpdateViewModel model)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            int employeeUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForEmployee(model.TaskAssignmentId, employeeUserId);

            if (assignment == null)
            {
                TempData["Error"] = "Task not found or you are not allowed to update this task.";
                return RedirectToAction("MyTasks");
            }

            if (assignment.TaskItem.Status == TaskStatus.Completed)
            {
                TempData["Error"] = "This task is already completed and cannot be updated.";
                return RedirectToAction("MyTasks");
            }


            if (assignment.TaskItem.Status == TaskStatus.Rejected)
            {
                if (model.NewStatus != TaskStatus.InProgress &&
                    model.NewStatus != TaskStatus.Submitted)
                {
                    TempData["Error"] = "Invalid status selected for a rejected task.";
                    return RedirectToAction("MyTasks");
                }
            }
            else
            {
                if (model.NewStatus != TaskStatus.Pending &&
                    model.NewStatus != TaskStatus.InProgress &&
                    model.NewStatus != TaskStatus.Submitted)
                {
                    TempData["Error"] = "Invalid status selected.";
                    return RedirectToAction("MyTasks");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Title = assignment.TaskItem.Title;
                model.CurrentStatus = assignment.TaskItem.Status.ToString();

                var allowedStatuses = new List<TaskStatus>();

                if (assignment.TaskItem.Status == TaskStatus.Rejected)
                {
                    allowedStatuses.Add(TaskStatus.InProgress);
                    allowedStatuses.Add(TaskStatus.Submitted);
                }
                else
                {
                    allowedStatuses.Add(TaskStatus.Pending);
                    allowedStatuses.Add(TaskStatus.InProgress);
                    allowedStatuses.Add(TaskStatus.Submitted);
                }

                ViewBag.StatusList = allowedStatuses
                    .Select(s => new SelectListItem
                    {
                        Value = ((int)s).ToString(),
                        Text = s.ToString(),
                        Selected = s == model.NewStatus
                    })
                    .ToList();

                return View(model);
            }

            taskService.UpdateTaskStatus(
                assignment.TaskItemId,
                model.NewStatus,
                employeeUserId,
                model.Remarks
            );

            TempData["Success"] = "Task status updated successfully.";
            return RedirectToAction("MyTasks");
        }


        [HttpGet]
        public ActionResult TaskHistory(int id)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            int employeeUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForEmployee(id, employeeUserId);

            if (assignment == null)
            {
                TempData["Error"] = "Task not found or you are not allowed to view this history.";
                return RedirectToAction("MyTasks");
            }

            var history = taskService.GetTaskStatusHistoryByTaskId(assignment.TaskItemId);

            ViewBag.TaskTitle = assignment.TaskItem.Title;

            return View(history);
        }


        public ActionResult SubmitWork()
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            int employeeUserId = Convert.ToInt32(Session["UserId"]);

            var tasks = taskService.GetTasksAssignedToEmployee(employeeUserId)
                .Where(t => t.TaskItem.Status != TaskStatus.Completed)
                .ToList();

            return View(tasks);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitWork(int taskAssignmentId, string remarks, HttpPostedFileBase submissionFile)
        {
            if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            int employeeUserId = Convert.ToInt32(Session["UserId"]);

            var assignment = taskService.GetTaskAssignmentByIdForEmployee(taskAssignmentId, employeeUserId);

            if (assignment == null)
            {
                return RedirectToAction("SubmitWork");
            }

            if (assignment.TaskItem.Status == TaskStatus.Completed)
            {
                TempData["Error"] = "Completed task cannot be submitted again.";
                return RedirectToAction("SubmitWork");
            }

            string filePath = null;
            string fileName = null;

            if (submissionFile != null && submissionFile.ContentLength > 0)
            {
                string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".zip" };

                string extension = Path.GetExtension(submissionFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    TempData["Error"] = "Only PDF, DOC, DOCX, JPG, PNG, ZIP files are allowed.";
                    return RedirectToAction("SubmitWork");
                }

                if (submissionFile.ContentLength > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "File size must be less than 5 MB.";
                    return RedirectToAction("SubmitWork");
                }

                string folderPath = Server.MapPath("~/Uploads/TaskSubmissions/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                fileName = Guid.NewGuid().ToString() + extension;
                string savePath = Path.Combine(folderPath, fileName);

                submissionFile.SaveAs(savePath);

                filePath = "/Uploads/TaskSubmissions/" + fileName;
            }

            taskService.UpdateTaskStatus(
                assignment.TaskItemId,
                TaskStatus.Submitted,
                employeeUserId,
                remarks,
                filePath,
                submissionFile != null ? submissionFile.FileName : null
            );

            TempData["Success"] = "Work submitted successfully.";

            return RedirectToAction("SubmitWork");
        }
    }
}