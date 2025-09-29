using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq; // Added for Average()
using OnlineExamSystem.Models;

namespace OnlineExamSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        // Sample data - in a real app, this would come from a database
        private static List<StudentExam> upcomingExams = new List<StudentExam>
        {
            new StudentExam { Id = 1, Title = "Mathematics Midterm", Subject = "Mathematics", Date = "2024-12-20", Duration = 90, TotalMarks = 100, Status = "Scheduled" },
            new StudentExam { Id = 2, Title = "Physics Quiz", Subject = "Physics", Date = "2024-12-22", Duration = 45, TotalMarks = 50, Status = "Scheduled" }
        };

        private static List<StudentExam> currentExams = new List<StudentExam>
        {
            new StudentExam { Id = 3, Title = "Chemistry Final", Subject = "Chemistry", Date = "2024-12-18", Duration = 120, TotalMarks = 150, Status = "In Progress" }
        };

        private static List<StudentExam> pastExams = new List<StudentExam>
        {
            new StudentExam { Id = 4, Title = "English Literature", Subject = "English", Date = "2024-12-10", Duration = 90, TotalMarks = 100, Status = "Completed", Score = 85 },
            new StudentExam { Id = 5, Title = "History Test", Subject = "History", Date = "2024-12-05", Duration = 60, TotalMarks = 75, Status = "Completed", Score = 70 }
        };

        private static List<StudyMaterial> studyMaterials = new List<StudyMaterial>
        {
            new StudyMaterial { Id = 1, Title = "Mathematics Notes", Type = "PDF", Size = "2.5 MB", Description = "Comprehensive notes covering calculus, algebra, and trigonometry fundamentals." },
            new StudyMaterial { Id = 2, Title = "Physics Video Lectures", Type = "MP4", Size = "45 min", Description = "Video lectures on mechanics, thermodynamics, and wave physics." },
            new StudyMaterial { Id = 3, Title = "Chemistry Lab Manual", Type = "DOCX", Size = "1.8 MB", Description = "Laboratory procedures and safety guidelines for chemistry experiments." },
            new StudyMaterial { Id = 4, Title = "History Presentation", Type = "PPTX", Size = "3.2 MB", Description = "Presentation slides covering world history from ancient to modern times." }
        };

        // Additional data for student dashboard and other pages
        private static List<StudentLoginHistory> loginHistory = new List<StudentLoginHistory>
        {
            new StudentLoginHistory { Id = 1, LoginTime = "2024-12-20 09:30:00", IPAddress = "192.168.1.101", Device = "Chrome - Windows", Status = "Success" },
            new StudentLoginHistory { Id = 2, LoginTime = "2024-12-19 14:20:00", IPAddress = "192.168.1.101", Device = "Chrome - Windows", Status = "Success" },
            new StudentLoginHistory { Id = 3, LoginTime = "2024-12-18 11:15:00", IPAddress = "192.168.1.101", Device = "Chrome - Windows", Status = "Success" }
        };

        private static StudentProfile studentProfile = new StudentProfile
        {
            Name = "John Doe",
            Email = "john.doe@student.edu",
            StudentId = "STU2024001",
            Department = "Computer Science",
            Phone = "+1-555-0124",
            Address = "123 Student Street, University City, UC 12345",
            EnrollmentDate = "2024-09-01"
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            ViewBag.UpcomingExams = upcomingExams.Count;
            ViewBag.CurrentExams = currentExams.Count;
            ViewBag.CompletedExams = pastExams.Count;
            ViewBag.AverageScore = pastExams.Count > 0 ? pastExams.Average(e => e.Score ?? 0) : 0;
            ViewBag.UpcomingExamsList = upcomingExams.Take(3).ToList();
            ViewBag.RecentExams = pastExams.Take(3).ToList();
            ViewBag.StudyMaterials = studyMaterials.Take(3).ToList();
            return View();
        }

        public IActionResult UpcomingExams()
        {
            ViewData["Title"] = "Upcoming Exams";
            ViewBag.Exams = upcomingExams;
            return View();
        }

        public IActionResult CurrentExams()
        {
            ViewData["Title"] = "Current Exams";
            ViewBag.Exams = currentExams;
            return View();
        }

        public IActionResult PastExams()
        {
            ViewData["Title"] = "Past Exams";
            ViewBag.Exams = pastExams;
            return View();
        }

        public IActionResult ExamResults()
        {
            ViewData["Title"] = "Exam Results";
            ViewBag.Exams = pastExams;
            return View();
        }

        public IActionResult StudyMaterials()
        {
            ViewData["Title"] = "Study Materials";
            ViewBag.Materials = studyMaterials;
            return View();
        }

        public IActionResult Profile()
        {
            ViewData["Title"] = "Profile";
            ViewBag.StudentProfile = studentProfile;
            return View();
        }

        public IActionResult LoginHistory()
        {
            ViewData["Title"] = "Login History";
            ViewBag.LoginHistory = loginHistory;
            return View();
        }

        // Dynamic actions for exam management
        [HttpPost]
        public IActionResult StartExam(int examId)
        {
            var exam = upcomingExams.FirstOrDefault(e => e.Id == examId);
            if (exam != null)
            {
                exam.Status = "In Progress";
                upcomingExams.Remove(exam);
                currentExams.Add(exam);
            }
            return RedirectToAction("CurrentExams");
        }

        [HttpPost]
        public IActionResult SubmitExam(int examId, int score)
        {
            var exam = currentExams.FirstOrDefault(e => e.Id == examId);
            if (exam != null)
            {
                exam.Status = "Completed";
                exam.Score = score;
                currentExams.Remove(exam);
                pastExams.Add(exam);
            }
            return RedirectToAction("PastExams");
        }

        [HttpPost]
        public IActionResult DownloadMaterial(int materialId)
        {
            // In a real app, this would handle file download
            // For now, just redirect back
            return RedirectToAction("StudyMaterials");
        }

        [HttpPost]
        public IActionResult UpdateProfile(string name, string email, string phone, string address)
        {
            if (studentProfile != null)
            {
                studentProfile.Name = name;
                studentProfile.Email = email;
                studentProfile.Phone = phone;
                studentProfile.Address = address;
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult AddStudyMaterial(string title, string type, string size, string description)
        {
            var newMaterial = new StudyMaterial
            {
                Id = studyMaterials.Count > 0 ? studyMaterials.Max(m => m.Id) + 1 : 1,
                Title = title,
                Type = type,
                Size = size,
                Description = description
            };
            studyMaterials.Add(newMaterial);
            return RedirectToAction("StudyMaterials");
        }
    }
}


