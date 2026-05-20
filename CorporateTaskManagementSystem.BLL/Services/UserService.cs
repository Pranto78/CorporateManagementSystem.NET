using System;
using System.Collections.Generic;
using System.Linq;
using CorporateTaskManagementSystem.Common.Helpers;
using CorporateTaskManagementSystem.DAL.Context;
using CorporateTaskManagementSystem.Entities.Models;

namespace CorporateTaskManagementSystem.BLL.Services
{
    public class UserService
    {
        public List<User> GetAllUsers(string searchText = null, int? departmentId = null)
        {
            var db = new ApplicationDbContext();
            
                var query = db.Users
                    .Include("Role")
                    .Include("Department")
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(u =>
                        u.FullName.Contains(searchText) ||
                        u.Email.Contains(searchText));
                }

                if (departmentId.HasValue && departmentId.Value > 0)
                {
                    query = query.Where(u => u.DepartmentId == departmentId.Value);
                }

                return query
                    .OrderByDescending(u => u.CreatedAt)
                    .ToList();
            
        }

        public bool EmailExists(string email)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users.Any(u => u.Email == email);
            }
        }

        public void CreateUser(User user, string password)
        {
            using (var db = new ApplicationDbContext())
            {
                user.PasswordHash = PasswordHelper.HashPassword(password);
                user.IsActive = true;
                user.CreatedAt = DateTime.Now;

                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public List<Role> GetUserCreateRoles()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Roles
                    .Where(r => r.RoleName == "Leader" || r.RoleName == "Employee")
                    .ToList();
            }
        }

        public List<Department> GetActiveDepartments()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Departments
                    .Where(d => d.IsActive)
                    .ToList();
            }
        }

        public bool DeactivateUser(int userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return false;
                }

              
                if (user.RoleId == 1)
                {
                    return false;
                }

                user.IsActive = false;
                db.SaveChanges();

                return true;
            }
        }

        public bool ActivateUser(int userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return false;
                }

                user.IsActive = true;
                db.SaveChanges();

                return true;
            }
        }

        public User GetUserById(int userId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users
                    .Include("Role")
                    .Include("Department")
                    .FirstOrDefault(u => u.UserId == userId);
            }
        }

        public bool UpdateUser(User updatedUser)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == updatedUser.UserId);

                if (user == null)
                {
                    return false;
                }

                // Do not allow editing Admin role from here
                if (user.RoleId == 1)
                {
                    return false;
                }

                user.FullName = updatedUser.FullName;
                user.RoleId = updatedUser.RoleId;
                user.DepartmentId = updatedUser.DepartmentId;

                db.SaveChanges();

                return true;
            }
        }


        public int GetTotalUsersCount()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users.Count();
            }
        }

        public int GetActiveUsersCount()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users.Count(x => x.IsActive);
            }
        }

       
    }
}