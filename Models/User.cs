using System;

namespace UserManagementApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegisteredAt { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; } // Added for middleware support
    }
}