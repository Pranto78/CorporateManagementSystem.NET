using System.Linq;
using CorporateTaskManagementSystem.Common.Helpers;
using CorporateTaskManagementSystem.DAL.Context;
using CorporateTaskManagementSystem.Entities.Models;

namespace CorporateTaskManagementSystem.BLL.Services
{
    public class AuthService
    {
        public User Login(string email, string password)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users
                    .Include("Role")
                    .FirstOrDefault(u => u.Email == email && u.IsActive);

                if (user == null)
                {
                    return null;
                }

                bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    return null;
                }

                return user;
            }
        }
    }
}