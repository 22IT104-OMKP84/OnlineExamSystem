using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int Students { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
    }

    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<int> Questions { get; set; } = new List<int>();
    }

    public class ExamResult
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ExamName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalMarks { get; set; }
        public string Date { get; set; } = string.Empty;
    }

    public class LoginHistory
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string LoginTime { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ManualMarkingItem
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ExamTitle { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int QuestionsPending { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
