using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CorporateTaskManagementSystem.Common.Enums;
using CorporateTaskManagementSystem.DAL.Context;
using CorporateTaskManagementSystem.Entities.Models;


namespace CorporateTaskManagementSystem.BLL.Services
{
    public class TaskService
    {
        public List<User> GetEmployees()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users
                    .Where(u => u.Role.RoleName == "Employee" && u.IsActive)
                    .OrderBy(u => u.FullName)
                    .ToList();
            }
        }


        public void CreateTask(TaskItem task, int assignedToUserId, int assignedByUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                task.CreatedAt = DateTime.Now;
                task.Status = CorporateTaskManagementSystem.Common.Enums.TaskStatus.Pending;

                db.TaskItems.Add(task);
                db.SaveChanges();

                var assignment = new TaskAssignment
                {
                    TaskItemId = task.TaskItemId,
                    AssignedToUserId = assignedToUserId,
                    AssignedByUserId = assignedByUserId,
                    AssignedAt = DateTime.Now,
                    IsActive = true
                };

                db.TaskAssignments.Add(assignment);
                db.SaveChanges();
            }
        }


        public List<TaskAssignment> GetTasksCreatedByLeader(int leaderUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Include(t => t.TaskItem)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.AssignedByUser)
                    .Where(t =>
                        t.AssignedByUserId == leaderUserId &&
                        t.IsActive &&
                        t.TaskItem.IsDeleted == false)
                    .OrderByDescending(t => t.AssignedAt)
                    .ToList();
            }
        }

        public List<TaskAssignment> GetTasksAssignedToEmployee(int employeeUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Include(t => t.TaskItem)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.AssignedByUser)
                    .Where(t =>
                        t.AssignedToUserId == employeeUserId &&
                        t.IsActive &&
                        t.TaskItem.IsDeleted == false)
                    .OrderByDescending(t => t.AssignedAt)
                    .ToList();
            }
        }


        public TaskAssignment GetTaskAssignmentByIdForEmployee(int assignmentId, int employeeUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Include(t => t.TaskItem)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.AssignedByUser)
                    .FirstOrDefault(t =>
                        t.TaskAssignmentId == assignmentId &&
                        t.AssignedToUserId == employeeUserId &&
                        t.IsActive);
            }
        }

        public void UpdateTaskStatus(
    int taskItemId,
    TaskStatus newStatus,
    int changedByUserId,
    string remarks,
    string submittedFilePath = null,
    string submittedFileName = null)
        {
            using (var db = new ApplicationDbContext())
            {
                var task = db.TaskItems.FirstOrDefault(t => t.TaskItemId == taskItemId);

                if (task == null)
                {
                    return;
                }

                var oldStatus = task.Status;

                task.Status = newStatus;

                var history = new TaskStatusHistory
                {
                    TaskItemId = taskItemId,
                    OldStatus = oldStatus,
                    NewStatus = newStatus,
                    ChangedByUserId = changedByUserId,
                    ChangedAt = DateTime.Now,
                    Remarks = remarks,
                    SubmittedFilePath = submittedFilePath,
                    SubmittedFileName = submittedFileName
                };

                db.TaskStatusHistories.Add(history);
                db.SaveChanges();
            }
        }

        public TaskStatusHistory GetLatestStatusHistoryByTaskId(int taskItemId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskStatusHistories
                    .Include(h => h.ChangedByUser)
                    .Where(h => h.TaskItemId == taskItemId)
                    .OrderByDescending(h => h.ChangedAt)
                    .FirstOrDefault();
            }
        }


        public TaskAssignment GetTaskAssignmentByIdForLeader(int assignmentId, int leaderUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Include(t => t.TaskItem)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.AssignedByUser)
                    .FirstOrDefault(t =>
                        t.TaskAssignmentId == assignmentId &&
                        t.AssignedByUserId == leaderUserId &&
                        t.IsActive);
            }
        }


        public List<TaskStatusHistory> GetTaskStatusHistoryByTaskId(int taskItemId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskStatusHistories
                    .Include(h => h.ChangedByUser)
                    .Where(h => h.TaskItemId == taskItemId)
                    .OrderByDescending(h => h.ChangedAt)
                    .ToList();
            }
        }


        public bool SoftDeleteTask(int taskItemId, int deletedByUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                var task = db.TaskItems.FirstOrDefault(t => t.TaskItemId == taskItemId);

                if (task == null)
                {
                    return false;
                }

                if (task.IsDeleted)
                {
                    return false;
                }

                task.IsDeleted = true;
                task.DeletedAt = DateTime.Now;
                task.DeletedByUserId = deletedByUserId;

                var assignments = db.TaskAssignments
                    .Where(a => a.TaskItemId == taskItemId && a.IsActive)
                    .ToList();

                foreach (var assignment in assignments)
                {
                    assignment.IsActive = false;
                }

                db.SaveChanges();

                return true;
            }
        }



        public List<TaskAssignment> GetAllActiveTaskAssignments()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Include(t => t.TaskItem)
                    .Include(t => t.TaskItem.CreatedByUser)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.AssignedByUser)
                    .Where(t =>
                        t.IsActive &&
                        t.TaskItem.IsDeleted == false)
                    .OrderByDescending(t => t.AssignedAt)
                    .ToList();
            }
        }

        public int GetTotalAssignedTasks()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Count(x => x.IsActive && !x.TaskItem.IsDeleted);
            }
        }

        public int GetTaskCountByStatus(TaskStatus status)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Count(x => x.IsActive
                             && !x.TaskItem.IsDeleted
                             && x.TaskItem.Status == status);
            }
        }


        public List<TaskAssignment> GetDeletedTaskAssignments()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.TaskAssignments
                    .Include(t => t.TaskItem)
                    .Include(t => t.TaskItem.CreatedByUser)
                    .Include(t => t.TaskItem.DeletedByUser)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.AssignedByUser)
                    .Where(t => t.TaskItem.IsDeleted == true)
                    .OrderByDescending(t => t.TaskItem.DeletedAt)
                    .ToList();
            }
        }


        public bool PermanentlyDeleteTask(int taskAssignmentId)
        {
            using (var db = new ApplicationDbContext())
            {
                var assignment = db.TaskAssignments
                    .Include("TaskItem")
                    .FirstOrDefault(x => x.TaskAssignmentId == taskAssignmentId);

                if (assignment == null)
                {
                    return false;
                }

                var taskId = assignment.TaskItemId;

                var histories = db.TaskStatusHistories
                    .Where(x => x.TaskItemId == taskId)
                    .ToList();

                db.TaskStatusHistories.RemoveRange(histories);

                db.TaskAssignments.Remove(assignment);

                var task = db.TaskItems.FirstOrDefault(x => x.TaskItemId == taskId);

                if (task != null)
                {
                    db.TaskItems.Remove(task);
                }

                db.SaveChanges();

                return true;
            }
        }



        public Dictionary<int, TaskStatusHistory> GetLatestSubmittedFilesForLeader(int leaderId)
        {
            using (var db = new ApplicationDbContext())
            {
                var leaderTaskIds = db.TaskAssignments
                    .Where(a => a.AssignedByUserId == leaderId && a.IsActive == true)
                    .Select(a => a.TaskItemId)
                    .ToList();

                var submittedFiles = db.TaskStatusHistories
                    .Where(h => leaderTaskIds.Contains(h.TaskItemId)
                                && h.SubmittedFilePath != null
                                && h.SubmittedFileName != null)
                    .GroupBy(h => h.TaskItemId)
                    .Select(g => g.OrderByDescending(x => x.ChangedAt).FirstOrDefault())
                    .ToList();

                return submittedFiles.ToDictionary(x => x.TaskItemId, x => x);
            }
        }

    }
}