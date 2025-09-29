namespace OnlineExamSystem.Models
{
    public class InstructorClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int Students { get; set; }
        public string Schedule { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class InstructorExam
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int TotalMarks { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<int> Questions { get; set; } = new List<int>();
    }

    public class InstructorManualMarking
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ExamTitle { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int QuestionsPending { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class StudentProgress
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public double AverageScore { get; set; }
        public int ExamsTaken { get; set; }
        public string LastExamDate { get; set; } = string.Empty;
    }

    public class InstructorLoginHistory
    {
        public int Id { get; set; }
        public string LoginTime { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class InstructorProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;
        public string OfficeHours { get; set; } = string.Empty;
        public string JoinDate { get; set; } = string.Empty;
    }
}
