using OnlineExamSystem.Models;
using System.Security.Cryptography;
using System.Text;

namespace OnlineExamSystem.Services
{
    public interface IUserService
    {
        Task<UserInfo?> AuthenticateAsync(string email, string password);
        Task<UserInfo?> RegisterAsync(RegisterRequest request);
        Task<UserInfo?> GetUserByIdAsync(int id);
        Task<UserInfo?> GetUserByEmailAsync(string email);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> UpdateUserAsync(UserInfo user);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }

    public class UserService : IUserService
    {
        // In a real application, this would be replaced with a database context
        private static readonly List<UserAccount> _users = new List<UserAccount>();

        static UserService()
        {
            // Initialize with default users
            _users.AddRange(new[]
            {
                new UserAccount
                {
                    Id = 1,
                    Name = "Admin User",
                    Email = "admin@example.com",
                    PasswordHash = "hashed_admin123", // Will be properly hashed in constructor
                    Role = "Admin",
                    Department = "Administration",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new UserAccount
                {
                    Id = 2,
                    Name = "John Instructor",
                    Email = "instructor@example.com",
                    PasswordHash = "hashed_instructor123",
                    Role = "Instructor",
                    Department = "Computer Science",
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new UserAccount
                {
                    Id = 3,
                    Name = "Jane Student",
                    Email = "student@example.com",
                    PasswordHash = "hashed_student123",
                    Role = "Student",
                    Department = "Computer Science",
                    StudentId = "STU001",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                }
            });
        }

        public UserService()
        {
            // Properly hash the passwords for default users
            var adminUser = _users.FirstOrDefault(u => u.Id == 1);
            if (adminUser != null) adminUser.PasswordHash = HashPassword("admin123");

            var instructorUser = _users.FirstOrDefault(u => u.Id == 2);
            if (instructorUser != null) instructorUser.PasswordHash = HashPassword("instructor123");

            var studentUser = _users.FirstOrDefault(u => u.Id == 3);
            if (studentUser != null) studentUser.PasswordHash = HashPassword("student123");
        }

        public async Task<UserInfo?> AuthenticateAsync(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return new UserInfo
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Department = user.Department,
                StudentId = user.StudentId
            };
        }

        public async Task<UserInfo?> RegisterAsync(RegisterRequest request)
        {
            // Check if user already exists
            if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var newUser = new UserAccount
            {
                Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1,
                Name = request.Name,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                Department = request.Department,
                StudentId = request.StudentId,
                CreatedAt = DateTime.UtcNow
            };

            _users.Add(newUser);

            return new UserInfo
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                Role = newUser.Role,
                Department = newUser.Department,
                StudentId = newUser.StudentId
            };
        }

        public async Task<UserInfo?> GetUserByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            return new UserInfo
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Department = user.Department,
                StudentId = user.StudentId
            };
        }

        public async Task<UserInfo?> GetUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null) return null;

            return new UserInfo
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Department = user.Department,
                StudentId = user.StudentId
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null || !VerifyPassword(currentPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = HashPassword(newPassword);
            return true;
        }

        public async Task<bool> UpdateUserAsync(UserInfo userInfo)
        {
            var user = _users.FirstOrDefault(u => u.Id == userInfo.Id);
            if (user == null) return false;

            user.Name = userInfo.Name;
            user.Email = userInfo.Email;
            user.Department = userInfo.Department;
            user.StudentId = userInfo.StudentId;
            return true;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "OnlineExamSystemSalt"));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        // Internal class for storing user data
        private class UserAccount
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string? Department { get; set; }
            public string? StudentId { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
