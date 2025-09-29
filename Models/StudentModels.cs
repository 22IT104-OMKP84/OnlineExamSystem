namespace OnlineExamSystem.Models
{
    public class StudentExam
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int TotalMarks { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? Score { get; set; }
    }

    public class StudyMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class StudentLoginHistory
    {
        public int Id { get; set; }
        public string LoginTime { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class StudentProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EnrollmentDate { get; set; } = string.Empty;
    }
}
