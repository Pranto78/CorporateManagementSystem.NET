# Corporate Task Management System

A role-based **Corporate Task Management System** built with **ASP.NET MVC 5**, **.NET Framework 4.7.2**, **Entity Framework 6**, and **SQL Server**.

This project is designed for an organization where Admins manage users and monitor tasks, Leaders create and assign tasks, and Employees update progress and submit work. The project follows an N-tier architecture with separated Web, BLL, DAL, Entities, and Common layers.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Main Objectives](#main-objectives)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [User Roles](#user-roles)
- [Authentication](#authentication)
- [Authorization](#authorization)
- [Password Hashing](#password-hashing)
- [Entity Framework ORM](#entity-framework-orm)
- [Database Design](#database-design)
- [Enums](#enums)
- [ViewModels](#viewmodels)
- [Core Features](#core-features)
- [Beyond CRUD Functionalities](#beyond-crud-functionalities)
- [Task Workflow](#task-workflow)
- [File Upload System](#file-upload-system)
- [Task History and Logs](#task-history-and-logs)
- [Soft Delete and Permanent Delete](#soft-delete-and-permanent-delete)
- [Dashboard Analytics](#dashboard-analytics)
- [Search and Filter](#search-and-filter)
- [Security Features](#security-features)
- [HTTP Method Usage](#http-method-usage)
- [UI Design](#ui-design)
- [Important Service Classes](#important-service-classes)
- [Request Flow Examples](#request-flow-examples)
- [Repository Pattern Note](#repository-pattern-note)
- [Installation and Setup](#installation-and-setup)
- [Future Improvements](#future-improvements)
- [Viva Explanation Notes](#viva-explanation-notes)
- [Project Status](#project-status)
- [Author](#author)

---

## Project Overview

The **Corporate Task Management System** is an ASP.NET MVC 5 web application for managing corporate users, assigning tasks, tracking task progress, reviewing submitted work, and maintaining task status history.

The system has three main user roles:

```text
Admin
Leader
Employee
```

Each role has a separate dashboard and separate permissions.

---

## Main Objectives

The project was developed to demonstrate:

- ASP.NET MVC 5 web application development
- N-tier architecture
- Entity Framework ORM usage
- SQL Server relational database design
- Role-based authentication
- Role-based authorization
- Forms Authentication
- Password hashing using PBKDF2
- Service-based business logic
- Task workflow management
- File upload for task submission
- Task status history/logs
- Dashboard summary analytics
- Search and department-wise filtering
- Soft delete and permanent delete
- Clean Razor UI with Bootstrap and custom CSS

---

## Technology Stack

| Technology | Purpose |
|---|---|
| ASP.NET MVC 5 | Web application framework |
| .NET Framework 4.7.2 | Application runtime |
| Entity Framework 6 | ORM and database interaction |
| SQL Server | Relational database |
| Razor Views | Server-side UI rendering |
| Bootstrap | Responsive UI layout |
| C# | Backend programming language |
| LINQ | Data querying |
| Forms Authentication | Login/logout authentication |
| PBKDF2 | Secure password hashing |
| Visual Studio 2022 | Development IDE |

---

## Architecture

The project follows an **N-tier architecture**.

```text
Presentation Layer → Business Logic Layer → Data Access Layer → Database
```

In this project:

```text
Web → BLL → DAL → SQL Server Database
```

The controller does not directly access the database. Controllers call service classes, and service classes use Entity Framework `ApplicationDbContext` for database operations.

---

## Project Structure

The solution contains five projects:

```text
CorporateTaskManagementSystem
│
├── CorporateTaskManagementSystem.Web
│   ├── Controllers
│   ├── Views
│   ├── ViewModels
│   └── UI/request-response handling
│
├── CorporateTaskManagementSystem.BLL
│   └── Services
│       ├── AuthService.cs
│       ├── UserService.cs
│       └── TaskService.cs
│
├── CorporateTaskManagementSystem.DAL
│   ├── Context
│   │   └── ApplicationDbContext.cs
│   ├── Interfaces
│   ├── Repositories
│   └── Migrations
│
├── CorporateTaskManagementSystem.Entities
│   └── Models
│       ├── User.cs
│       ├── Role.cs
│       ├── Department.cs
│       ├── TaskItem.cs
│       ├── TaskAssignment.cs
│       ├── TaskComment.cs
│       ├── Notification.cs
│       └── TaskStatusHistory.cs
│
└── CorporateTaskManagementSystem.Common
    ├── Enums
    │   ├── UserRole.cs
    │   ├── TaskStatus.cs
    │   └── TaskPriority.cs
    └── Helpers
        └── PasswordHelper.cs
```

---

## Layer Responsibilities

### Web Layer

Project:

```text
CorporateTaskManagementSystem.Web
```

Responsibilities:

- Handles HTTP requests
- Contains MVC Controllers
- Contains Razor Views
- Contains ViewModels
- Handles request/response flow
- Displays UI pages
- Sends data to BLL services

Examples:

```text
AccountController.cs
AdminController.cs
LeaderController.cs
EmployeeController.cs
```

---

### BLL Layer

Project:

```text
CorporateTaskManagementSystem.BLL
```

Responsibilities:

- Contains business logic
- Handles application rules
- Calls DAL/Entity Framework
- Keeps controller code clean

Important services:

```text
AuthService.cs
UserService.cs
TaskService.cs
```

---

### DAL Layer

Project:

```text
CorporateTaskManagementSystem.DAL
```

Responsibilities:

- Contains `ApplicationDbContext`
- Handles Entity Framework configuration
- Contains migrations
- Handles database connection and database operations through EF

Important file:

```text
ApplicationDbContext.cs
```

---

### Entities Layer

Project:

```text
CorporateTaskManagementSystem.Entities
```

Responsibilities:

- Contains database entity classes
- Entity classes represent database tables
- Used by EF, BLL, and Web layers

Examples:

```text
User
Role
Department
TaskItem
TaskAssignment
TaskStatusHistory
```

---

### Common Layer

Project:

```text
CorporateTaskManagementSystem.Common
```

Responsibilities:

- Contains shared enums
- Contains helper classes
- Can be reused by other layers

Examples:

```text
UserRole
TaskStatus
TaskPriority
PasswordHelper
```

---

## Architecture Rule

The project follows this flow:

```text
Controller → Service → ApplicationDbContext → Database
```

Example:

```text
AdminController → UserService → ApplicationDbContext → Users Table
LeaderController → TaskService → ApplicationDbContext → Task Tables
EmployeeController → TaskService → ApplicationDbContext → Task Tables
```

The controller does not directly use `ApplicationDbContext`.

---

## User Roles

## 1. Admin

Admin is the highest-level user.

Admin can:

- Login
- Logout
- View Admin Dashboard
- Create Leader and Employee users
- Edit non-admin users
- Activate users
- Deactivate users
- Search users by name or email
- Filter users by department
- View all active tasks
- Soft delete any active task
- View deleted tasks
- Permanently delete tasks
- View dashboard summary analytics

Admin pages:

```text
Admin Dashboard
Manage Users
Create User
Edit User
All Active Tasks
Deleted Tasks
```

---

## 2. Leader

Leader is responsible for creating and managing tasks.

Leader can:

- Login
- Logout
- View Leader Dashboard
- Create tasks
- Assign tasks to employees
- Set task title
- Set task description
- Set task priority
- Set due date
- View tasks assigned by them
- Track employee progress
- View latest employee remarks
- Review submitted tasks
- Mark tasks as Completed
- Reject tasks with remarks
- View task history
- Delete own assigned tasks

Leader pages:

```text
Leader Dashboard
Create Task
Assigned Tasks
Review Task
Task History
```

---

## 3. Employee

Employee is responsible for working on assigned tasks.

Employee can:

- Login
- Logout
- View Employee Dashboard
- View assigned tasks
- Update task status
- Add remarks
- Submit work
- Upload submission files
- View task history

Employee pages:

```text
Employee Dashboard
My Tasks
Update Task Status
Submit Work
Task History
```

---

## Authentication

Authentication is done using **Forms Authentication**.

Important files:

```text
CorporateTaskManagementSystem.Web/Controllers/AccountController.cs
CorporateTaskManagementSystem.BLL/Services/AuthService.cs
CorporateTaskManagementSystem.Common/Helpers/PasswordHelper.cs
```

Important methods:

```csharp
public ActionResult Login()
public ActionResult Login(LoginViewModel model)
public ActionResult Logout()
public User Login(string email, string password)
public static bool VerifyPassword(string password, string storedHash)
```

Authentication flow:

```text
User enters email and password
        ↓
AccountController receives login request
        ↓
AuthService checks active user by email
        ↓
PasswordHelper verifies password
        ↓
FormsAuthentication.SetAuthCookie()
        ↓
Session values are stored
        ↓
User is redirected based on role
```

Session values stored after login:

```text
Session["UserId"]
Session["FullName"]
Session["Email"]
Session["RoleId"]
Session["RoleName"]
```

---

## Authorization

Authorization is done using:

```csharp
[Authorize]
```

and session-based role checking.

Used in:

```text
AdminController.cs
LeaderController.cs
EmployeeController.cs
```

Admin check example:

```csharp
if (Session["RoleName"] == null || Session["RoleName"].ToString() != "Admin")
{
    return RedirectToAction("Login", "Account");
}
```

Leader check:

```csharp
Session["RoleName"].ToString() != "Leader"
```

Employee check:

```csharp
Session["RoleName"].ToString() != "Employee"
```

Role-based redirect after login:

```text
Admin    → Admin Dashboard
Leader   → Leader Dashboard
Employee → Employee Dashboard
```

---

## Password Hashing

Passwords are not stored as plain text.

The project uses **PBKDF2 password hashing**, not MD5.

Important file:

```text
CorporateTaskManagementSystem.Common/Helpers/PasswordHelper.cs
```

Important methods:

```csharp
HashPassword(string password)
VerifyPassword(string password, string storedHash)
```

Password create flow:

```text
Plain password
    ↓
PasswordHelper.HashPassword()
    ↓
PBKDF2 hash
    ↓
Saved in PasswordHash column
```

Login flow:

```text
User enters password
    ↓
PasswordHelper.VerifyPassword()
    ↓
Input password is checked against saved hash
    ↓
Login success or failed
```

PBKDF2 is better than MD5 because:

- MD5 is old and insecure
- PBKDF2 uses salt
- PBKDF2 uses multiple iterations
- PBKDF2 makes brute-force attacks harder
- Plain password is never stored

---

## Entity Framework ORM

The project uses **Entity Framework 6** as ORM.

ORM means:

```text
Object Relational Mapping
```

Entity Framework maps C# entity classes to SQL Server database tables.

Examples:

```text
User.cs → Users table
Role.cs → Roles table
Department.cs → Departments table
TaskItem.cs → TaskItems table
TaskAssignment.cs → TaskAssignments table
TaskStatusHistory.cs → TaskStatusHistories table
```

Database operations are done using:

```csharp
ApplicationDbContext
DbSet
LINQ
SaveChanges()
```

Example:

```csharp
db.Users.Add(user);
db.SaveChanges();
```

---

## Database Design

Database name:

```text
CorporateTaskManagementSystemDb
```

Main tables:

```text
Roles
Departments
Users
TaskItems
TaskAssignments
TaskComments
Notifications
TaskStatusHistories
```

---

## Database Relationships

Main relationships:

```text
User → Role
User → Department
TaskItem → CreatedByUser
TaskItem → DeletedByUser
TaskAssignment → TaskItem
TaskAssignment → AssignedToUser
TaskAssignment → AssignedByUser
TaskStatusHistory → TaskItem
TaskStatusHistory → ChangedByUser
```

Relationship meaning:

- A user belongs to one role
- A user can belong to one department
- A leader can create tasks
- A task can be assigned to an employee
- A task assignment stores who assigned and who received the task
- Task status history stores all status changes
- Deleted task information stores who deleted the task

SQL Server multiple cascade path issue was handled using:

```csharp
WillCascadeOnDelete(false)
```

inside `ApplicationDbContext`.

---

## Important Entity Classes

### User

Important fields:

```text
UserId
FullName
Email
PasswordHash
RoleId
DepartmentId
IsActive
CreatedAt
Role
Department
```

---

### TaskItem

Important fields:

```text
TaskItemId
Title
Description
Priority
Status
CreatedAt
DueDate
CreatedByUserId
CreatedByUser
IsDeleted
DeletedAt
DeletedByUserId
DeletedByUser
```

---

### TaskAssignment

Important fields:

```text
TaskAssignmentId
TaskItemId
AssignedToUserId
AssignedByUserId
AssignedAt
IsActive
TaskItem
AssignedToUser
AssignedByUser
```

---

### TaskStatusHistory

Important fields:

```text
TaskStatusHistoryId
TaskItemId
OldStatus
NewStatus
ChangedByUserId
ChangedAt
Remarks
SubmittedFilePath
SubmittedFileName
TaskItem
ChangedByUser
```

---

## Enums

Enums are used for fixed named values.

Enum means a special data type used to define a fixed set of named values.

### UserRole

```csharp
public enum UserRole
{
    Admin = 1,
    Leader = 2,
    Employee = 3
}
```

### TaskStatus

```csharp
public enum TaskStatus
{
    Pending = 1,
    InProgress = 2,
    Submitted = 3,
    Completed = 4,
    Rejected = 5,
    Overdue = 6
}
```

### TaskPriority

```csharp
public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
```

Why enums are used:

- To avoid magic numbers
- To make code readable
- To reduce wrong values
- To maintain fixed options like role, status, and priority

Example:

```csharp
TaskStatus.Submitted
```

is clearer than:

```csharp
3
```

---

## ViewModels

ViewModels are used to handle page/form-specific data.

Entity model represents database table data.  
ViewModel represents only the data needed for a specific page/form.

Important ViewModels:

```text
LoginViewModel
UserCreateViewModel
UserEditViewModel
AdminDashboardViewModel
TaskCreateViewModel
TaskReviewViewModel
TaskStatusUpdateViewModel
```

Why ViewModels are used:

- To separate UI data from database entities
- To avoid exposing unnecessary database fields
- To protect sensitive fields like PasswordHash
- To make validation easier
- To keep controllers clean
- To make each page receive only the data it needs

Example:

The `User` entity has:

```text
UserId
FullName
Email
PasswordHash
RoleId
DepartmentId
IsActive
CreatedAt
Role
Department
```

But `UserCreateViewModel` only needs:

```text
FullName
Email
Password
RoleId
DepartmentId
```

So the password is taken from the form, hashed using `PasswordHelper`, and saved as `PasswordHash` in the database.

---

## Core Features

## 1. Login and Logout

- User can login using email and password
- Password is verified using PBKDF2 hash
- User is redirected based on role
- User can logout
- Session is cleared after logout

---

## 2. Admin User Management

Admin can:

- Create Leader and Employee
- Edit Leader and Employee
- Activate user
- Deactivate user
- Search users
- Filter users department-wise

---

## 3. Leader Task Management

Leader can:

- Create task
- Assign task to employee
- Set priority
- Set due date
- View assigned tasks
- Review submitted work
- Complete or reject task
- View task history
- Delete own assigned task

---

## 4. Employee Task Management

Employee can:

- View assigned tasks
- Update task status
- Add remarks
- Submit work
- Upload file
- View task history

---

## 5. Admin Task Monitoring

Admin can:

- View all active tasks
- Delete active tasks
- View deleted tasks
- Permanently delete tasks

---

## Beyond CRUD Functionalities

The requirement asked for functionalities beyond normal CRUD. This project includes multiple features beyond CRUD:

1. Role-based authentication and authorization
2. Task workflow system
3. File upload for task submission
4. Task status history/logs
5. Admin dashboard analytics
6. Search and department filter
7. Soft delete system
8. Deleted task archive/trash
9. Permanent delete
10. Role-wise access protection

---

## Task Workflow

The main task workflow:

```text
Leader creates task
        ↓
Task assigned to Employee
        ↓
Employee views task
        ↓
Employee updates status
        ↓
Employee submits work
        ↓
Leader reviews submitted work
        ↓
Leader marks task as Completed or Rejected
```

Status flow:

```text
Pending → InProgress → Submitted → Completed
```

or:

```text
Pending → InProgress → Submitted → Rejected → Resubmitted → Completed
```

---

## File Upload System

Employees can submit work with file upload.

Supported file types:

```text
PDF
DOC
DOCX
JPG
JPEG
PNG
ZIP
```

Maximum file size:

```text
5 MB
```

Upload folder:

```text
~/Uploads/TaskSubmissions/
```

File information is stored in `TaskStatusHistory`:

```text
SubmittedFilePath
SubmittedFileName
```

---

## Task History and Logs

Every task status change is stored in `TaskStatusHistory`.

Stored information:

```text
OldStatus
NewStatus
ChangedByUserId
ChangedAt
Remarks
SubmittedFilePath
SubmittedFileName
```

Benefits:

- Employee can view task progress history
- Leader can view employee updates
- Admin can monitor task activity
- Status changes are traceable

---

## Soft Delete and Permanent Delete

The project uses both soft delete and permanent delete.

### Soft Delete

When a task is deleted from active task pages, it is not removed from the database.

Instead:

```text
IsDeleted = true
DeletedAt = current date/time
DeletedByUserId = logged-in user
```

Soft deleted tasks disappear from active task pages.

---

### Deleted Tasks Archive

Admin can view soft deleted tasks in the Deleted Tasks page.

Admin can see:

```text
Task title
Created by
Assigned to
Assigned by
Priority
Last status
Deleted at
Deleted by
```

---

### Permanent Delete

Permanent delete removes task records from the database.

It deletes:

```text
TaskStatusHistories
TaskAssignment
TaskItem
```

Permanent delete is only available on the Admin Deleted Tasks page.

---

## Dashboard Analytics

Admin Dashboard shows:

```text
Total Users
Active Users
Assigned Tasks
Pending Tasks
In Progress Tasks
Submitted Tasks
Completed Tasks
Rejected Tasks
```

Important service methods:

```csharp
GetTotalUsersCount()
GetActiveUsersCount()
GetTotalAssignedTasks()
GetTaskCountByStatus()
```

---

## Search and Filter

Admin Manage Users page has:

- Search by full name
- Search by email
- Filter by department
- Combined search and department filter
- Reset button

Important method:

```csharp
GetAllUsers(string searchText = null, int? departmentId = null)
```

This method:

- Includes Role
- Includes Department
- Filters by name/email
- Filters by department
- Orders by CreatedAt descending

---

## Security Features

Security features included:

```text
PBKDF2 password hashing
Forms Authentication
Session-based role checking
[Authorize]
[ValidateAntiForgeryToken]
@Html.AntiForgeryToken()
POST methods for data-changing operations
Confirm popup before delete
File type validation
File size validation
No direct database access from controllers
```

---

## AntiForgeryToken

In Razor forms:

```csharp
@Html.AntiForgeryToken()
```

In controller POST actions:

```csharp
[ValidateAntiForgeryToken]
```

Purpose:

- Prevents CSRF attacks
- Ensures the POST request came from the original application form
- Rejects fake external form submissions

---

## HTTP Method Usage

The project uses proper HTTP methods.

### GET

Used to show pages/forms.

Examples:

```csharp
[HttpGet]
public ActionResult Login()

[HttpGet]
public ActionResult CreateUser()

[HttpGet]
public ActionResult EditUser(int id)

[HttpGet]
public ActionResult CreateTask()
```

---

### POST

Used to submit forms or change data.

Examples:

```csharp
[HttpPost]
public ActionResult Login(LoginViewModel model)

[HttpPost]
public ActionResult CreateUser(UserCreateViewModel model)

[HttpPost]
public ActionResult EditUser(UserEditViewModel model)

[HttpPost]
public ActionResult CreateTask(TaskCreateViewModel model)

[HttpPost]
public ActionResult UpdateTaskStatus(TaskStatusUpdateViewModel model)

[HttpPost]
public ActionResult ReviewTask(TaskReviewViewModel model)

[HttpPost]
public ActionResult DeleteTask(int id)

[HttpPost]
public ActionResult PermanentlyDeleteTask(int id)
```

---

## UI Design

The project uses a custom classic premium UI theme.

Design characteristics:

```text
Cream background
White cards
Brown/dark headers
Gold/brown borders
Serif headings
Soft shadows
Rounded cards
Styled tables
Styled buttons
Responsive layout
```

Role-based accent colors:

```text
Admin    → Brown/Gold classic style
Leader   → Green accent
Employee → Teal accent
```

Redesigned pages:

### Admin Pages

```text
Admin Dashboard
Manage Users
Create User
Edit User
All Active Tasks
Deleted Tasks
```

### Leader Pages

```text
Leader Dashboard
Create Task
Assigned Tasks
Review Task
Task History
```

### Employee Pages

```text
Employee Dashboard
My Tasks
Update Task Status
Submit Work
Task History
```

---

## Important Service Classes

## AuthService

Handles:

```text
Login validation
Active user checking
Password verification
```

Important method:

```csharp
Login(string email, string password)
```

---

## UserService

Handles:

```text
Get all users
Search users
Filter users
Create user
Hash password during user creation
Email duplicate checking
Get roles
Get departments
Edit user
Activate user
Deactivate user
User counts for dashboard
```

Important methods:

```csharp
GetAllUsers(string searchText = null, int? departmentId = null)
EmailExists(string email)
CreateUser(User user, string password)
GetUserCreateRoles()
GetActiveDepartments()
DeactivateUser(int userId)
ActivateUser(int userId)
GetUserById(int userId)
UpdateUser(User updatedUser)
GetTotalUsersCount()
GetActiveUsersCount()
```

---

## TaskService

Handles:

```text
Get employees
Create task
Assign task
Get leader tasks
Get employee tasks
Update task status
Submit work
Review task
Get latest task remarks
Get task history
Soft delete task
Get deleted tasks
Permanent delete
Dashboard task counts
```

Important methods:

```csharp
GetEmployees()
CreateTask()
GetTasksCreatedByLeader()
GetTasksAssignedToEmployee()
GetTaskAssignmentByIdForEmployee()
GetTaskAssignmentByIdForLeader()
UpdateTaskStatus()
GetLatestStatusHistoryByTaskId()
GetTaskStatusHistoryByTaskId()
SoftDeleteTask()
GetAllActiveTaskAssignments()
GetDeletedTaskAssignments()
PermanentlyDeleteTask()
GetTotalAssignedTasks()
GetTaskCountByStatus()
```

---

## Request Flow Examples

## Login Flow

```text
Login View
   ↓
AccountController.Login POST
   ↓
AuthService.Login
   ↓
PasswordHelper.VerifyPassword
   ↓
FormsAuthentication.SetAuthCookie
   ↓
Session values set
   ↓
Role-based redirect
```

---

## Create User Flow

```text
Admin Create User View
   ↓
AdminController.CreateUser POST
   ↓
ModelState validation
   ↓
UserService.EmailExists
   ↓
UserService.CreateUser
   ↓
PasswordHelper.HashPassword
   ↓
ApplicationDbContext.Users.Add
   ↓
Database
```

---

## Create Task Flow

```text
Leader Create Task View
   ↓
LeaderController.CreateTask POST
   ↓
TaskService.CreateTask
   ↓
TaskItem created
   ↓
TaskAssignment created
   ↓
Database
```

---

## Employee Task Update Flow

```text
Employee My Tasks
   ↓
Update Task Status
   ↓
EmployeeController.UpdateTaskStatus POST
   ↓
TaskService.UpdateTaskStatus
   ↓
TaskItem status updated
   ↓
TaskStatusHistory inserted
   ↓
Database
```

---

## Employee Submit Work Flow

```text
Employee Submit Work View
   ↓
EmployeeController.SubmitWork POST
   ↓
File validation
   ↓
File saved in Uploads folder
   ↓
Task status becomes Submitted
   ↓
TaskStatusHistory stores remarks and file info
   ↓
Leader can review
```

---

## Leader Review Flow

```text
Leader Assigned Tasks
   ↓
Review Submitted Task
   ↓
LeaderController.ReviewTask POST
   ↓
Leader selects Completed or Rejected
   ↓
TaskService updates status
   ↓
TaskStatusHistory saved
   ↓
Employee can see result
```

---

## Delete Flow

```text
Admin/Leader clicks Delete
   ↓
Controller DeleteTask POST
   ↓
TaskService.SoftDeleteTask
   ↓
IsDeleted = true
   ↓
Task hidden from active pages
   ↓
Admin can see it in Deleted Tasks
```

---

## Permanent Delete Flow

```text
Admin Deleted Tasks page
   ↓
Delete Permanently button
   ↓
AdminController.PermanentlyDeleteTask POST
   ↓
TaskService.PermanentlyDeleteTask
   ↓
Task history removed
   ↓
Task assignment removed
   ↓
Task item removed
   ↓
Database updated
```

---

## Repository Pattern Note

This project uses a **Service + EF DbContext approach**.

Current flow:

```text
Controller → Service → ApplicationDbContext → Database
```

A full Repository Pattern flow would be:

```text
Controller → Service → Repository → ApplicationDbContext → Database
```

Separate repository classes were not implemented because the project scope is small. However, controllers still do not directly access the database. The BLL service classes handle business logic and use EF `ApplicationDbContext` for data operations.

Viva-safe explanation:

```text
I used Service + EF DbContext approach instead of full Repository Pattern.
N-tier separation is still maintained because controllers do not directly use ApplicationDbContext.
For a larger enterprise project, repository interfaces and repository classes can be added later.
```

---

## Validation and Error Handling

The project includes:

```text
ModelState.IsValid
ValidationMessageFor
ValidationSummary
TempData success messages
TempData error messages
Anti-forgery token validation
Role checking before page access
File type validation
File size validation
Confirm popup before delete
Boolean service return values for failed operations
```

---

## Installation and Setup

## Prerequisites

Install:

```text
Visual Studio 2022
SQL Server / SQL Server Express
SQL Server Management Studio
.NET Framework 4.7.2
Entity Framework 6
```

---

## Step 1: Clone the Repository

```bash
git clone https://github.com/your-username/CorporateTaskManagementSystem.git
```

Replace the URL with your actual repository URL.

---

## Step 2: Open the Solution

Open the `.sln` file in Visual Studio 2022.

---

## Step 3: Restore NuGet Packages

In Visual Studio:

```text
Right click solution → Restore NuGet Packages
```

Or use Package Manager Console if needed.

---

## Step 4: Configure Database Connection

Update the connection string in `Web.config`.

Example:

```xml
<connectionStrings>
  <add name="CorporateTaskManagementSystemDb"
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=CorporateTaskManagementSystemDb;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Replace `YOUR_SERVER_NAME` with your SQL Server name.

Example:

```text
DESKTOP-XXXX\SQLEXPRESS
```

---

## Step 5: Run Migrations

Open Package Manager Console:

```powershell
Update-Database
```

If migrations are inside the DAL project, select the DAL project as the Default Project in Package Manager Console.

---

## Step 6: Run the Application

Set this project as startup project:

```text
CorporateTaskManagementSystem.Web
```

Then run:

```text
Ctrl + F5
```

or click the Start button in Visual Studio.

---

## Functional Requirement Mapping

| Requirement | Project Implementation | Status |
|---|---|---|
| MVC App | ASP.NET MVC 5 with Controllers and Razor Views | Completed |
| Proper HTTP Methods | GET for showing pages, POST for form submission/data change | Completed |
| N-tier Architecture | Web, BLL, DAL, Entities, Common | Completed |
| CRUD | User and Task management | Completed |
| 3–5 Features Beyond CRUD | Workflow, file upload, history, dashboard, search/filter, soft delete | Completed |
| Entity Framework ORM | EF6 with ApplicationDbContext | Completed |
| SQL Database | SQL Server | Completed |
| Validation | ViewModels, ModelState, validation messages | Completed |
| Error Handling | TempData messages, checks, confirm dialogs | Completed |
| Viva Readiness | Request flow, architecture, features explained | Completed |

---

## Future Improvements

Possible future features:

```text
Restore deleted task
Department management
Change password
Admin reset user password
Role-based dynamic navbar
Advanced task search/filter
Overdue task marking
Task comments
Notification system
Attendance system
Present/absent system
Pie chart/bar chart dashboard
Chart.js analytics
Repository Pattern with interfaces
Dependency Injection
```

---

## Viva Explanation Notes

## What is Entity Framework?

Entity Framework is an ORM that maps C# classes to database tables and allows database operations using C# code instead of writing raw SQL everywhere.

---

## What is ORM?

ORM means Object Relational Mapping. It connects object-oriented C# classes with relational database tables.

---

## What is an Entity Class?

An entity class represents a database table.

Example:

```text
User entity → Users table
TaskItem entity → TaskItems table
```

---

## What is a ViewModel?

A ViewModel contains only the data needed for a specific page or form.

It separates UI data from database entity data.

---

## Why use ViewModels?

ViewModels improve:

```text
Security
Validation
Readability
Maintainability
UI-specific data handling
```

Example:

`UserCreateViewModel` takes plain password from the form, but `PasswordHash` is not exposed to the UI.

---

## What is Enum?

Enum is a special data type used to define a fixed set of named values.

Example:

```csharp
TaskStatus.Pending
TaskStatus.Completed
TaskPriority.High
UserRole.Admin
```

---

## What is AntiForgeryToken?

AntiForgeryToken protects forms from CSRF attacks.

In the view:

```csharp
@Html.AntiForgeryToken()
```

In the controller:

```csharp
[ValidateAntiForgeryToken]
```

---

## What is HttpPost?

`[HttpPost]` means the action handles submitted form data or data-changing requests.

Examples:

```text
Create
Update
Delete
Login submit
Task submit
Review submit
```

---

## Why no DTO?

This is an MVC Razor-based application, not an API-based project.

ViewModels were used for UI/form data separation. DTOs can be added later if the project becomes API-based or larger.

---

## Why no Repository Pattern?

The project uses Service + EF DbContext approach.

Controllers do not directly access the database. Services use `ApplicationDbContext`.

For a larger system, repository interfaces and repository classes can be added.

---

## Project Status

```text
Authentication: Completed
Authorization: Completed
Admin module: Completed
Leader module: Completed
Employee module: Completed
User management: Completed
Task management: Completed
Task workflow: Completed
File upload: Completed
Task history: Completed
Soft delete: Completed
Deleted tasks archive: Completed
Permanent delete: Completed
Search/filter: Completed
Dashboard analytics: Completed
UI redesign: Completed
```

---

## Author

**Fahim Shahriyar**

GitHub: `@fshahriyar44`

---

## License

This project is developed for academic and learning purposes.
