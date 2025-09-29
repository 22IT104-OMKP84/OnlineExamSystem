using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineExamSystem.Models;
using OnlineExamSystem.Services;
using System.Security.Claims;

namespace OnlineExamSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserService userService, IJwtService jwtService, ILogger<LoginController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // If user is already authenticated, redirect to appropriate dashboard
            if (User.Identity?.IsAuthenticated == true)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                return role switch
                {
                    "Admin" => RedirectToAction("Dashboard", "Admin"),
                    "Instructor" => RedirectToAction("Index", "Instructor"),
                    "Student" => RedirectToAction("Index", "Student"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Please provide valid email and password.";
                    return View("Index", request);
                }

                var user = await _userService.AuthenticateAsync(request.Email, request.Password);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Invalid email or password.";
                    return View("Index", request);
                }

                // Generate JWT token
                var token = _jwtService.GenerateAccessToken(user);

                // Store token in cookie for web interface
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("jwt_token", token, cookieOptions);

                _logger.LogInformation($"User {user.Email} logged in successfully via web interface");

                // Redirect to appropriate dashboard based on role
                return user.Role switch
                {
                    "Admin" => RedirectToAction("Dashboard", "Admin"),
                    "Instructor" => RedirectToAction("Index", "Instructor"),
                    "Student" => RedirectToAction("Index", "Student"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                ViewBag.ErrorMessage = "An error occurred during login. Please try again.";
                return View("Index", request);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Please provide valid information.";
                    return View(request);
                }

                var user = await _userService.RegisterAsync(request);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "User with this email already exists.";
                    return View(request);
                }

                // Generate JWT token
                var token = _jwtService.GenerateAccessToken(user);

                // Store token in cookie for web interface
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("jwt_token", token, cookieOptions);

                _logger.LogInformation($"New user registered: {user.Email} with role: {user.Role}");

                ViewBag.SuccessMessage = "Registration successful! Welcome to the Online Exam System.";

                // Redirect to appropriate dashboard based on role
                return user.Role switch
                {
                    "Admin" => RedirectToAction("Dashboard", "Admin"),
                    "Instructor" => RedirectToAction("Index", "Instructor"),
                    "Student" => RedirectToAction("Index", "Student"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                ViewBag.ErrorMessage = "An error occurred during registration. Please try again.";
                return View(request);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            // Remove JWT token cookie
            Response.Cookies.Delete("jwt_token");
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"User {userId} logged out successfully");

            return RedirectToAction("Index", "Home");
        }
    }
}
