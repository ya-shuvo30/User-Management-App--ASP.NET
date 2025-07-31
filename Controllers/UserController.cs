using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Data; // Use your actual DbContext namespace
using UserManagementApp.Models;
using System.Linq;

namespace UserManagementApp.Controllers
{
    //[Authorize] // Temporarily commented out for testing
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /User/
        public IActionResult Index(string search, string sortField, string sortOrder, bool? isBlocked, bool? isDeleted)
        {
            // Add test users if database is empty (for testing purposes)
            if (!_context.Users.Any())
            {
                var testUsers = new List<User>
                {
                    new User 
                    { 
                        Email = "user1@test.com", 
                        PasswordHash = "hashedpassword1", 
                        RegisteredAt = DateTime.UtcNow.AddDays(-10),
                        LastLogin = DateTime.UtcNow.AddDays(-1),
                        IsBlocked = false, 
                        IsDeleted = false 
                    },
                    new User 
                    { 
                        Email = "user2@test.com", 
                        PasswordHash = "hashedpassword2", 
                        RegisteredAt = DateTime.UtcNow.AddDays(-5),
                        LastLogin = DateTime.UtcNow.AddHours(-2),
                        IsBlocked = false, 
                        IsDeleted = false 
                    },
                    new User 
                    { 
                        Email = "user3@test.com", 
                        PasswordHash = "hashedpassword3", 
                        RegisteredAt = DateTime.UtcNow.AddDays(-3),
                        LastLogin = DateTime.UtcNow.AddMinutes(-30),
                        IsBlocked = true, 
                        IsDeleted = false 
                    }
                };
                
                _context.Users.AddRange(testUsers);
                _context.SaveChanges();
            }

            var users = _context.Users.AsQueryable();

            // Searching
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.Email.Contains(search));
            }

            // Filtering
            if (isBlocked.HasValue)
                users = users.Where(u => u.IsBlocked == isBlocked);

            if (isDeleted.HasValue)
                users = users.Where(u => u.IsDeleted == isDeleted);

            // Sorting (default Email ASC)
            switch (sortField)
            {
                case "Registered":
                    users = sortOrder == "desc" ? users.OrderByDescending(u => u.RegisteredAt) : users.OrderBy(u => u.RegisteredAt);
                    break;
                case "LastLogin":
                    users = sortOrder == "desc" ? users.OrderByDescending(u => u.LastLogin) : users.OrderBy(u => u.LastLogin);
                    break;
                case "Email":
                default:
                    users = sortOrder == "desc" ? users.OrderByDescending(u => u.Email) : users.OrderBy(u => u.Email);
                    break;
            }

            return View(users.ToList());
        }

        // POST: /User/BulkAction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkAction(string action, int[] selectedUsers)
        {
            try
            {
                if (selectedUsers == null || selectedUsers.Length == 0)
                {
                    TempData["Alert"] = "No users selected.";
                    TempData["AlertType"] = "warning";
                    return RedirectToAction("Index");
                }

                var users = _context.Users.Where(u => selectedUsers.Contains(u.Id)).ToList();

                if (users.Count == 0)
                {
                    TempData["Alert"] = "Selected users not found.";
                    TempData["AlertType"] = "warning";
                    return RedirectToAction("Index");
                }

                switch (action?.ToLower())
                {
                    case "block":
                        foreach (var user in users)
                        {
                            user.IsBlocked = true;
                        }
                        TempData["Alert"] = $"{users.Count} user(s) blocked successfully.";
                        TempData["AlertType"] = "success";
                        break;

                    case "unblock":
                        foreach (var user in users)
                        {
                            user.IsBlocked = false;
                        }
                        TempData["Alert"] = $"{users.Count} user(s) unblocked successfully.";
                        TempData["AlertType"] = "success";
                        break;

                    case "delete":
                        foreach (var user in users)
                        {
                            user.IsDeleted = true;
                        }
                        TempData["Alert"] = $"{users.Count} user(s) deleted successfully.";
                        TempData["AlertType"] = "success";
                        break;

                    default:
                        TempData["Alert"] = "Invalid action.";
                        TempData["AlertType"] = "danger";
                        return RedirectToAction("Index");
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Alert"] = $"Error performing bulk action: {ex.Message}";
                TempData["AlertType"] = "danger";
                return RedirectToAction("Index");
            }
        }

        // GET: /User/Test - Simple test endpoint
        public IActionResult Test()
        {
            var userCount = _context.Users.Count();
            var blockedCount = _context.Users.Count(u => u.IsBlocked);
            var deletedCount = _context.Users.Count(u => u.IsDeleted);
            
            ViewBag.UserCount = userCount;
            ViewBag.BlockedCount = blockedCount;
            ViewBag.DeletedCount = deletedCount;
            
            return View();
        }
    }
}
