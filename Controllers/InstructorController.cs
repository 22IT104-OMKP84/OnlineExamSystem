using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq; // Added for FirstOrDefault
using OnlineExamSystem.Models;

namespace OnlineExamSystem.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        // Sample data - in a real app, this would come from a database
        private static List<InstructorClass> classes = new List<InstructorClass>
        {
            new InstructorClass { Id = 1, Name = "Mathematics 101", Subject = "Mathematics", Students = 25, Schedule = "Mon, Wed 10:00 AM", Status = "Active" },
            new InstructorClass { Id = 2, Name = "Physics 101", Subject = "Physics", Students = 30, Schedule = "Tue, Thu 2:00 PM", Status = "Active" },
            new InstructorClass { Id = 3, Name = "Chemistry 101", Subject = "Chemistry", Students = 28, Schedule = "Fri 9:00 AM", Status = "Pending" }
        };

        // Using the Question model from AdminController
        private static List<Question> questions = new List<Question>
        {
            new Question { Id = 1, QuestionText = "What is the derivative of x²?", Subject = "Mathematics", Type = "MCQ", Difficulty = "Medium", Status = "Active", CorrectAnswer = "2x", Options = new List<string> { "2x", "x²", "2x²", "x" } },
            new Question { Id = 2, QuestionText = "Calculate the area of a circle with radius 5", Subject = "Mathematics", Type = "Numeric", Difficulty = "Easy", Status = "Active", CorrectAnswer = "78.54", Options = new List<string>() },
            new Question { Id = 3, QuestionText = "Explain Newton's First Law of Motion", Subject = "Physics", Type = "Essay", Difficulty = "Hard", Status = "Active", CorrectAnswer = "An object at rest stays at rest unless acted upon by an external force.", Options = new List<string>() }
        };

        // Using the Exam model from AdminController but with additional properties for instructor view
        private static List<InstructorExam> exams = new List<InstructorExam>
        {
            new InstructorExam { Id = 1, Title = "Midterm Mathematics", Subject = "Mathematics", Class = "Mathematics 101", Duration = 90, TotalMarks = 100, Status = "Scheduled", Questions = new List<int> { 1, 2 } },
            new InstructorExam { Id = 2, Title = "Physics Final", Subject = "Physics", Class = "Physics 101", Duration = 120, TotalMarks = 150, Status = "Completed", Questions = new List<int> { 3 } },
            new InstructorExam { Id = 3, Title = "Chemistry Quiz", Subject = "Chemistry", Class = "Chemistry 101", Duration = 45, TotalMarks = 50, Status = "Draft", Questions = new List<int>() }
        };

        // Additional data for instructor dashboard and other pages
        private static List<InstructorManualMarking> manualMarkingItems = new List<InstructorManualMarking>
        {
            new InstructorManualMarking { Id = 1, StudentName = "John Doe", ExamTitle = "Midterm Mathematics", Subject = "Mathematics", QuestionsPending = 3, Priority = "High", Status = "Pending" },
            new InstructorManualMarking { Id = 2, StudentName = "Jane Smith", ExamTitle = "Physics Final", Subject = "Physics", QuestionsPending = 2, Priority = "Medium", Status = "Pending" },
            new InstructorManualMarking { Id = 3, StudentName = "Mike Johnson", ExamTitle = "Chemistry Quiz", Subject = "Chemistry", QuestionsPending = 1, Priority = "Low", Status = "In Progress" }
        };

        private static List<StudentProgress> studentProgress = new List<StudentProgress>
        {
            new StudentProgress { Id = 1, StudentName = "John Doe", Class = "Mathematics 101", Subject = "Mathematics", AverageScore = 85.5, ExamsTaken = 5, LastExamDate = "2024-12-15" },
            new StudentProgress { Id = 2, StudentName = "Jane Smith", Class = "Physics 101", Subject = "Physics", AverageScore = 92.3, ExamsTaken = 3, LastExamDate = "2024-12-10" },
            new StudentProgress { Id = 3, StudentName = "Mike Johnson", Class = "Chemistry 101", Subject = "Chemistry", AverageScore = 78.9, ExamsTaken = 4, LastExamDate = "2024-12-12" }
        };

        private static List<InstructorLoginHistory> loginHistory = new List<InstructorLoginHistory>
        {
            new InstructorLoginHistory { Id = 1, LoginTime = "2024-12-20 09:30:00", IPAddress = "192.168.1.100", Device = "Chrome - Windows", Status = "Success" },
            new InstructorLoginHistory { Id = 2, LoginTime = "2024-12-19 14:20:00", IPAddress = "192.168.1.100", Device = "Chrome - Windows", Status = "Success" },
            new InstructorLoginHistory { Id = 3, LoginTime = "2024-12-18 11:15:00", IPAddress = "192.168.1.100", Device = "Chrome - Windows", Status = "Success" }
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            ViewBag.TotalClasses = classes.Count;
            ViewBag.ActiveClasses = classes.Count(c => c.Status == "Active");
            ViewBag.TotalQuestions = questions.Count;
            ViewBag.TotalExams = exams.Count;
            ViewBag.ExamsInProgress = exams.Count(e => e.Status == "In Progress");
            ViewBag.ExamsCompleted = exams.Count(e => e.Status == "Completed");
            ViewBag.PendingMarking = manualMarkingItems.Count(m => m.Status == "Pending");
            ViewBag.Classes = classes;
            ViewBag.Exams = exams;
            ViewBag.ManualMarkingItems = manualMarkingItems;
            return View();
        }

        public IActionResult ManageClasses()
        {
            ViewData["Title"] = "Manage Classes";
            ViewBag.Classes = classes;
            ViewBag.Subjects = new List<string> { "Mathematics", "Physics", "Chemistry", "Biology", "Computer Science" };
            return View();
        }

        public IActionResult QuestionBank()
        {
            ViewData["Title"] = "Question Bank";
            ViewBag.Questions = questions;
            ViewBag.Subjects = new List<string> { "Mathematics", "Physics", "Chemistry", "Biology", "Computer Science" };
            ViewBag.QuestionTypes = new List<string> { "MCQ", "Numeric", "Essay", "True/False" };
            ViewBag.DifficultyLevels = new List<string> { "Easy", "Medium", "Hard" };
            return View();
        }

        public IActionResult CreateExam()
        {
            ViewData["Title"] = "Create Exam";
            ViewBag.Classes = classes;
            ViewBag.Questions = questions;
            ViewBag.Subjects = new List<string> { "Mathematics", "Physics", "Chemistry", "Biology", "Computer Science" };
            return View();
        }

        public IActionResult ManualMarking()
        {
            ViewData["Title"] = "Manual Marking";
            ViewBag.ManualMarkingItems = manualMarkingItems;
            return View();
        }

        public IActionResult ExamResults()
        {
            ViewData["Title"] = "Exam Results";
            ViewBag.Exams = exams;
            return View();
        }

        public IActionResult StudentProgress()
        {
            ViewData["Title"] = "Student Progress";
            ViewBag.StudentProgress = studentProgress;
            ViewBag.Classes = classes;
            return View();
        }

        public IActionResult Profile()
        {
            ViewData["Title"] = "Profile";
            ViewBag.InstructorProfile = new InstructorProfile 
            { 
                Name = "Dr. Sarah Johnson", 
                Email = "sarah.johnson@university.edu", 
                Department = "Mathematics", 
                Phone = "+1-555-0123",
                Office = "Room 205, Science Building",
                OfficeHours = "Mon, Wed 2:00-4:00 PM"
            };
            return View();
        }

        public IActionResult LoginHistory()
        {
            ViewData["Title"] = "Login History";
            ViewBag.LoginHistory = loginHistory;
            return View();
        }

        // Dynamic actions for class management
        [HttpPost]
        public IActionResult AddClass(string name, string subject, string schedule)
        {
            var newClass = new InstructorClass
            {
                Id = classes.Count > 0 ? classes.Max(c => c.Id) + 1 : 1,
                Name = name,
                Subject = subject,
                Students = 0,
                Schedule = schedule,
                Status = "Pending"
            };
            classes.Add(newClass);
            return RedirectToAction("ManageClasses");
        }

        [HttpPost]
        public IActionResult DeleteClass(int id)
        {
            var classItem = classes.FirstOrDefault(c => c.Id == id);
            if (classItem != null)
            {
                classes.Remove(classItem);
            }
            return RedirectToAction("ManageClasses");
        }

        [HttpPost]
        public IActionResult UpdateClassStatus(int id, string status)
        {
            var classItem = classes.FirstOrDefault(c => c.Id == id);
            if (classItem != null)
            {
                classItem.Status = status;
            }
            return RedirectToAction("ManageClasses");
        }

        [HttpPost]
        public IActionResult UpdateClass(int id, string name, string subject, string schedule)
        {
            var classItem = classes.FirstOrDefault(c => c.Id == id);
            if (classItem != null)
            {
                classItem.Name = name;
                classItem.Subject = subject;
                classItem.Schedule = schedule;
            }
            return RedirectToAction("ManageClasses");
        }

        // Dynamic actions for question management
        [HttpPost]
        public IActionResult AddQuestion(string questionText, string subject, string type, string difficulty, string correctAnswer, string[] options)
        {
            var newQuestion = new Question
            {
                Id = questions.Count > 0 ? questions.Max(q => q.Id) + 1 : 1,
                QuestionText = questionText,
                Subject = subject,
                Type = type,
                Difficulty = difficulty,
                Status = "Active",
                CorrectAnswer = correctAnswer,
                Options = options?.ToList() ?? new List<string>()
            };
            questions.Add(newQuestion);
            return RedirectToAction("QuestionBank");
        }

        [HttpPost]
        public IActionResult DeleteQuestion(int id)
        {
            var question = questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
            {
                questions.Remove(question);
            }
            return RedirectToAction("QuestionBank");
        }

        [HttpPost]
        public IActionResult UpdateQuestion(int id, string questionText, string subject, string type, string difficulty, string correctAnswer, string[] options)
        {
            var question = questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
            {
                question.QuestionText = questionText;
                question.Subject = subject;
                question.Type = type;
                question.Difficulty = difficulty;
                question.CorrectAnswer = correctAnswer;
                question.Options = options?.ToList() ?? new List<string>();
            }
            return RedirectToAction("QuestionBank");
        }

        // Dynamic actions for exam management
        [HttpPost]
        public IActionResult CreateExam(string title, string subject, string className, int duration, int totalMarks, string instructions, int[] questionIds)
        {
            var newExam = new InstructorExam
            {
                Id = exams.Count > 0 ? exams.Max(e => e.Id) + 1 : 1,
                Title = title,
                Subject = subject,
                Class = className,
                Duration = duration,
                TotalMarks = totalMarks,
                Status = "Draft",
                Questions = questionIds?.ToList() ?? new List<int>()
            };
            exams.Add(newExam);
            return RedirectToAction("CreateExam");
        }

        [HttpPost]
        public IActionResult UpdateExamStatus(int id, string status)
        {
            var exam = exams.FirstOrDefault(e => e.Id == id);
            if (exam != null)
            {
                exam.Status = status;
            }
            return RedirectToAction("ExamResults");
        }

        [HttpPost]
        public IActionResult UpdateMarkingStatus(int id, string status)
        {
            var markingItem = manualMarkingItems.FirstOrDefault(m => m.Id == id);
            if (markingItem != null)
            {
                markingItem.Status = status;
            }
            return RedirectToAction("ManualMarking");
        }

        [HttpPost]
        public IActionResult UpdateProfile(string name, string email, string department, string phone, string office, string officeHours)
        {
            // In a real app, this would update the instructor profile in database
            // For now, just redirect back
            return RedirectToAction("Profile");
        }
    }
}
