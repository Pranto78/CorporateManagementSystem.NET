
using System;
using System.Data.Entity.Migrations;
using CorporateTaskManagementSystem.Common.Helpers;
using CorporateTaskManagementSystem.Entities.Models;

namespace CorporateTaskManagementSystem.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CorporateTaskManagementSystem.DAL.Context.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CorporateTaskManagementSystem.DAL.Context.ApplicationDbContext";
        }

        protected override void Seed(CorporateTaskManagementSystem.DAL.Context.ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(
                r => r.RoleName,
                new CorporateTaskManagementSystem.Entities.Models.Role { RoleId = 1, RoleName = "Admin" },
                new CorporateTaskManagementSystem.Entities.Models.Role { RoleId = 2, RoleName = "Leader" },
                new CorporateTaskManagementSystem.Entities.Models.Role { RoleId = 3, RoleName = "Employee" }
            );

        context.Departments.AddOrUpdate(
        d => d.DepartmentName,
        new CorporateTaskManagementSystem.Entities.Models.Department
        {
            DepartmentId = 1,
            DepartmentName = "IT",
            Description = "Information Technology Department",
            IsActive = true
        },
        new CorporateTaskManagementSystem.Entities.Models.Department
        {
            DepartmentId = 2,
            DepartmentName = "HR",
            Description = "Human Resources Department",
            IsActive = true
        },
        new CorporateTaskManagementSystem.Entities.Models.Department
        {
            DepartmentId = 3,
            DepartmentName = "Finance",
            Description = "Finance Department",
            IsActive = true
        },
        new CorporateTaskManagementSystem.Entities.Models.Department
        {
            DepartmentId = 4,
            DepartmentName = "Marketing",
            Description = "Marketing Department",
            IsActive = true
        },
        new CorporateTaskManagementSystem.Entities.Models.Department
        {
            DepartmentId = 5,
            DepartmentName = "Operations",
            Description = "Operations Department",
            IsActive = true
        }
    );
   
     context.Users.AddOrUpdate(
    u => u.Email,

    new User
    {
        UserId = 1,
        FullName = "System Admin",
        Email = "admin@gmail.com",
        PasswordHash = PasswordHelper.HashPassword("Admin@123"),
        RoleId = 1,
        DepartmentId = 1,
        IsActive = true,
        CreatedAt = DateTime.Now
    },

    new User
    {
        UserId = 2,
        FullName = "Team Leader",
        Email = "leader@gmail.com",
        PasswordHash = PasswordHelper.HashPassword("Leader@123"),
        RoleId = 2,
        DepartmentId = 1,
        IsActive = true,
        CreatedAt = DateTime.Now
    },

    new User
    {
        UserId = 3,
        FullName = "Employee User",
        Email = "employee@gmail.com",
        PasswordHash = PasswordHelper.HashPassword("Employee@123"),
        RoleId = 3,
        DepartmentId = 1,
        IsActive = true,
        CreatedAt = DateTime.Now
    }
);
        }
    }
}