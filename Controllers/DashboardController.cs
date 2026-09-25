using RODRIGUEZ_MESSAGEBOXE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RODRIGUEZ_MESSAGEBOXE.Controllers
{
    [AllowAnonymous]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // CHANGED: Use SQL database to get all users
            return View(UserStore.GetAll());
        }

        [HttpPost]
        public IActionResult Insert(string name, string email, string username, string password, string gender, int age, string address)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return Json(new { success = false, error = "Name, email, username and password are required." });

            if (UserStore.FindByUsername(username) != null)
                return Json(new { success = false, error = "Username already exists." });

            if (UserStore.FindByEmail(email) != null)
                return Json(new { success = false, error = "Email already exists." });

            // Build SQL statement from User model (for display purposes only)
            var sql = Models.User.BuildInsertStatement(
                new[] { "name", "email", "username", "password", "gender", "age", "address" },
                new object[] { name, email, username, password, gender, age, address },
                "Users"
            );

            var user = new User
            {
                Name = name,
                Email = email,
                Gender = gender ?? "",
                Age = age,
                Address = address ?? "",
                Username = username,
                Password = password,
                CreatedAtUtc = DateTime.UtcNow
            };

            // CHANGED: Insert into SQL database
            UserStore.Add(user);

            return Json(new { success = true, message = $"User \"{name}\" added.", sql, users = GetRows() });
        }

        [HttpPost]
        public IActionResult Update(int id, string name, string email, string username, string password, string gender, int age, string address)
        {
            var user = UserStore.FindById(id);
            if (user == null)
                return Json(new { success = false, error = $"No user found with ID {id}." });

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username))
                return Json(new { success = false, error = "Name, email and username are required." });

            // Check for duplicates (excluding current user)
            var allUsers = UserStore.GetAll();
            if (allUsers.Any(u => u.Id != id && u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return Json(new { success = false, error = "Username already taken." });

            if (allUsers.Any(u => u.Id != id && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return Json(new { success = false, error = "Email already taken." });

            // Build SQL statement from User model (for display purposes only)
            var sql = Models.User.BuildUpdateStatement(
                new[] { "name", "email", "username", "password", "gender", "age", "address" },
                new object[] { name, email, username, password ?? user.Password, gender, age, address },
                "Users",
                "id = " + id
            );

            user.Name = name;
            user.Email = email;
            user.Username = username;
            user.Password = string.IsNullOrWhiteSpace(password) ? user.Password : password;
            user.Gender = gender ?? "";
            user.Age = age;
            user.Address = address ?? "";

            // CHANGED: Update SQL database
            UserStore.Update(user);

            return Json(new { success = true, message = $"User ID {id} updated.", sql, users = GetRows() });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = UserStore.FindById(id);
            if (user == null)
                return Json(new { success = false, error = $"No user found with ID {id}." });

            // Build SQL statement from User model (for display purposes only)
            var sql = Models.User.BuildDeleteStatement("Users", "id = " + id);

            // CHANGED: Delete from SQL database
            UserStore.Remove(user);

            return Json(new { success = true, message = $"User ID {id} deleted.", sql, users = GetRows() });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Build SQL statement from User model (for display purposes only)
            var sql = Models.User.BuildViewStatement("Users");
            return Json(new { success = true, sql, users = GetRows() });
        }

        private static object GetRows() =>
            UserStore.GetAll().Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Username,
                u.Password,
                u.Gender,
                u.Age,
                u.Address,
                createdAt = u.CreatedAtUtc.ToString("MMM d, yyyy")
            }).ToList();
    }
}