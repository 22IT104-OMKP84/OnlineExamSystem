using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq; // Added for FirstOrDefault
using System.Text;
using OnlineExamSystem.Models;

namespace OnlineExamSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // Sample data - in a real app, this would come from a database
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com", Role = "Teacher", Status = "Active" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Role = "Student", Status = "Active" },
            new User { Id = 3, Name = "Mike Johnson", Email = "mike@example.com", Role = "Teacher", Status = "Inactive" }
        };

        private static List<Class> classes = new List<Class>
        {
            new Class { Id = 1, Name = "Mathematics 101", Subject = "Mathematics", Students = 25, Status = "Active" },
            new Class { Id = 2, Name = "Physics 101", Subject = "Physics", Students = 30, Status = "Active" },
            new Class { Id = 3, Name = "Chemistry 101", Subject = "Chemistry", Students = 28, Status = "Pending" }
        };

        // Dynamic data for questions
        private static List<Question> questions = new List<Question>
        {
            new Question { Id = 1, QuestionText = "What is the derivative of x²?", Subject = "Mathematics", Type = "MCQ", Difficulty = "Medium", Status = "Active", Options = new List<string> { "2x", "x²", "2x²", "x" }, CorrectAnswer = "2x" },
            new Question { Id = 2, QuestionText = "Calculate the area of a circle with radius 5", Subject = "Mathematics", Type = "Numeric", Difficulty = "Easy", Status = "Active", CorrectAnswer = "78.54" },
            new Question { Id = 3, QuestionText = "Explain Newton's First Law of Motion", Subject = "Physics", Type = "Essay", Difficulty = "Hard", Status = "Active", CorrectAnswer = "An object at rest stays at rest unless acted upon by an external force." }
        };

        // Dynamic data for exams
        private static List<Exam> exams = new List<Exam>
        {
            new Exam { Id = 1, Title = "Midterm Mathematics", Subject = "Mathematics", Duration = 90, StartDate = "2024-02-01", Status = "Active", Questions = new List<int> { 1, 2 } },
            new Exam { Id = 2, Title = "Physics Final", Subject = "Physics", Duration = 120, StartDate = "2024-02-15", Status = "Draft", Questions = new List<int> { 3 } },
            new Exam { Id = 3, Title = "Chemistry Quiz", Subject = "Chemistry", Duration = 45, StartDate = "2024-01-30", Status = "Completed", Questions = new List<int>() }
        };

        private static List<ExamResult> examResults = new List<ExamResult>
        {
            new ExamResult { Id = 1, StudentName = "Alice Johnson", ExamName = "Mathematics Midterm", Score = 85, TotalMarks = 100, Date = "2024-01-15" },
            new ExamResult { Id = 2, StudentName = "Bob Smith", ExamName = "Physics Final", Score = 92, TotalMarks = 100, Date = "2024-01-16" },
            new ExamResult { Id = 3, StudentName = "Carol Davis", ExamName = "Chemistry Quiz", Score = 78, TotalMarks = 100, Date = "2024-01-17" },
            new ExamResult { Id = 4, StudentName = "David Wilson", ExamName = "Mathematics Midterm", Score = 88, TotalMarks = 100, Date = "2024-01-15" },
            new ExamResult { Id = 5, StudentName = "Eva Brown", ExamName = "Physics Final", Score = 95, TotalMarks = 100, Date = "2024-01-16" }
        };

        private static List<LoginHistory> loginHistory = new List<LoginHistory>
        {
            new LoginHistory { Id = 1, UserName = "john@example.com", Role = "Teacher", LoginTime = "2024-01-20 09:30:00", Status = "Success" },
            new LoginHistory { Id = 2, UserName = "jane@example.com", Role = "Student", LoginTime = "2024-01-20 10:15:00", Status = "Success" },
            new LoginHistory { Id = 3, UserName = "mike@example.com", Role = "Teacher", LoginTime = "2024-01-20 11:00:00", Status = "Failed" },
            new LoginHistory { Id = 4, UserName = "alice@example.com", Role = "Student", LoginTime = "2024-01-20 14:20:00", Status = "Success" },
            new LoginHistory { Id = 5, UserName = "bob@example.com", Role = "Student", LoginTime = "2024-01-20 16:45:00", Status = "Success" }
        };

        // Dynamic data for subjects
        private static List<Subject> subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "Mathematics", Description = "Advanced mathematics including calculus and algebra", Credits = 4, Status = "Active" },
            new Subject { Id = 2, Name = "Physics", Description = "Fundamental physics principles and mechanics", Credits = 3, Status = "Active" },
            new Subject { Id = 3, Name = "Chemistry", Description = "General chemistry and laboratory techniques", Credits = 3, Status = "Active" },
            new Subject { Id = 4, Name = "Biology", Description = "Life sciences and biological systems", Credits = 3, Status = "Active" },
            new Subject { Id = 5, Name = "Computer Science", Description = "Programming and software development", Credits = 4, Status = "Active" }
        };

        // Dynamic data for manual marking
        private static List<ManualMarkingItem> manualMarkingItems = new List<ManualMarkingItem>
        {
            new ManualMarkingItem { Id = 1, StudentName = "John Doe", ExamTitle = "Midterm Mathematics", Subject = "Mathematics", QuestionsPending = 3, Priority = "High", Status = "Pending" },
            new ManualMarkingItem { Id = 2, StudentName = "Jane Smith", ExamTitle = "Physics Final", Subject = "Physics", QuestionsPending = 2, Priority = "Medium", Status = "Pending" },
            new ManualMarkingItem { Id = 3, StudentName = "Mike Johnson", ExamTitle = "Chemistry Quiz", Subject = "Chemistry", QuestionsPending = 1, Priority = "Low", Status = "Pending" },
            new ManualMarkingItem { Id = 4, StudentName = "Alice Brown", ExamTitle = "Mathematics Advanced", Subject = "Mathematics", QuestionsPending = 4, Priority = "High", Status = "Pending" },
            new ManualMarkingItem { Id = 5, StudentName = "Bob Wilson", ExamTitle = "Biology Midterm", Subject = "Biology", QuestionsPending = 2, Priority = "Medium", Status = "In Progress" }
        };

        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            ViewBag.TotalTeachers = users.Count(u => u.Role == "Teacher" && u.Status == "Active");
            ViewBag.TotalStudents = users.Count(u => u.Role == "Student" && u.Status == "Active");
            ViewBag.ActiveClasses = classes.Count(c => c.Status == "Active");
            ViewBag.PendingClasses = classes.Count(c => c.Status == "Pending");
            return View();
        }

        public IActionResult GeneralSettings()
        {
            ViewData["Title"] = "General Settings";
            return View();
        }

        public IActionResult ClassManagement()
        {
            ViewData["Title"] = "Class Management";
            ViewBag.Classes = classes;
            return View();
        }

        public IActionResult UserManagement()
        {
            ViewData["Title"] = "User Management";
            ViewBag.Users = users;
            return View();
        }

        public IActionResult SubjectManagement()
        {
            ViewData["Title"] = "Subject Management";
            ViewBag.Subjects = subjects;
            return View();
        }

        public IActionResult QuestionManagement()
        {
            ViewData["Title"] = "Question Management";
            ViewBag.Questions = questions;
            ViewBag.Subjects = new List<string> { "Mathematics", "Physics", "Chemistry", "Biology", "Computer Science" };
            ViewBag.QuestionTypes = new List<string> { "MCQ", "Numeric", "Essay", "True/False" };
            ViewBag.DifficultyLevels = new List<string> { "Easy", "Medium", "Hard" };
            return View();
        }

        public IActionResult ExamManagement()
        {
            ViewData["Title"] = "Exam Management";
            ViewBag.Exams = exams;
            ViewBag.Subjects = new List<string> { "Mathematics", "Physics", "Chemistry", "Biology", "Computer Science" };
            ViewBag.Questions = questions;
            return View();
        }

        public IActionResult ExamResult()
        {
            ViewData["Title"] = "Exam Result";
            ViewBag.ExamResults = examResults;
            return View();
        }

        public IActionResult ManualMarking()
        {
            ViewData["Title"] = "Manual Marking";
            ViewBag.ManualMarkingItems = manualMarkingItems;
            return View();
        }

        public IActionResult LoginHistory()
        {
            ViewData["Title"] = "Login History";
            ViewBag.LoginHistory = loginHistory;
            return View();
        }

        // Question Management Actions
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
            return RedirectToAction("QuestionManagement");
        }

        [HttpPost]
        public IActionResult DeleteQuestion(int id)
        {
            var question = questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
            {
                questions.Remove(question);
            }
            return RedirectToAction("QuestionManagement");
        }

        [HttpPost]
        public IActionResult UpdateQuestionStatus(int id, string status)
        {
            var question = questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
            {
                question.Status = status;
            }
            return RedirectToAction("QuestionManagement");
        }

        // Exam Management Actions
        [HttpPost]
        public IActionResult AddExam(string title, string subject, int duration, string startDate, int[] questionIds)
        {
            var newExam = new Exam
            {
                Id = exams.Count > 0 ? exams.Max(e => e.Id) + 1 : 1,
                Title = title,
                Subject = subject,
                Duration = duration,
                StartDate = startDate,
                Status = "Draft",
                Questions = questionIds?.ToList() ?? new List<int>()
            };
            exams.Add(newExam);
            return RedirectToAction("ExamManagement");
        }

        [HttpPost]
        public IActionResult DeleteExam(int id)
        {
            var exam = exams.FirstOrDefault(e => e.Id == id);
            if (exam != null)
            {
                exams.Remove(exam);
            }
            return RedirectToAction("ExamManagement");
        }

        [HttpPost]
        public IActionResult UpdateExamStatus(int id, string status)
        {
            var exam = exams.FirstOrDefault(e => e.Id == id);
            if (exam != null)
            {
                exam.Status = status;
            }
            return RedirectToAction("ExamManagement");
        }

        [HttpPost]
        public IActionResult AddQuestionsToExam(int examId, int[] questionIds)
        {
            var exam = exams.FirstOrDefault(e => e.Id == examId);
            if (exam != null && questionIds != null)
            {
                foreach (var questionId in questionIds)
                {
                    if (!exam.Questions.Contains(questionId))
                    {
                        exam.Questions.Add(questionId);
                    }
                }
            }
            return RedirectToAction("ExamManagement");
        }

        // New action methods for dashboard functionality
        [HttpPost]
        public IActionResult StartAllOperations()
        {
            try
            {
                // Simulate starting all pending operations
                // In a real application, this would start background jobs, processes, etc.
                
                // Update pending classes to active
                foreach (var classItem in classes.Where(c => c.Status == "Pending"))
                {
                    classItem.Status = "Active";
                }

                // Simulate processing time
                System.Threading.Thread.Sleep(1000);

                return Json(new { success = true, message = "All operations started successfully" });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult DownloadResults()
        {
            try
            {
                // Create CSV content
                var csv = new StringBuilder();
                csv.AppendLine("Student Name,Exam Name,Score,Total Marks,Date");
                
                foreach (var result in examResults)
                {
                    csv.AppendLine($"{result.StudentName},{result.ExamName},{result.Score},{result.TotalMarks},{result.Date}");
                }

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(bytes, "text/csv", "exam_results.csv");
            }
            catch (System.Exception ex)
            {
                return Content($"Error generating download: {ex.Message}");
            }
        }

        public IActionResult DownloadIndividualResult(int id)
        {
            try
            {
                var result = examResults.FirstOrDefault(r => r.Id == id);
                if (result == null)
                {
                    return Content("Result not found");
                }

                // Create individual result content (simulating PDF content as text)
                var content = new StringBuilder();
                content.AppendLine($"EXAM RESULT REPORT");
                content.AppendLine($"==================");
                content.AppendLine($"Student Name: {result.StudentName}");
                content.AppendLine($"Exam Name: {result.ExamName}");
                content.AppendLine($"Score: {result.Score}/{result.TotalMarks}");
                content.AppendLine($"Percentage: {Math.Round((double)result.Score / result.TotalMarks * 100, 1)}%");
                content.AppendLine($"Date: {result.Date}");
                content.AppendLine($"Status: {(result.Score >= 60 ? "Passed" : "Failed")}");

                var bytes = Encoding.UTF8.GetBytes(content.ToString());
                return File(bytes, "text/plain", $"exam_result_{id}.txt");
            }
            catch (System.Exception ex)
            {
                return Content($"Error generating download: {ex.Message}");
            }
        }

        public IActionResult DownloadDetailedResult(int id)
        {
            try
            {
                var result = examResults.FirstOrDefault(r => r.Id == id);
                if (result == null)
                {
                    return Content("Result not found");
                }

                // Create detailed result content
                var content = new StringBuilder();
                content.AppendLine($"DETAILED EXAM RESULT REPORT");
                content.AppendLine($"============================");
                content.AppendLine($"Student Name: {result.StudentName}");
                content.AppendLine($"Exam Name: {result.ExamName}");
                content.AppendLine($"Score: {result.Score}/{result.TotalMarks}");
                content.AppendLine($"Percentage: {Math.Round((double)result.Score / result.TotalMarks * 100, 1)}%");
                content.AppendLine($"Date: {result.Date}");
                content.AppendLine($"Status: {(result.Score >= 60 ? "Passed" : "Failed")}");
                content.AppendLine();
                content.AppendLine("QUESTION ANALYSIS:");
                content.AppendLine("Question 1: Correct (10/10)");
                content.AppendLine("Question 2: Incorrect (0/10)");
                content.AppendLine("Question 3: Correct (10/10)");
                content.AppendLine("Question 4: Partially Correct (5/10)");
                content.AppendLine("Question 5: Correct (10/10)");

                var bytes = Encoding.UTF8.GetBytes(content.ToString());
                return File(bytes, "text/plain", $"detailed_result_{id}.txt");
            }
            catch (System.Exception ex)
            {
                return Content($"Error generating download: {ex.Message}");
            }
        }

        // Dynamic actions for user management
        [HttpPost]
        public IActionResult AddUser(string name, string email, string role)
        {
            var newUser = new User
            {
                Id = users.Count + 1,
                Name = name,
                Email = email,
                Role = role,
                Status = "Active"
            };
            users.Add(newUser);
            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
            }
            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public IActionResult UpdateUserStatus(int id, string status)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Status = status;
            }
            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public IActionResult UpdateUser(int id, string name, string email, string role)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Name = name;
                user.Email = email;
                user.Role = role;
            }
            return RedirectToAction("UserManagement");
        }

        // Dynamic actions for class management
        [HttpPost]
        public IActionResult AddClass(string name, string subject)
        {
            var newClass = new Class
            {
                Id = classes.Count + 1,
                Name = name,
                Subject = subject,
                Students = 0,
                Status = "Pending"
            };
            classes.Add(newClass);
            return RedirectToAction("ClassManagement");
        }

        [HttpPost]
        public IActionResult DeleteClass(int id)
        {
            var classItem = classes.FirstOrDefault(c => c.Id == id);
            if (classItem != null)
            {
                classes.Remove(classItem);
            }
            return RedirectToAction("ClassManagement");
        }

        [HttpPost]
        public IActionResult UpdateClassStatus(int id, string status)
        {
            var classItem = classes.FirstOrDefault(c => c.Id == id);
            if (classItem != null)
            {
                classItem.Status = status;
            }
            return RedirectToAction("ClassManagement");
        }

        [HttpPost]
        public IActionResult UpdateClass(int id, string name, string subject)
        {
            var classItem = classes.FirstOrDefault(c => c.Id == id);
            if (classItem != null)
            {
                classItem.Name = name;
                classItem.Subject = subject;
            }
            return RedirectToAction("ClassManagement");
        }

        // Dynamic actions for subject management
        [HttpPost]
        public IActionResult AddSubject(string name, string description, int credits)
        {
            var newSubject = new Subject
            {
                Id = subjects.Count + 1,
                Name = name,
                Description = description,
                Credits = credits,
                Status = "Active"
            };
            subjects.Add(newSubject);
            return RedirectToAction("SubjectManagement");
        }

        [HttpPost]
        public IActionResult DeleteSubject(int id)
        {
            var subject = subjects.FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                subjects.Remove(subject);
            }
            return RedirectToAction("SubjectManagement");
        }

        [HttpPost]
        public IActionResult UpdateSubjectStatus(int id, string status)
        {
            var subject = subjects.FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                subject.Status = status;
            }
            return RedirectToAction("SubjectManagement");
        }

        [HttpPost]
        public IActionResult UpdateSubject(int id, string name, string description, int credits)
        {
            var subject = subjects.FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                subject.Name = name;
                subject.Description = description;
                subject.Credits = credits;
            }
            return RedirectToAction("SubjectManagement");
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
    }
}
